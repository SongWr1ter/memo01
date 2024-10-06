using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public BulletInfo info;
    protected int baseDamage;
    protected float speed;
    protected float deadTime;protected float deadtime=0f;
    [SerializeField]
    protected bool canDamagePlayer=false;
    protected int charcDamage;
    TrailRenderer trail;

    protected void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    protected void OnEnable() {
        baseDamage = info.baseDamage;
        speed = info.speed;
        deadTime = info.deadTime;
        charcDamage = GlobalResManager.Instance.
                        Player.GetComponent<PlayerInfo>().playerPropertes.Strength;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(speed * transform.up
        * Time.deltaTime,Space.World);


        deadtime += Time.deltaTime;
        if(deadtime >= deadTime) {
            CollectGarbage();
            deadtime -= deadTime;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Obstacle"))
        {
            //播放枪打到墙上的特效
            CollectGarbage();
            return;
        }
        if(other.CompareTag("Enemy") && !canDamagePlayer)
        {
            //other.GetComponent<simpleFSM>().GetDamaged(transform.up);
            other.GetComponent<IDamageable>().GetDamaged(baseDamage + charcDamage);
            //Debug.Log("Hit Enemy;Damgage = "+ (baseDamage + charcDamage));
            CollectGarbage();
            return;
        }
        if(other.CompareTag("Player") && canDamagePlayer)
        {
            other.GetComponent<IDamageable>().GetDamaged(baseDamage);
            CollectGarbage();
        }
    }

    void CollectGarbage()
    {
        if (trail != null)
        {
            GetComponent<TrailRenderer>().Clear();
        }
        
        ObjectPool.Instance.PushObject(gameObject);
    }
}
