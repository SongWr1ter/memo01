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
            ObjectPool.Instance.PushObject(gameObject);
            deadtime -= deadTime;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Obstacle"))
        {
            //播放枪打到墙上的特效
            Destroy(gameObject);
            return;
        }
        if(other.CompareTag("Enemy") && !canDamagePlayer)
        {
            //other.GetComponent<simpleFSM>().GetDamaged(transform.up);
            other.GetComponent<IDamageable>().GetDamaged(baseDamage + charcDamage);
            //Debug.Log("Hit Enemy;Damgage = "+ (baseDamage + charcDamage));
            ObjectPool.Instance.PushObject(gameObject);
            return;
        }
        if(other.CompareTag("Player") && canDamagePlayer)
        {
            other.GetComponent<IDamageable>().GetDamaged(baseDamage);
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
