using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerCaster _player;
    [SerializeField] private PlayerInputManager _playerInputManager;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerSpellManager _playerSpellManager;

    public override void InstallBindings()
    {
        Container.Bind<PlayerCaster>().FromInstance(_player).AsSingle();
        Container.Bind<PlayerInputManager>().FromInstance(_playerInputManager).AsSingle();
        Container.Bind<PlayerMovement>().FromInstance(_playerMovement).AsSingle();
        Container.Bind<PlayerStats>().FromInstance(_playerStats).AsSingle();
        Container.Bind<PlayerSpellManager>().FromInstance(_playerSpellManager).AsSingle();
    }
}