using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float radius;
    [SerializeField]
    private int damage;
    // Start is called before the first frame update
    //void OnEnable()
    //{
    //    int len = Physics2D.OverlapCircleNonAlloc(transform.position, radius, colls,layer);
        
    //    if (len > 0)
    //    {
    //        for (int i = 0; i < len; i++) {
    //            print(colls[i].name);
    //            IDamageable damageable;
    //            colls[i].TryGetComponent<IDamageable>(out damageable);
    //            if (damageable != null)
    //            {
    //                damageable.GetDamaged(damage);
    //            }
    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IDamageable>() != null)
        {
            collision.GetComponent<IDamageable>().GetDamaged(damage);
        }
    }

    void Gone()
    {
        ObjectPool.Instance.PushObject(gameObject);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, radius);
    //}

}
