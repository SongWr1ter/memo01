using UnityEngine;

public class Enemy5 : BaseEnemy
{
    private GameObject explosionPrefab;
    private Animator animator;
    private bool flag;//has trigger exploded?
    private float speed;

    protected override void Start()
    {
        base.Start();
        speed = paramater.speed;
        explosionPrefab = GlobalResManager.Instance.ExplosionPrefab;
        animator = GetComponent<Animator>();
        flag = false;
    }

    protected override void Shoot(Vector3 targetDir)
    {
        //sui
        if (!flag)
        {
            suicide();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().GetDamaged(1);
        }
    }

    void suicide()
    {
        flag = true;
        paramater.invincible = true;
        
        paramater.speed = 0f;
        sSoundManager.PlayAudio("sfx_lowhealth_alarmloop1");
        Invoke("Dead", 1f);
    }

    protected override void Dead()
    {
        //boom!!
        sSoundManager.PlayOneAudio("Explosion_Blood_01");
        flag = false;
        paramater.invincible = false;
        base.Dead();
        paramater.speed = speed;
        ObjectPool.Instance.GetObject(explosionPrefab).transform.position = transform.position;
        
    }

}
