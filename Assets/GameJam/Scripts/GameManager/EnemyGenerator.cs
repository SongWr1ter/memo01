using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    #region ȡ��
    public enum _AreaType { Square, Circle }
    [HideInInspector]
    public _AreaType type;
    [HideInInspector]
    public Color Color = Color.red;
    Vector3 p;
    #endregion

    #region ����������
    [Header("���ɱ���")]
    public List<GameObject> EnemyPrefabs;
    GameObject _enemyPrefab;
    [Header("����Ƶ��")]
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

    #region ���ȡ��
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
            //����Բ��˵����ת�Ͳ���תûɶ�������Է��ص�pos����Ӧ��rotation�����ĸı���
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

    #region ������

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
            //ģ����߳�
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

    #region ����ȡ������
    void OnDrawGizmos()
    {
        Gizmos.color = Color;
        //Gizmos.DrawWireSphere(p, .5f);
        if (type == _AreaType.Square)
        {
            //                                                                                                            ��Z��ת
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
            //                                                      ����      ȡ����localscale���뾶
            //UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, GetMaximumScale());
        }
    }
    #endregion
#endif

}
