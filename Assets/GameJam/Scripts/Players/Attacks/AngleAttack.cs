using UnityEngine;

[CreateAssetMenu(fileName = "AngleAttack", menuName = "GameJam/AngleAttack", order = 0)]
public class AngleAttack : BaseAttack {
    public override void Attack(Vector3 _shootPos, Quaternion rotation)
    {
        GameObject bullet = ObjectPool.Instance.GetObject(BulletPrefab);
        bullet.transform.position = _shootPos;
        bullet.transform.rotation = Quaternion.identity * Quaternion.Euler(0,0,-90f) * rotation;
        sSoundManager.PlayAudio(shotClipName);
    }
}