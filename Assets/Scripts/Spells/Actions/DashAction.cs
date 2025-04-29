using System;
using System.Collections.Generic;
using UnityEngine;

public class DashData
{
    public Vector2 direction;
    //public float speed;
    public float duration;

    public DashData(Vector2 direction, /*float speed,*/ float duration)
    {
        this.direction = direction;
        //this.speed = speed;
        this.duration = duration;
    }
}

public class DashAction : BaseAction
{
    //private const float _dashSpeed = 2f; 
    private const float _dashDuration = 0.2f;

    //private Vector2 _pos;
    private Direction _direction;
    public DashAction(Direction direction)
    {
        duration = 0.3f;
        reload = 1f;
        mana = 6f;
        format = "W1D1W";

        _direction = direction;
    }

    public static event Action<DashData> OnActivate;

    public override void Initiate(SpellCast caster) { }
    //public override void Final(SpellCast caster) { }

    public override void Activate(CastData actData)
    {
        //if(targetType != ActionTargetType.Player) { return; }

        Vector2 v = DirectionHandler.GetDirectionVector(_direction);
        OnActivate?.Invoke(new DashData(v, _dashDuration));
    }
    //public override void Finish(CastData actData) { }
    protected override BaseAction ReadArgs(List<string> args)
    {
        bool correct = false;
        Direction dir = SpellArguments.ArgToDirection(args[0], ref correct);
        if (!correct) return null;
        return new DashAction(dir);
    }
}