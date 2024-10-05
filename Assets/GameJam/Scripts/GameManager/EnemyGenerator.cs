using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    #region 取点
    public enum _AreaType { Square, Circle }
    [HideInInspector]
    public _AreaType type;
    [HideInInspector]
    public Color Color = Color.red;
    Vector3 p;
    #endregion

    #region 生成器变量
    [Header("生成变量")]
    public List<GameObject> EnemyPrefabs;
    GameObject _enemyPrefab;
    [Header("生成频率")]
    [Range(1f,10f)]
    public float MAX_GENERATE_INVERNAL;
    float gtime = 1f;
    public int threadNum;
    private float[] gtimes;
    public bool canGenerate = true;
    #endregion

    private void Start() {
        gtimes = new float[threadNum];
    }

    #region 随机取点
    public Quaternion GetRotation()
    {
        return Quaternion.Euler(0,0, transform.rotation.eulerAngles.z);
    }

    public Vector3 GetPosition()
    {
        if (type == _AreaType.Square)
        {
            float x = Random.Range(-transform.localScale.x, transform.localScale.x);
            float y = Random.Range(-transform.localScale.y, transform.localScale.y);
            Vector3 v = GlobalResManager.Instance.Player.transform.position + transform.rotation * new Vector3(x, y, 0);
            return v;
        }
        else if (type == _AreaType.Circle)
        {
            Vector3 dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f),0).normalized;
            //对于圆来说，旋转和不旋转没啥区别，所以返回的pos不用应用rotation对它的改变了
            return GlobalResManager.Instance.Player.transform.position + dir * Random.Range(0, GetMaximumScale());
        }

        return transform.position;
    }

    private float GetMaximumScale()
    {
        float scale = Mathf.Max(transform.localScale.x, transform.localScale.y);
        return Mathf.Max(scale, transform.localScale.z);
    }
    #endregion

    #region 生成器

    void GenerateEnemy(Vector3 pos)
    {
        //_enemyPrefab = Instantiate(EnemyPrefab1, pos, Quaternion.identity);
        int index = Random.Range(0,EnemyPrefabs.Count);
        _enemyPrefab = ObjectPool.Instance.GetObject(EnemyPrefabs[index]);
        _enemyPrefab.transform.position = pos;
        _enemyPrefab.transform.rotation = Quaternion.identity;
    }


    #endregion
    private void Update()
    {
        if(canGenerate)
        {
            if (gtime >= 0)
            {
                gtime -= Time.deltaTime;
            }
            else
            {

                GenerateEnemy(GetPosition());
                gtime = Random.Range(MAX_GENERATE_INVERNAL, MAX_GENERATE_INVERNAL+2f);
            }
            //模拟多线程
            for (int i = 0; i < gtimes.Length; i++)
            {
                if(gtimes[i] >= 0)
                {
                    gtimes[i] -= Time.deltaTime;
                }
                else
                {
                    GenerateEnemy(GetPosition());
                    gtimes[i] = Random.Range(1.0f, 1.0f+2f);
                }
            }
        }


    }

#if UNITY_EDITOR

    #region 绘制取点区域
    void OnDrawGizmos()
    {
        Gizmos.color = Color;
        //Gizmos.DrawWireSphere(p, .5f);
        if (type == _AreaType.Square)
        {
            //                                                                                                            绕Z轴转
            Vector3 p1 = transform.position + transform.rotation * new Vector3(transform.localScale.x, transform.localScale.y, 0);
            Vector3 p2 = transform.position + transform.rotation * new Vector3(transform.localScale.x, -transform.localScale.y, 0);
            Vector3 p3 = transform.position + transform.rotation * new Vector3(-transform.localScale.x, transform.localScale.y, 0);
            Vector3 p4 = transform.position + transform.rotation * new Vector3(-transform.localScale.x, -transform.localScale.y, 0);


            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p1, p3);
            Gizmos.DrawLine(p2, p4);
            Gizmos.DrawLine(p3, p4);

        }
        else if (type == _AreaType.Circle)
        {
            //UnityEditor.Handles.color = Color;
            //                                                      法线      取最大的localscale做半径
            //UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, GetMaximumScale());
        }
    }
    #endregion
#endif

}
