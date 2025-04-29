using System.Collections.Generic;
using UnityEngine;

public class WaitAction : BaseAction
{

    public WaitAction(float duration)
    {
        this.duration = duration;
        reload = 0f;
        mana = 1f;
        format = "AS11D";

        castOnce = true;
    }
    public override void Initiate(SpellCast caster) { }
    public override void Activate(CastData actData) { }
    public override void Finish(CastData actData) { }
    protected override BaseAction ReadArgs(List<string> args) {
        var nums = SpellArguments.ArgToIntList(args[0]);
        int x = nums[0] * 4 + nums[1];
        //if (x == 0) return null;
        return new WaitAction(0.1f * x + 0.5f);
    }

}
