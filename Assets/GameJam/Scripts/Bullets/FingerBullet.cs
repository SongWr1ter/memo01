using UnityEngine;
public class FingerBullet : BaseBullet
{
    protected override void Update()
    {
       transform.Translate(speed * transform.right
        * Time.deltaTime,Space.World);


        deadtime += Time.deltaTime;
        if(deadtime >= deadTime) {
            ObjectPool.Instance.PushObject(gameObject);
            deadtime -= deadTime;
        }
    }
}
