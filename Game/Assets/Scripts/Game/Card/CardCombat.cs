using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardCombat : MonoBehaviour {

    Text cardHealth;
    Text cardAttack;

	// Use this for initialization
	void Start () {
        cardHealth = transform.Find("CardInfo/CardHealth/CardHealthText").GetComponent<Text>();
        cardAttack = transform.Find("CardInfo/CardAttack/CardAttackText").GetComponent<Text>();
    }

    public void RecieveDamage(int amount)
    {
        Animation anim = GetComponent<Animation>();
        anim.Play("RecieveDamageAnimation");

        int health = int.Parse(cardHealth.text);
        health -= amount;
        cardHealth.text = health.ToString();
    }
}
