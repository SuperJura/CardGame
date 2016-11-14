using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public static string combatMsg;

    private int enemyID;

    void Awake()
    {
        DontDestroyOnLoad(transform.parent.parent.gameObject);
        enemyID = transform.GetComponentInParent<EnemyInfo>().enemyID;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!CharacterMovement.instance.isMoving && EnemyCombatFlags.enemysPerLevel[enemyID])
        {
            AdventureGame.SaveGame();
            EnemyCombatFlags.enemysPerLevel[enemyID] = false;
            string[] cards = GetComponentInParent<EnemyInfo>().cardIDs;
            combatMsg = GetComponentInParent<EnemyInfo>().combatMsg;

            ICardDatabase repo = Repository.GetCardDatabaseInstance();
            if (AdventureDeck.DeckOpponent != null) AdventureDeck.DeckOpponent.Clear();
            else AdventureDeck.DeckOpponent = new List<Card>();
            foreach (string card in cards)
            {
                AdventureDeck.DeckOpponent.Add(repo.GetCard(card));
            }
            SceneManager.LoadScene("AdventureCombat");
        }
    }
}