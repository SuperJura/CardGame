using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class EnemyCombatFlags
{
    //sluze za to kada player se bori protiv jednog, da se nece boriti vise puta. kada borba zavrsi, enemijev flag postane false
    public static Dictionary<int, bool> enemysPerLevel;//njegov id, bool dali se vec borio protiv njega

    static EnemyCombatFlags()
    {
        enemysPerLevel = new Dictionary<int, bool>();
    }

}
