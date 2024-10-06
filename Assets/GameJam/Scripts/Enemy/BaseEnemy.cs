using UnityEngine;
using System;
using UnityEditor.Tilemaps;

[Serializable]
public class EnemyParamater
{
    public float speed;
    public float turnSpeed;
    public int health;
    public bool invincible;
    public float attackDistance;
    public GameObject BulletPrefab;
    public float attackInterval;
    public int score;
    public bool isRotate;
}
public class BaseEnemy : MonoBehaviour,IDamageable
{
    public enum eState{
        Chase,Attack,Hurt,Dead
    }
    public EnemyParamater paramater;
    protected eState curState;
    protected Rigidbody2D rigi;
    protected void OnEnable() {

        curState = eState.Chase;
        health = paramater.health;
        if (GlobalResManager.isInitialize)
        {
            Target = GlobalResManager.Instance.Player;
        }
        EventRegister();
    }
    protected GameObject Target;
    protected SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Target = GlobalResManager.Instance.Player;
        rigi = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public int health {get;set;}
    Quaternion rotation = Quaternion.identity;
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        Flip();
        CheckDist();
        switch (curState)
        {
            case eState.Chase:
            default:
                Chase();
                break;
            case eState.Attack:
                Attack();
                break;
            case eState.Dead:
                Dead();
                break;
            case eState.Hurt:
                Hurt();
                break;
        }
    }

    protected void OnDisalbe()
    {
        EventDeleter();
    }

    protected void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,paramater.attackDistance);
    }

    protected virtual void Chase()
    {
        Vector3 targetDir = Target.transform.position - transform.position;
        targetDir.z = 0;
        rigi.velocity = targetDir.normalized * paramater.speed;
        /*条件转移判断*/
        if(health <= 0)//Dead
        {
            rigi.velocity = Vector3.zero;
            StateTranslator(eState.Dead);
        }
        else if(targetDir.magnitude < paramater.attackDistance)
        {
            rigi.velocity = Vector3.zero;
            StateTranslator(eState.Attack);
        }
        
    }
    protected float attackTime;
    protected void Attack()
    {
        Vector3 targetDir = Target.transform.position - transform.position;
        if(attackTime >= paramater.attackInterval)
        {
            Shoot(targetDir);
        }
        else
        {
            attackTime += Time.deltaTime;
        }

        /*条件转移判断*/
        if(health <= 0)//Dead
        {
            StateTranslator(eState.Dead);
        }
        else if(targetDir.magnitude > paramater.attackDistance)
        {
            StateTranslator(eState.Chase);
        }

    }

    protected virtual void Shoot(Vector3 targetDir)
    {
        attackTime -= paramater.attackInterval;
        targetDir.z = 0;
        GameObject _bullet = ObjectPool.Instance.GetObject(paramater.BulletPrefab);
        _bullet.transform.position = transform.position;
        _bullet.transform.rotation = Quaternion.identity;
        Vector3 curDir = Vector3.up;
        float angle = Vector3.SignedAngle(curDir,targetDir,Vector3.forward);
        Quaternion quaternion = Quaternion.AngleAxis(angle,Vector3.forward);
        _bullet.transform.rotation *= quaternion;
    }

    protected virtual void Hurt()
    {

    }

    protected virtual void Dead()
    {
        //TODO: 死亡特效
        BodyCollect();
        MessageCenter.SendMessage(new CommonMessage()
        {
            Mid = (int)MESSAGE_TYPE.ADD_SCORE,
            intParam = paramater.score
        });
    }

    protected void StateTranslator(eState targetState)
    {
        curState = targetState;
    }

    public void GetDamaged(int _damage)
    {
        if(paramater.invincible) {return; }
        health -= _damage;
    } 

    protected void Flip()
    {
        Vector3 mstr2player = Target.transform.position - transform.position ;


        //rotate
        if (!paramater.isRotate)
        {
            float dot = Vector3.Dot(transform.right, mstr2player.normalized);
            if (dot > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
        else {
            mstr2player.z = 0;
            Vector3 desiredForward = Vector3.RotateTowards(transform.right, mstr2player, paramater.turnSpeed * Time.deltaTime, 0);
            rotation = Quaternion.FromToRotation(transform.right, desiredForward);
            transform.rotation *= rotation;
        }
        
    }

    protected void CheckDist()
    {
        float d = Vector3.Distance(Target.transform.position, transform.position);
        if (d >= GlobalResManager.Instance.maxDeleteDist)
        {
            BodyCollect();
        }
    }

    void BodyCollect()
    {
        rigi.velocity = Vector3.zero;
        ObjectPool.Instance.PushObject(this.gameObject);
        MessageCenter.SendMessage(new CommonMessage
        {
            Mid = (int)MESSAGE_TYPE.ENEMY_DEAD
        });
    }

    protected void OnGameWin(CommonMessage msg)
    {
        if (msg.Mid != (int)MESSAGE_TYPE.WIN) return;
        //TODO: 死亡特效
        BodyCollect();

    }

    protected virtual void EventRegister()
    {
        MessageCenter.AddListener(OnGameWin);
    }

    protected virtual void EventDeleter()
    {
        MessageCenter.RemoveListner(OnGameWin);
    }
}
