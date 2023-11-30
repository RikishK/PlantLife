using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidEnemy : Enemy
{
    [SerializeField] private GameObject deadAphid;

    protected override void DieExtras()
    {
        GameObject deadAphidObj = Instantiate(deadAphid);
        deadAphidObj.transform.position = transform.position;
    }
}
