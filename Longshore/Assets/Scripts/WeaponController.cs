using NUnit.Compatibility;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Style
{
    Axe
}

public class WeaponController : MonoBehaviourPun
{
    private int damage;
    private float attackRange;
    private float attackSweep;
    private float attackRate;
    private float lastAttackTime;

    public LayerMask enemyMask;
    private Style weaponStyle;

    private Animator weaponAnim;

    private void Start()
    {
        weaponAnim = GetComponentInChildren<Animator>();
    }

    [PunRPC]
    public void SetWeapon(string type, int newDamage, float newRange, float newRate, float newSweep)
    {
        if(type == "Axe")
        {
            weaponStyle = Style.Axe;
            attackSweep = newSweep;
        }

        damage = newDamage; 
        attackRange = newRange;
        attackRate = newRate;
    }

    public void Attack()
    {
        //prevents attack from executing if the weapon is not set up
        if(weaponAnim == null)
        {
            return;
        }
        //returns if the attack exeeds its rate
        if(Time.time - lastAttackTime < attackRate)
        {
            return;
        }

        Vector3 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;

        if (weaponStyle == Style.Axe)
        {
            //gets all enemies in sweep range
            Collider2D[] hitCheck = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyMask);

            foreach (Collider2D enemy in hitCheck) {
                Vector2 directionToEnemy = (enemy.transform.position - transform.position).normalized;

                //narrows down enemies based on sweep angle
                if (Vector2.Angle(dir, directionToEnemy) < attackSweep / 2)
                {
                    //not checking obstructions for now, range should be too low anyway
                    enemy.GetComponent<Enemy>().photonView.RPC("TakeDamage", RpcTarget.MasterClient, damage);
                }
            }
        }

        lastAttackTime = Time.time;
        weaponAnim.SetTrigger("Attack");
    }
}
