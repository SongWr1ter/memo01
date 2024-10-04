using UnityEngine;

[CreateAssetMenu(fileName = "BulletInfo", menuName = "GameJam/BulletInfo", order = 0)]
public class BulletInfo : ScriptableObject {
    public int baseDamage;
    public float speed;
    public float deadTime;
}
