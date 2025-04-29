using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum ActionTargetType
{
    Caster = 0, // whoever started the spell
    Bullet,
    Ground,
    Playerr,
    Enemy,
}
/*
    Action might change depending on cast object:
    - freeze on bullet changes hit effect
    - freeze on player/ground hits nearby objects
    - freeze on enemy applies effect directly
*/

public class CastData
{
    public ActionTargetType targetType;
    public GameObject target;
    public SpellCast spellCaster;
    public IActionCaster actionCaster;

    public CastData(ActionTargetType targetType, GameObject target, SpellCast spellCaster)
    {
        this.targetType = targetType;
        this.target = target;
        this.spellCaster = spellCaster;
        actionCaster = spellCaster.GetCastObject().GetComponent<IActionCaster>();
        if(actionCaster == null)
        {
            Debug.LogError("IActionCaster component not found.");
        }
    }
}

public abstract class BaseAction
{
    public float duration;
    public float reload;
    public float mana;

    public string format;
    //public int argCount;

    public bool castOnce = false; // if true, cast on first target only, i.e. WaitAction

    protected Dictionary<ActionTargetType, bool> _possibleTargets;
    protected bool _isWrongFormat = false;

    protected BaseAction()
    {
        duration = 0f;
        reload = 0f;
        mana = 0f;

        _possibleTargets = new Dictionary<ActionTargetType, bool>();
        FillPossibleTargets(true);
    }

    public virtual void Initiate(SpellCast caster) { }

    public abstract void Activate(CastData actData);

    public virtual void Finish(CastData actData) { }

    public virtual void Final(SpellCast caster) { }

    protected abstract class FormatData
    {

    }

    /// <summary>
    /// Checks if input is formatted correctly, then generates action with given params.
    /// </summary>
    /// <returns>BaseAction instance (or null if wrong format)</returns>
    public BaseAction TryFitInput(string input)
    {
        _isWrongFormat = false;
        if (CheckInputFormat(input) == false) return null;
        List<string> args = SpellArguments.ReadInput(format, input);
        if (_isWrongFormat) return null;
        return ReadArgs(args);
    }

    protected abstract BaseAction ReadArgs(List<string> args);

    private bool CheckInputFormat(string input)
    {
        if (input.Length != format.Length) return false;
        for (int i = 0; i < input.Length; i++)
        {
            if (char.IsDigit(format[i]) == false && format[i] != input[i])
            {
                return false;
            }
        }
        return true;
    }

    protected void FillPossibleTargets(bool value)
    {
        foreach (ActionTargetType targetType in Enum.GetValues(typeof(ActionTargetType)))
        {
            _possibleTargets[targetType] = value;
        }
    }

    public bool IsCorrectTarget(CastData actData)
    {
        return _possibleTargets[actData.targetType];
    }
}
