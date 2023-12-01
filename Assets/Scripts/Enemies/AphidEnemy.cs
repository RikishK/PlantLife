using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AphidEnemy : Enemy
{
    [SerializeField] private GameObject deadAphid;

    protected override void DieExtras()
    {
        float chance = Random.Range(1f, 100f);
        if(chance > 75){
            GameObject deadAphidObj = Instantiate(deadAphid);
            deadAphidObj.transform.position = transform.position;
        }
    }
}
