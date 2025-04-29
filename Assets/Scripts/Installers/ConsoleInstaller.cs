using UnityEngine;
using Zenject;

public class ConsoleInstaller : MonoInstaller
{

    [SerializeField] private ConsoleCellSprites _consoleCellSprites;
    [SerializeField] private ConsoleRender _consoleRender;

    public override void InstallBindings()
    {
        Container.Bind<ConsoleCellSprites>().FromInstance(_consoleCellSprites).AsSingle();
        Container.Bind<ConsoleRender>().FromInstance(_consoleRender).AsSingle();
    }
}