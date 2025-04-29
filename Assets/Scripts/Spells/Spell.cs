using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static Spell;

/// <summary>
/// Castable list of actions
/// </summary>
public class Spell
{
    public List<BaseAction> actions { get; private set; }
    public float manaCost { get; private set; }

    private CastGroup _castGroup;
    private GameObject _castObject;

    public enum CastGroup
    {
        Player = 1, // No player dmg
        Enemy, // No enemy dmg
        Neutral, // Damages everyone
    }

    //[Inject]
    //private Player _player;


    //public Spell(List<BaseAction> actions) {
    //    this.actions = actions;
    //    _castGroup = CastGroup.Player;
    //    manaCost = GetManaCost();

    //    if(_player == null)
    //    {
    //        Debug.LogError("No player? O_o");
    //        _castObject = null;
    //    }
    //    else
    //    {
    //        _castObject = _player.gameObject;
    //    }

    //}

    public Spell() : this(new List<BaseAction>()) { }
    public Spell(List<BaseAction> actions) : this(actions, CastGroup.Player, PlayerCaster.Instance.gameObject) { }
    public Spell(List<BaseAction> actions, CastGroup castGroup, GameObject castObject)
    {
        this.actions = actions;
        _castGroup = castGroup;
        _castObject = castObject;
        manaCost = GetManaCost();
    }

    private float GetManaCost()
    {
        float cost = 0;
        foreach (BaseAction action in actions)
        {
            cost += action.mana;
        }
        return cost;
    }
    public void Activate()
    {
        SpellCast spellCast = SpellCastCreator.Instance.CreateSpellCast();
        spellCast.SetCastObject(_castObject);
        spellCast.Activate(this);
    }

}
