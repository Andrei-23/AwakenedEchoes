using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpellManager : MonoBehaviour
{
    public const int spellCount = 3;
    public List<Spell> spells { get; private set; }

    private bool CheckSpellID(int id) => ((0 <= id) && (id < spellCount));

    private void OnEnable()
    {
        PlayerInputManager.OnSpellCasted += ActivateSpell;
    }

    private void OnDisable()
    {
        PlayerInputManager.OnSpellCasted -= ActivateSpell;
    }

    private void Awake()
    {
        spells = new List<Spell>();
        for (int i = 0; i < spellCount; i++)
        {
            spells.Add(null);
        }

        spells[0] = new Spell();
        spells[1] = new Spell();
        spells[2] = new Spell();
        //spells[0] = new Spell(new List<BaseAction>
        //{
        //    new ShootAction(0f, 0, true),
        //});
        //spells[1] = new Spell(new List<BaseAction>
        //{
        //    new ShootAction(-15f, 0, true),
        //    new ShootAction(15f, 0, true),
        //});
        //spells[2] = new Spell(new List<BaseAction>
        //{
        //    new ShootAction(0f, 0, true),
        //    new ShootAction(-30f, 0, true),
        //    new ShootAction(30f, 0, true),
        //});
    }
    public void SetSpell(Spell spell, int id = 0)
    {
        if (CheckSpellID(id) == false) return;
        spells[id] = spell;
    }

    public void ActivateSpell(int id)
    {
        if (CheckSpellID(id) == false) return;
        if (spells[id] == null) return;
        //if (spells[id].manaCost > PlayerStats.Instance.mana) return;

        LevelManager.Instance.LockAllPuzzles();
        CheckPuzzleCastPoints();
        
        if (PlayerStats.Instance.SpendMana(spells[id].manaCost))
        {
            spells[id].Activate();
        }
    }

    private void CheckPuzzleCastPoints()
    {
        var group = LevelManager.Instance.FindActiveCastPointGroup(PlayerCaster.Instance.GetPosition());
        if(group == null) return;

        PlayerStats.Instance.SetMana(PlayerStats.Instance.maxMana);
        PlayerCaster.Instance.transform.position = group.GetCastPoint().transform.position;
        group.OnPuzzleCast();
    }
}
