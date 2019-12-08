using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("基本欄位")]
    public float attack = 20;
    public float hp = 250;
    [Range(0f,100f)][Tooltip("移動送度")]
    public float speed = 1.5f;
    [Range(0f, 100f)][Tooltip("追蹤距離")]
    public float distanceTrank = 25f;
    [Range(0f, 100f)][Tooltip("攻擊距離")]
    public float distanceAttack = 5f;
    [Range(2.5f, 5f)][Tooltip("每次攻擊冷卻時間")]
    public float cd = 3.5f;
    [Range(2.5f, 5f)][Tooltip("攻擊範圍")]
    public float rangeAttack = 3f;
    [Range(0, 5)][Tooltip("攻擊延遲判定")]
    public float delayAttack = 1.2f;
    
    private float timer;

    private Transform target;    //目標物件
    private Animator ani;        //動畫元件
    private NavMeshAgent agent;  //導覽代理器元件

    private void Start()
    {
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("玩家").transform;
        agent.speed = speed;
    }

    private void Update()
    {
        Tack();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, distanceAttack);

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, distanceTrank);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * rangeAttack); //forward : 前方Z , right : 右方X , up : 上方Y
    }

    private void Tack()
    {
        float dis = Vector3.Distance(target.position, transform.position);  //代理器 目的地 設定三圍項量

        if (dis <= distanceAttack)
        {
            Attack();
        }

        if (dis <= distanceTrank)
        {
            agent.isStopped = false;
            ani.SetBool("走路開關", !agent.isStopped);
            agent.SetDestination(target.position);
        }
        else
        {
            Idle();
        }
    }
    private void Idle()
    {
        agent.isStopped = true;
        ani.SetBool("走路開關", !agent.isStopped);
    }
    private void Attack()
    {
        if (timer >= cd)                                     //如果計時器 >= 冷卻時間
        {
            timer = 0;                                       //歸零重新計算時間
            agent.isStopped = true;                          //停止代理器避免滑行
            ani.SetTrigger("攻擊觸發");                      //攻擊動畫
            Invoke("DelayAttack", delayAttack);              //延遲調用("方法名稱",延遲時間)
        }
        else
        {
            timer += Time.deltaTime;                         //否則計時器 < 冷卻時間累加
            Idle();                                          //等待
        }
    }

    private void DelayAttack()
    {
        RaycastHit hit; //射線碰撞資訊
        //物理.射線碰撞(起點.方向.射線碰撞資訊，長度)
        if(Physics.Raycast(transform.position + Vector3.up, transform.forward,out hit, rangeAttack))
        {
            print(hit.collider.gameObject);
        }
    }
    private void Hit()
    {

    }
    private void Dead()
    {

    }
}
