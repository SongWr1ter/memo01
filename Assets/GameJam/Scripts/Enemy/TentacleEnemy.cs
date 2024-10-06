using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleEnemy : BaseEnemy
{

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamageable>().GetDamaged(1);
        }
    }
}
