using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Enemy : MonoBehaviourPun
{
    [Header("Info")]
    public string enemyName;
    public float moveSpeed;

    public bool isBoss;
    public int curHp;
    public int maxHp;

    public float chaseRange;
    public float attackRange;

    private PlayerController targetPlayer;

    public float playerDetectRate = 0.2f;
    private float lastPlayerDetectTime;

    public string objectToSpawnOnDeath;

    [Header("Attack")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;

    [Header("Components")]
    public HeaderInfo healthBar;
    private SpriteRenderer sr;
    private Rigidbody2D rig;

    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        rig = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        //scales boss health based on players in lobby
        if (isBoss)
        {
            maxHp *= PhotonNetwork.PlayerList.Length;
            curHp = maxHp;
        }
        healthBar.Initialize(enemyName, maxHp);
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if(targetPlayer != null)
        {
            float dist = Vector3.Distance(transform.position, targetPlayer.transform.position);

            if(dist < attackRange && Time.time - lastAttackTime >= attackRange)
            {
                Attack();
            }
            else if(dist > attackRange)
            {
                Vector3 dir = targetPlayer.transform.position - transform.position;
                rig.velocity = dir.normalized * moveSpeed;
            }
            else
            {
                rig.velocity = Vector2.zero;
            }
        }

        DetectPlayer();
    }

    private void Attack()
    {
        Debug.Log("EnemyAttack");
        lastAttackTime = Time.time;
        targetPlayer.photonView.RPC("TakeDamage", targetPlayer.photonPlayer, damage);
    }

    private void DetectPlayer()
    {
        if(Time.time - lastPlayerDetectTime > playerDetectRate)
        {
            lastPlayerDetectTime = Time.time;
            foreach(PlayerController player in GameManager.instance.players)
            {
                float dist = Vector2.Distance(transform.position, player.transform.position);
                if(player == targetPlayer)
                {
                    if(dist > chaseRange)
                    {
                        targetPlayer = null;
                    }
                }
                else if (dist < chaseRange)
                {
                    if (targetPlayer == null)
                    {
                        targetPlayer = player;
                    }
                }
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage)
    {
        curHp -= damage;

        healthBar.photonView.RPC("UpdateHealthBar", RpcTarget.All, curHp);

        if(curHp <= 0)
        {
            Die();
        }
        else
        {
            photonView.RPC("FlashDamage", RpcTarget.All);
        }
    }

    private void Die()
    {
        if(objectToSpawnOnDeath != string.Empty)
        {
            PhotonNetwork.Instantiate(objectToSpawnOnDeath, transform.position, Quaternion.identity);
        }

        PhotonNetwork.Destroy(gameObject);
    }

    [PunRPC]
    void FlashDamage()
    {
        StartCoroutine(DamageFlash());

        IEnumerator DamageFlash()
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.05f);
            sr.color = Color.white;
        }
    }
}
