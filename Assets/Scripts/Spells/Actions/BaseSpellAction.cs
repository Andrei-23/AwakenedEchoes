using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpellAction : BaseAction
{

    protected Spell _spell;

    public BaseSpellAction()
    {
        _spell = new Spell();

        mana = _spell.manaCost;
        duration = 0f;
        reload = 0.2f;
        //SetSpell(new Spell());
    }

    //protected virtual void SetSpell(Spell spell)
    //{
    //    _spell = spell;

    //    mana = _spell.manaCost;
    //    duration = 0f;
    //    reload = 0.2f;
    //}

    public override void Activate(CastData castData)
    {
        //castData.caster.AddActionInList(this);
        castData.spellCaster.AddActionInList(_spell.actions);
    }

    public override void Finish(CastData actData) {
        
    }

    protected abstract override BaseAction ReadArgs(List<string> args);

}
