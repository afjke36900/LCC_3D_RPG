using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("基本欄位")]
    public float attack = 20;
    public float hp = 250;
    [Range(0f,100f)]
    public float speed = 1.5f;
    [Range(0f, 100f)][Tooltip("追蹤距離")]
    public float distanceTrank = 25f;
    [Range(0f, 100f)][Tooltip("攻擊距離")]
    public float distanceAttack = 5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, distanceAttack);
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawSphere(transform.position, distanceTrank);
    }
}
