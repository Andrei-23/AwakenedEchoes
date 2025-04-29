using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int hp { get; private set; }
    public int maxHp;
    public float mana { get; private set; }
    public float maxMana;
    public float manaRegenSpeed;

    public int codeLines;

    private Vector2 _respawnPoint;

    private void Awake()
    {
        Instance = this;
        hp = maxHp;
        mana = maxMana;
    }
    private void Start()
    {
        SetRespawnPoint(PlayerCaster.Instance.GetPosition());
        LevelManager.Instance.WorldReset();
    }
    private void Update()
    {
        SetMana(mana + manaRegenSpeed * Time.deltaTime);
    }
    public void SetRespawnPoint(Vector2 pos)
    {
        _respawnPoint = pos;
    }

    public void KillPlayer()
    {
        Die();
    }
    private void Die()
    {
        SetHP(maxHp);
        SetMana(maxMana);
        PlayerCaster.Instance.transform.position = _respawnPoint;
        LevelManager.Instance.WorldReset();
    }
    public void TakeDamage(int damage)
    {
        if(damage > 1)
        {
            // crit
        }
        SetHP(Mathf.Max(0, hp - damage));
        if(hp <= 0)
        {
            Die();
        }
    }
    public void SetHP(int value)
    {
        hp = value;
        // event
    }
    public void SetMana(float value)
    {
        value = Mathf.Clamp(value, 0, maxMana);
        mana = value;
        // event
    }
    public bool SpendMana(float value)
    {
        if (mana < value) return false;
        SetMana(mana - value);
        return true;
    }
}
