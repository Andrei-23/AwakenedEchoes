using UnityEngine;
using System.Collections.Generic;

public class ConsoleFastCommands : MonoBehaviour
{
    [SerializeField] private List<PresetActionPanel> _commands;
    private List<bool> _saved = new List<bool>();

    private void Awake()
    {
        _saved = new List<bool>(new bool[_commands.Count]);
        OnCommandSave(0);
    }
    private void OnEnable()
    {
        SpellCompiler.OnCommandSave += OnCommandSave;
    }
    private void OnDisable()
    {
        SpellCompiler.OnCommandSave -= OnCommandSave;
    }

    private void OnCommandSave(int id)
    {
        _saved[id] = true;
        _commands[id].gameObject.SetActive(true);
    }
}
