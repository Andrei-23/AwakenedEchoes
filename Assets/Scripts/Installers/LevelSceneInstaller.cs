using UnityEngine;
using Zenject;

public class LevelSceneInstaller : MonoInstaller
{
    [SerializeField] private SpellCastCreator _spellCastCreator;
    public override void InstallBindings()
    {
    }
}