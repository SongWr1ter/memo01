using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finger : MonoBehaviour
{
    [SerializeField]
    private float radius;
    public GameObject Finger;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = GlobalResManager.Instance.Player.transform.position - transform.position;
        Finger.transform.position = transform.position + targetDir.normalized * radius;
        Vector3 fingerRight = Finger.transform.right;
        float angle = Vector3.SignedAngle(fingerRight,targetDir.normalized,Vector3.forward);
        Quaternion quaternion = Quaternion.AngleAxis(angle,Vector3.forward);
        Finger.transform.rotation *= quaternion;
    }
}
