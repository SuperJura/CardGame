using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EnemyCombatFlags
{
    //sluze za to kada player se bori protiv jednog, da se nece boriti vise puta. kada borba zavrsi, enemijev flag postane false
    public static Dictionary<string, bool[]> enemysPerLevel;

    static EnemyCombatFlags()
    {
        enemysPerLevel = new Dictionary<string, bool[]>();
        enemysPerLevel.Add("Tutorial_1", new [] {true});
    }

}
