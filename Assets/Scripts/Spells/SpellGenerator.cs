using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpellGenerator : MonoBehaviour
{
    [Inject] private PlayerCaster _player;

    private void Awake()
    {
        //PlayerInputManager.OnSpellCasted += OnSpellCasted;
    }
    private void OnDestroy()
    {
        //PlayerInputManager.OnSpellCasted -= OnSpellCasted;
    }

    public void OnSpellCasted(int id)
    {
        ActivateSpell();
    }

    public void ActivateSpell()
    {
        GameObject playerObj = _player.gameObject;

        List<BaseAction> actions = new List<BaseAction>
        {

            //new ShootCircleAction(0, true, 18),
            //new ShootAction(Direction.Up, 0, true),
            //new WaitAction(0.5f),
            //new ShootCircleAction(0, false, 12),


            new ShootAction(0f, 0, true),
            new ShootAction(75f, 0, true),
            new ShootAction(-75f, 0, true),
            new RollAction(),
            new WaitAction(.4f),
            new SelectAction(ActionTargetType.Bullet, SpellCast.SelectMode.All),
            new ShootCircleAction(0, false),
            //new WaitAction(.4f),
            //new ShootAction(180f, 0, false),
            //new ShootAction(0f, 0, false),
            //new ShootAction(30f, 0, false),
            //new ShootAction(-30f, 0, false),
            //new WaitAction(.75f),
            //new SelectAction(ActionTargetType.Bullet, SpellCast.SelectMode.All),



            //new WaitAction(.7f),
            //new SelectAction(ActionTargetType.Bullet, SpellCast.SelectMode.All),
            //new ShootCircleAction(0, false),

            //new ShootAction(-15f, 0, false),

            //new ShootAction(60f, 0, true),
            //new ShootAction(75f, 0, true),
            //new ShootAction(90f, 0, true),
            //new ShootAction(105f, 0, true),
            //new ShootAction(120f, 0, true),

            //new DashAction(Direction.Down),

            //new SelectAction(ActionTargetType.Bullet, true),
            //new ShootAction(-90f, 0, false),

            //new ShootAction(0f, 0, true),
            //new ShootAction(-90f, 0, true),
            //new ShootAction(-180f, 0, true),

            //new WaitAction(0.5f),

            //new SelectAction(ActionTargetType.Bullet, true),
            //new ShootAction(-90f, 0, false),

            //new WaitAction(0.7f),
            //new ShootAction(45f, 0, false),
            //new ShootAction(135f, 0, false),

            //new SelectAction(ActionTargetType.Bullet, true),
            //new WaitAction(0.3f),
            //new ShootAction(90f, 0, false),

            //new SelectAction(ActionTargetType.Player, true),
            //new WaitAction(0.5f),
            //new ShootAction(540f, 0, false),


            //new RollAction(),
            //new DashAction(Direction.Up),
            //new DashAction(Direction.Down),
            //new DashAction(Direction.Down),
            //new ShootAction(Direction.UpLeft, 0),
            //new ShootAction(Direction.UpRight, 0),
            //new RollAction(),
        };

        Spell spell = new Spell(actions, Spell.CastGroup.Player, playerObj);
        Debug.Log("Spell cost: " + spell.manaCost.ToString());
        spell.Activate();

    }

    void Update()
    {
        
    }
}
