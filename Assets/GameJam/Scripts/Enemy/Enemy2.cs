using UnityEngine;

public class Enemy2 : BaseEnemy
{
    protected override void Shoot(Vector3 targetDir)
    {
        attackTime -= paramater.attackInterval;
        targetDir.z = 0;
        GameObject _bullet = ObjectPool.Instance.GetObject(paramater.BulletPrefab);
        _bullet.transform.position = transform.position;
        _bullet.transform.rotation = transform.GetChild(0).rotation;
    }
}
