using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    enum State
    {
        Normal,
        Roll
    }
    public float speed;
    public LayerMask floorMask;
    public float attackSpeed;
    public float excuteRadius;
    [Tooltip("角色一直盯着看的物体")]
    public Transform AimingTarget;
    private Vector3 mov;

    private Transform weaponTrans;
    private Animator weaponAnim;

    private Rigidbody2D rigi;

    private bool isDead;
    private Vector3 oriPos;

    #region Roll
    private Vector3 rollDir;
    private State state;
    [SerializeField] private float rollSpeed;
    private LayerMask oriLayer;
    float _rollspeed;
    private Vector3 lastMov;
    private bool isStageCleared;
    #endregion

    #region 手感优化待用
    // [HideInInspector]
    // [System.Runtime.InteropServices.DllImport("user32.dll")]
    // public static extern int SetCursorPos(int x, int y);
    // [HideInInspector]
    // [System.Runtime.InteropServices.DllImport("user32.dll")]
    // private static extern bool GetCursorPos(out POINT pt);
    // private void KeepCursorCenter(){
    //     if (HideCursor){
    //         SetCursorPos((int)Screen.width / 2, (int)Screen.height / 2);
    //     }

    //     if (Input.GetKeyDown(KeyCode.Escape)){
    //         HideCursor = false;
    //         Cursor.visible = true;
    //     }
    // }
    #endregion

    private void Awake() {
        MessageCenter.AddListener(OnGameRestart);
        MessageCenter.AddListener(OnMovNexStage);
        MessageCenter.AddListener(OnStageClear);
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponTrans = transform.GetChild(0);
        weaponAnim = weaponTrans.GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
        MouseMove mouseMove;
        if(TryGetComponent<MouseMove>(out mouseMove))
        {
            AimingTarget = mouseMove.target.transform;
        }

        oriPos = transform.position;
        oriLayer = gameObject.layer;
        state = State.Normal;
        _rollspeed = rollSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Normal:
            if(isDead)
            {
                return;
            }
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            mov = new Vector3(h,v);
            if(h != 0 || v != 0)
            {
                //not idle
                lastMov = mov;
            }
            
            turn();
            if(Input.GetMouseButton(0))
            {
                attack();
            }
            if(Input.GetKeyDown(KeyCode.Space))
            {
                rollDir = lastMov;
                state = State.Roll;
                rollSpeed = _rollspeed;
                gameObject.layer = 3;//gameobect.layer和Phycis.Raycast的layer赋
                                    //值好像不一样，前者使用【位掩码的序号】来赋值
                                    //我刚才的操作LayerMask.GetMask()是把二进制掩码转换为十进制整数来赋值了
                                    //比如DownEnemy位掩码 = 0000······01000，转换为十进制整数就是8
                                    //而8正好对应位掩码0000···100000000是Enemy层所以才会出现bug
                                    //正确方法是用0000······01000对应的【位掩码的序号】3
            }
            break;
            case State.Roll:
            float multiplier = 5f;
            rollSpeed -= rollSpeed * multiplier * Time.deltaTime;
            if(rollSpeed <= 10f)
            {
                state = State.Normal;
                gameObject.layer = oriLayer;
            }
            break;
        }
        
    }

    private void FixedUpdate() {
        switch(state)
        {
            case State.Normal:
                rigi.velocity = mov.normalized * speed;
            break;
            case State.Roll:
                rigi.velocity = rollDir.normalized * rollSpeed;
            break;
        }
        
    }

    private void OnDestroy() {
        MessageCenter.RemoveListner(OnGameRestart);
        MessageCenter.RemoveListner(OnMovNexStage);
        MessageCenter.RemoveListner(OnStageClear);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position,excuteRadius);
    }

    #region 动作,行动
    //角色y轴始终朝向鼠标
    void turn()
    {
        //transform.rotation = Quaternion.LookRotation(); 不能用因为LookAt是Z轴朝向目标方向
        if(AimingTarget == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit ,1000f,floorMask))
            {
                Vector3 player2mouse = hit.point - transform.position;
                player2mouse.z = 0;
                //计算角度
                float angle = Vector3.SignedAngle(transform.up,player2mouse,Vector3.forward);
                Quaternion quaternion = Quaternion.AngleAxis(angle,Vector3.forward);
                transform.rotation *= quaternion;
            }
        }else
        {
            Vector3 player2mouse = AimingTarget.position - transform.position;
            float angle = Vector3.SignedAngle(transform.up,player2mouse,Vector3.forward);
            Quaternion quaternion = Quaternion.AngleAxis(angle,Vector3.forward);
            transform.rotation *= quaternion;
        }
    }

    void attack()
    {
        weaponAnim.Play("attack");
    }

    IEnumerator ExcuteSound()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSecondsRealtime(0.25f);
            SoundManager.instance.PlayEnemyEffect("slash");
        }
    }
    #endregion

    #region 额..事件源?or公共函数
    public void GetDamaged(Object param)
    {
        if(isStageCleared) return;
        isDead = true;
        //生成尸体
        gameObject.SetActive(false);
        GameOver();
    }
    #region  事件源
    public void GameOver()
    {
        MessageCenter.SendMessage(new CommonMessage()
        {
            Mid = (int)MESSAGE_TYPE.GAME_OVER,
            content = null
        });
    }

    #endregion

    void OnGameRestart(CommonMessage msg)
    {
        if(msg.Mid != (int)MESSAGE_TYPE.GAME_RESTART) return;
        transform.position = oriPos;
        isDead = false;
        //重置子弹
        //重置分数
        gameObject.SetActive(true);
    }

    void OnMovNexStage(CommonMessage msg)
    {
        if(msg.Mid != (int)MESSAGE_TYPE.MOV_nSTAGE) return;
        oriPos = transform.position;
        isStageCleared = false;
    }

    public void OnStageClear(CommonMessage msg)
    {
        if(msg.Mid != (int)MESSAGE_TYPE.STAGE_CLEAR) return;
        isStageCleared = true;
    }
    #endregion
}
