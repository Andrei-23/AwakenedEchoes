using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shoots bullets from an object, add rotation offset from caster object
/// </summary>
[RequireComponent(typeof(IActionCaster))]
public class Shooter : MonoBehaviour
{
    private IActionCaster _actionCaster;

    private List<float> _angleSpread = new List<float> { 3f };
    private void Awake()
    {
        _actionCaster = GetComponent<IActionCaster>();
    }
    public void Shoot(float angle, int type, int teamType)
    {
        Vector2 v = DirectionHandler.AngleToVector2(angle);
        Shoot(v, type, teamType);
    }
    public void Shoot(Vector2 velocity, int type, int teamType)
    {
        float spread = _angleSpread[type];
        float newAngle = _actionCaster.GetCastAngle() + DirectionHandler.Vector2ToAngle(velocity);
        newAngle += Random.Range(-spread, spread);
        Vector2 v1 = DirectionHandler.AngleToVector2(newAngle);

        Quaternion offsetRotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
        GameObject go = Instantiate(
            BulletManager.Instance.GetType(type),
            transform.position,
            offsetRotation,
            BulletManager.Instance.bulletFolder.transform);

        Bullet bullet = go.GetComponent<Bullet>();
        if (bullet == null) return;

        bullet.SetVelocity(v1);
        bullet.SetTeam(teamType);

        BulletManager.Instance.UpdateLastBullet(go);
    }
}
