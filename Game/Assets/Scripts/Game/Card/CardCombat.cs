using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardCombat : MonoBehaviour {

    Text cardHealth;
    Text cardAttack;
    Text cardCooldown;
    Animation anim;

    // Use this for initialization
    void Start () {
        cardHealth = transform.Find("CardInfo/CardHealth/CardHealthText").GetComponent<Text>();
        cardAttack = transform.Find("CardInfo/CardAttack/CardAttackText").GetComponent<Text>();
        cardCooldown = transform.Find("CardInfo/CardCooldown/CardCooldownText").GetComponentInChildren<Text>();
        anim = GetComponent<Animation>();
    }

    public void RecieveDamage(int amount)
    {
        anim.Play("RecieveDamageAnimation");
        int health = int.Parse(cardHealth.text);
        health -= amount;
        cardHealth.text = health.ToString();
    }

    public void DecreaseCooldown()
    {
        anim.Play("CooldownAnimation");

        int cooldown = int.Parse(cardCooldown.text);
        if (cooldown == 0)
        {
            return;
        }
        cardCooldown.text = (--cooldown).ToString();
    }
}
