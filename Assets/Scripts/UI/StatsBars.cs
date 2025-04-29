using UnityEngine;
using Zenject;

public class StatsBars : MonoBehaviour
{
    [SerializeField] private UIBar _healthBar;
    [SerializeField] private UIBar _manaBar;
    [Inject] private PlayerStats _playerStats;

    private void Update()
    {
        _healthBar.SetValue(_playerStats.hp, _playerStats.maxHp);
        _manaBar.SetValue(_playerStats.mana, _playerStats.maxMana);
    }
}
