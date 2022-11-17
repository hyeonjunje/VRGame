using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class EventManager : MonoBehaviour
{
    public static event Action enemyDieEvent;

    public static void RunEnemyDiesEvent()
    {
        if (enemyDieEvent != null)
            enemyDieEvent();
    }


    public static event Action shootEvent;

    public static void RunShootEvent()
    {
        if (shootEvent != null)
            shootEvent();
    }
}
