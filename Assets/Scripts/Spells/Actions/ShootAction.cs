using System;
using System.Collections.Generic;
using UnityEngine;
//public class ShootData
//{
//    public Vector2 direction;
//    public int type;
//    public TeamType team;

//    public enum TeamType
//    {
//        Player = 0,
//        Enemy,
//        Neutral,
//    }
//    public ShootData(Vector2 direction, int type, TeamType team)
//    {
//        this.direction = direction;
//        this.type = type;
//        this.team = team;
//    }
//}
public class ShootAction : BaseAction
{
    
    private float _angle;
    private int _type;
    public bool _shootFromCaster; // if true, cast from player

    public ShootAction(float angle, int type, bool shootFromCaster)
    {
        duration = 0f;
        reload = 0.2f;
        mana = 4f;
        format = "AD11";

        _angle = angle;
        _type = type;
        _shootFromCaster = shootFromCaster;
    }
    public ShootAction(Direction direction, int type, bool shootFromCaster)
        : this(DirectionHandler.GetDirectionAngle(direction), type, shootFromCaster) { }

    public override void Initiate(SpellCast caster) 
    {
        if (_shootFromCaster)
        {
            caster.SelectTarget(ActionTargetType.Caster);
        }
        caster.lastBullets = new List<GameObject>();
    }
    public override void Activate(CastData actData)
    {
        Shooter shooter = actData.target.GetComponent<Shooter>();
        if (shooter == null) {
            Debug.LogWarning("\"" + actData.target.name + "\" has no Shooter attribute.");
            return;
        }

        int teamType = (actData.actionCaster is PlayerCaster) ? 0 : 1;
        shooter.Shoot(_angle, _type, teamType);

        actData.spellCaster.lastBullets.Add(BulletManager.Instance.lastBullet);
        actData.spellCaster.allBullets.Add(BulletManager.Instance.lastBullet);
    }

    public override void Final(SpellCast caster) {
        caster.SelectTarget(ActionTargetType.Bullet, SpellCast.SelectMode.Previous);
    }
    protected override BaseAction ReadArgs(List<string> args)
    {
        bool correct = false;
        float angle = SpellArguments.ArgToAngle(args[0], ref correct);
        if (!correct) return null;
        return new ShootAction(angle, 0, false);
    }
}
