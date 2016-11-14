using UnityEngine;
using System.Collections;

public class EnemyInfo : MonoBehaviour
{
    //enemy id se veze uz njegov bool u EnemyCombatFlag
    [HideInInspector]
    public int enemyID;
    public string combatMsg;
    public string[] cardIDs;

    private static int idCounter = 0;
    void Awake()
    {
        enemyID = ++idCounter;
        EnemyCombatFlags.enemysPerLevel.Add(enemyID, true);
        Debug.Log(enemyID + " " + EnemyCombatFlags.enemysPerLevel[enemyID]);
    }
}