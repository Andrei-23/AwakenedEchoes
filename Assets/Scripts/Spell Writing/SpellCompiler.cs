using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpellCompiler : MonoBehaviour
{
    [SerializeField] private Console _console;
    [SerializeField] private UIManager _UIManager;

    [Inject] private PlayerSpellManager _playerSpellManager;
    [Inject] private PlayerInputManager _playerInputManager;

    //private bool _isWrongFormat = false;
    //private List<Func<List<string>, BaseAction>> _actionFormatReaders = new List<Func<List<string>, BaseAction>>{};
    private List<BaseAction> _actionsFormat;

    public static event Action<int> OnCommandSave;
    private void Awake()
    {
        _actionsFormat = new List<BaseAction>
        {
            new ShootAction(Direction.Up, 0, false),
            new RollAction(),
            new DashAction(Direction.Up),
            new WaitAction(1f),
            new SelectAction(ActionTargetType.Caster),
            new ShootCircleAction(0, false),
        };
    }

    public void TrySaveSpell()
    {
        Spell spell = ReadCode();
        if (spell == null) return;
        _playerSpellManager.SetSpell(spell, _playerInputManager.GetSlotId());
        _UIManager.SetState(UIManager.State.Game);
    }
    public Spell ReadCode()
    {
        List<BaseAction> actions = new List<BaseAction>();
        for (int row = 0; row < _console.code.Count; row++)
        {
            string str = "";
            foreach(Console.Symbol sym in _console.code[row])
            {
                char c = Console.SymbolToChar(sym);
                if (c == ' ') continue;
                str += c;
            }

            if (str == "") continue;
            
            var action = ReadLine(str);
            if(action == null)
            {
                ShowError(row);
                return null;
            }
            actions.Add(action);
        }
        foreach(BaseAction act in actions)
        {
            SaveActionCommand(act);
        }
        Spell spell = new Spell(actions);
        return spell;
    }

    private void ShowError(int row)
    {

    }
    private BaseAction ReadLine(string input)
    {
        foreach(BaseAction action in _actionsFormat)
        {
            BaseAction res = action.TryFitInput(input);
            if(res != null)
            {
                return res;
            }
        }
        return null;
    }

    private void SaveActionCommand(BaseAction action)
    {
        for(int i = 0; i < _actionsFormat.Count; i++) { 
            if(action.GetType() == _actionsFormat[i].GetType())
            {
                OnCommandSave?.Invoke(i);
            }
        }
    }
}
