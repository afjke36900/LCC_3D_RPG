using UnityEngine;
using UnityEngine.UI;
using Invector.CharacterController;

public class PlayerController : MonoBehaviour
{
    public float hp = 100;

    private Animator ani, aniRoot;          // 模型動畫控制器，工具動畫控制器
    private vThirdPersonController tpc;     // 第三人稱控制器
    private Rigidbody rig;

    public Slider hpSlider;

    [Range(2.5f, 5f)]
    [Tooltip("攻擊範圍")]
    public float rangeAttack = 3f;
    [Range(0, 5)]
    [Tooltip("攻擊延遲判定")]
    public float delayAttack = 1.2f;

    public float attack = 20;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * rangeAttack); //forward : 前方Z , right : 右方X , up : 上方Y
    }

    private void DelayAttack()
    {
        RaycastHit hit; //射線碰撞資訊
        //物理.射線碰撞(起點.方向.射線碰撞資訊，長度)
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, rangeAttack))
        {
            print(hit.collider.gameObject);
            if (hit.collider.gameObject.tag == "敵人")
            {
                hit.collider.GetComponent<Enemy>().Hit(attack);
            }
        }
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
        aniRoot = transform.root.GetComponent<Animator>();          // 根物件 transform.root
        tpc = transform.root.GetComponent<vThirdPersonController>();
        rig = transform.root.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (ani.GetBool("死亡開關")) return;
        Move();
        Jump();
        Attack();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");     // 左右、AD
        float v = Input.GetAxisRaw("Vertical");       // 上下、WS

        //print("左右：" + h);
        //print("上下：" + v);

        ani.SetBool("走路開關", h != 0 || v != 0);
        ani.SetBool("跑步開關", Input.GetKey(KeyCode.LeftShift));
    }

    private void Jump()
    {
        ani.SetBool("跳躍開關", !aniRoot.GetBool("IsGrounded"));
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊")&& !ani.IsInTransition(0))
        {
            ani.SetTrigger("攻擊觸發");
            Invoke("DelayAttack", delayAttack);
        }

        // 取得動畫狀態資訊.動畫名稱是否為 ""
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("攻擊") || ani.GetCurrentAnimatorStateInfo(0).IsName("受傷"))
        {
            tpc.enabled = false;
            tpc.lockMovement = true;
            rig.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            tpc.enabled = true;
            tpc.lockMovement = false;
            rig.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void Hit(float damage)
    {
        hp -= damage;
        hpSlider.value = hp;
        ani.SetTrigger("受傷觸發");
        if (hp <= 0) Dead();
    }

    private void Dead()
    {
        ani.SetBool("死亡開關",true);
        tpc.enabled = false;
        tpc.lockMovement = true;
        rig.constraints = RigidbodyConstraints.FreezeAll;
    }
}
