using UnityEngine;
using System;

[Serializable]
public class EnemyParamater
{
    public float speed;
    public float turnSpeed;
    public int health;
    public float attackDistance;
    public GameObject BulletPrefab;
    public float attackInterval;
    public int score;
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
    [SerializeField]
    protected GameObject Target;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Target = GlobalResManager.Instance.Player;
        rigi = GetComponent<Rigidbody2D>();
    }
    public int health {get;set;}
    Quaternion rotation = Quaternion.identity;
    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
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
        Vector3 desiredForward = Vector3.RotateTowards(-transform.up,targetDir,paramater.turnSpeed * Time.deltaTime,0);
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
        //rotation = Quaternion.LookRotation(desiredForward,Vector3.forward);
        //transform.rotation *= rotation;
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
        rigi.velocity = Vector3.zero;
        ObjectPool.Instance.PushObject(this.gameObject);
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
        health -= _damage;
    } 

    protected virtual void EventRegister()
    {

    }

    protected virtual void EventDeleter()
    {

    }
}
