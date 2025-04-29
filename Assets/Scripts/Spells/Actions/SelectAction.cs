using System.Collections.Generic;
using UnityEngine;

public class SelectAction : BaseAction
{

    public ActionTargetType targetType;
    public SpellCast.SelectMode mode; // 0 = select prev targets, 1 = select all, 2 = select cnt
    public int selectCount; // if in counted mode, select last cnt created objects
    //public bool selectAll; 
    public SelectAction(
        ActionTargetType targetType,
        SpellCast.SelectMode mode = SpellCast.SelectMode.Previous,
        int selectCount = 1)
    {
        format = "WSWS";
        duration = 0f;
        reload = 0f;
        mana = 5f;

        this.targetType = targetType;
        this.mode = mode;
        this.selectCount = selectCount;
    }
    public override void Activate(CastData actData) {
        actData.spellCaster.SelectTarget(targetType, mode, selectCount);
    }

    protected override BaseAction ReadArgs(List<string> args)
    {
        return new SelectAction(ActionTargetType.Bullet, SpellCast.SelectMode.All);
    }
}
