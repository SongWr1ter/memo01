using UnityEngine;

public class Enemy5 : BaseEnemy
{
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Shoot(Vector3 targetDir)
    {
        animator.Play("attack");
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().GetDamaged(1);
        }
    }

}
