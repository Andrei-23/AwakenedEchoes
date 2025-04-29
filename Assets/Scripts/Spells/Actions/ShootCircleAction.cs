using System.Collections.Generic;
using UnityEngine;

public class ShootCircleAction : BaseSpellAction
{
    private int _type;
    public bool _shootFromPlayer; // if true, cast from player
    private int _bulletCount;

    public ShootCircleAction(int type, bool shootFromPlayer, int bulletCount = 12)
    {
        format = "SSD11WDW";
        _type = type;
        _shootFromPlayer = shootFromPlayer;
        _bulletCount = Mathf.Max(bulletCount, 1);

        var actions = new List<BaseAction>();
        for (int i = 0; i < _bulletCount; i++)
        {
            float angle = 360f / _bulletCount * (shootFromPlayer ? i : 1);
            actions.Add(new ShootAction(angle, type, shootFromPlayer));
        }
        actions.Add(new SelectAction(ActionTargetType.Bullet, SpellCast.SelectMode.Counted, bulletCount));
        _spell = new Spell(actions);

        mana = _spell.manaCost * 0.5f;
        duration = 0f;
        reload = 1f;
    }

    //protected override void SetSpell(Spell spell)
    //{
    //    base.SetSpell(spell);
    //}

    public override void Finish(CastData actData)
    {

    }

    protected override BaseAction ReadArgs(List<string> args)
    {
        var nums = SpellArguments.ArgToIntList(args[0]);
        int x = 3 + nums[0] * 4 + nums[1];
        return new ShootCircleAction(0, false, x);
    }
}
