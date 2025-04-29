using System;
using System.Collections.Generic;
using UnityEngine;

public class RollAction : BaseAction
{

    public RollAction()
    {
        format = "WDSA";
        duration = 0.5f;
        reload = 1f;
        mana = 4f;
    }

    public static event Action OnActivate;

    public override void Initiate(SpellCast caster) { }
    //public override void Final(SpellCast caster) { }
    public override void Activate(CastData actData)
    {
        //if (targetType != ActionTargetType.Player) { return; }

        OnActivate?.Invoke();
    }
    public override void Finish(CastData actData) { }

    protected override BaseAction ReadArgs(List<string> args)
    {
        return new RollAction();
    }
}
