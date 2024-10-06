using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Move : MonoBehaviour
{
    enum State
    {
        Normal,
        Roll
    }
    public float speed;
    public LayerMask floorMask;
    public float attackSpeed;
    [Tooltip("角色一直盯着看的物体")]
    [HideInInspector]
    public Transform AimingTarget;
    private Vector3 mov;

    private Transform weaponTrans;
    private Animator weaponAnim;
    private Animator moveAnim;

    private Rigidbody2D rigi;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer gunSpriteRenderer;

    private bool isDead;
    private Vector3 oriPos;
    [SerializeField]
    private Transform gun;//让枪的贴图朝向目标而非玩家的贴图

    #region Roll
    private Vector3 rollDir;
    private State state;
    [SerializeField] private float rollSpeed;
    private LayerMask oriLayer;
    float _rollspeed;
    private Vector3 lastMov;
    #endregion
    [Header("无限地图相关")]
    public SpriteRenderer sprite;
    [SerializeField]
    private float offsetSpeed;
    private Material _material;
    // Start is called before the first frame update
    void Start()
    {
        _material = sprite.materials[0];
        weaponTrans = transform.GetChild(0);
        weaponAnim = weaponTrans.GetComponent<Animator>();
        moveAnim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();
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
                moveAnim.SetBool("run",true);
                }
                else
                {
                    moveAnim.SetBool("run", false);
                }
            
            turn();
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
        float offsetY = _material.GetFloat("_Horizontal");
        _material.SetFloat("_Horizontal", rigi.velocity.y / offsetSpeed + offsetY);
        float offsetX = _material.GetFloat("_Vertical");
        _material.SetFloat("_Vertical", rigi.velocity.x / offsetSpeed + offsetX);
    }


    #region 动作,行动
    //角色y轴始终朝向鼠标 ---modified--> 让枪的X轴朝向目标而非玩家的&Flip
    void turn()
    {
        //transform.rotation = Quaternion.LookRotation(); 不能用因为LookAt是Z轴朝向目标方向
        if(AimingTarget == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit ,1000f,floorMask))
            {
                Vector3 gun2mouse = hit.point - gun.position;
                gun2mouse.z = 0;
                //计算角度
                float angle = Vector3.SignedAngle(gun.right, gun2mouse, Vector3.forward);
                Quaternion quaternion = Quaternion.AngleAxis(angle,Vector3.forward);
                gun.rotation *= quaternion;
                //计算Flip
                Vector3 player2mouse = hit.point - transform.position;
                float dot = Vector3.Dot(transform.right, player2mouse.normalized);
                if (dot > 0)
                {
                    spriteRenderer.flipX = false;
                    gunSpriteRenderer.flipY = false;
                    gun.localPosition = new Vector3(0.3f,gun.localPosition.y,0f);
                }
                else
                {
                    spriteRenderer.flipX = true;
                    gunSpriteRenderer.flipY = true;
                    gun.localPosition = new Vector3(-0.3f, gun.localPosition.y, 0f);
                }
            }
        }else
        {
            Vector3 gun2mouse = AimingTarget.position - gun.position;
            gun2mouse.z = 0;
            float angle = Vector3.SignedAngle(gun.right, gun2mouse, Vector3.forward);
            Quaternion quaternion = Quaternion.AngleAxis(angle,Vector3.forward);
            gun.rotation *= quaternion;
            //计算Flip
            Vector3 player2mouse = AimingTarget.position - transform.position;
            float dot = Vector3.Dot(transform.right, player2mouse.normalized);
            if (dot > 0)
            {
                spriteRenderer.flipX = false;
                gunSpriteRenderer.flipY = false;
                gun.localPosition = new Vector3(0.3f, gun.localPosition.y, 0f);
            }
            else
            {
                spriteRenderer.flipX = true;
                gunSpriteRenderer.flipY = true;
                gun.localPosition = new Vector3(-0.3f, gun.localPosition.y, 0f);
            }
        }
    }
    #endregion
    public void SetGunActive(int active)
    {
        if(active == 0)
            gun.gameObject.SetActive(false);
        else
            gun.gameObject.SetActive(true);
    }

}
