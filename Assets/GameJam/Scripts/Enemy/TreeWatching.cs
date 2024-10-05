using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeWatching : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private LayerMask layerMask;
    private Animator anim;
    
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (Physics2D.OverlapCircle(transform.position, radius, layerMask))
        {
            anim.SetBool("watch", true);
        }
        else
        {
            anim.SetBool("watch", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
