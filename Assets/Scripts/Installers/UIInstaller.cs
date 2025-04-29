using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private UIManager _UIManager;

    public override void InstallBindings()
    {
        Container.Bind<UIManager>().FromInstance(_UIManager).AsSingle();
    }
}