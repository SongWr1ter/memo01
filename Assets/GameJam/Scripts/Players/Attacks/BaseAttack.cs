using UnityEngine;

public abstract class BaseAttack : ScriptableObject {
    
    public GameObject BulletPrefab;
    [Tooltip("50 = 0.2s射一发")]
    public float ShootSpeed;
    [Tooltip("true=连射 false=点射")]
    public bool isAutomatic;
    public int AmmoAmount;
    public int AmmoCost;
    public float ReloadingTime;
    public string shotClipName;
    public abstract void Attack(Vector3 shootTrans,Quaternion playerTrans);
}
