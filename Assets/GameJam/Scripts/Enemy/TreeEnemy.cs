using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnemy : BaseEnemy
{
    protected override void FixedUpdate()
    {
        CheckDist();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().GetDamaged(1);
        }
    }
}
