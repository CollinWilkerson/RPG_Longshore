using UnityEngine;
using UnityEngine.UI;

public class PortraitController : MonoBehaviour
{
    private Image portrait;

    private void Awake()
    {
        portrait = gameObject.GetComponent<Image>();
    }

    public void SetPortrait(Sprite npcPortrait)
    {
        portrait.sprite = npcPortrait;
    }
}
