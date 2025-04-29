using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class EnemyAttack
{
    public Spell spell;
    public float cooldown; // begins when spell casted
    public bool onCooldown = false;
    public EnemyAttack(GameObject castObject, float cooldown, List<BaseAction> spellActions)
    {
        spell = new Spell(spellActions, Spell.CastGroup.Enemy, castObject);
        this.cooldown = cooldown;
    }
    public void Activate()
    {
        if (onCooldown) return;
        spell.Activate();
        onCooldown = true;
    }
}
public enum EnemyState
{
    Idle = 0,
    Attack,
    Search,
    Escape,
    Dead,
}

[RequireComponent(typeof(EnemyMovement))]
public abstract class BaseEnemy : MonoBehaviour, IActionCaster
{
    public float maxHealth;
    [HideInInspector] public float health;


    [Header("Movement")]

    [SerializeField] protected float _idleSpeed;
    [SerializeField] protected float _defaultSpeed;
    protected Vector2 _pathTarget; // ai navigation target

    [Header("States")]
    protected EnemyState _state = EnemyState.Idle;

    [SerializeField] protected float _idleWaitDelayMin;
    [SerializeField] protected float _idleWaitDelayMax;

    [SerializeField] protected float _searchTime;

    [SerializeField] protected float _attackRange;
    [SerializeField] protected float _visionRange;
    [SerializeField] protected float _followRange;


    //[Inject] private Player _player; // correct instantiation required!
    protected PlayerCaster _player;
    protected EnemyMovement _enemyMovement;

    protected float _attackTimer;
    private EnemyDamageColor _enemyDamageColor;

    protected virtual void Initialize()
    {
        health = maxHealth;
        _player = PlayerCaster.Instance;
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyDamageColor = GetComponent<EnemyDamageColor>();
    }

    protected virtual void Awake()
    {
        Initialize();
    }
    private void Start()
    { 
        StartBehaviours();
    }

    public void SetEnemyState(EnemyState state)
    {
        if(state != _state)
        {
            _state = state;
            StopAllCoroutines();
            StartBehaviours();
        }
    }

    private void StartBehaviours()
    {
        StartCoroutine(Behaviour());
        StartCoroutine(PlayerVisionCoroutine());
        StartCoroutine(MovementCoroutine());
    }
    protected IEnumerator Behaviour()
    {
        while (true)
        {
            //Debug.Log("State: " + _state.ToString());
            switch (_state)
            {
                case EnemyState.Idle:
                    yield return IdleBehaviour();
                    break;
                case EnemyState.Attack:
                    yield return AttackBehaviour();
                    break;
                case EnemyState.Search:
                    yield return SearchBehaviour();
                    break;
                case EnemyState.Dead:
                    yield return DeadBehaviour();
                    break;
                default:
                    yield return null;
                    break;
            }
        }
    }

    protected IEnumerator PlayerVisionCoroutine()
    {
        while (true)
        {
            //Debug.Log(GetDistFromPlayer() + " -> " + CanSeePlayer().ToString());
            if (CanSeePlayer())
            {
                if(_state != EnemyState.Attack)
                {
                    SetEnemyState(EnemyState.Attack);
                }
            }
            else
            {
                if (_state == EnemyState.Attack)
                {
                    SetEnemyState(EnemyState.Search);
                }
            }
            yield return null;
        }
    }
    protected IEnumerator MovementCoroutine()
    {
        _enemyMovement.SetSpeed(GetStateSpeed());
        yield return null;
    }

    private float GetStateSpeed()
    {
        if(_state == EnemyState.Idle)
        {
            return _idleSpeed;
        }
        return _defaultSpeed;
    }
    protected void SetPathTarget(Vector2 destination)
    {
        _pathTarget = destination;
        _enemyMovement.SetTarget(destination);
    }
    protected void SetPlayerTarget()
    {
        SetPathTarget(_player.GetPosition());
    }
    protected void SelectRandomPathTarget()
    {
        // choose random point
    }

    protected virtual IEnumerator StayForSeconds(float delay)
    {
        _enemyMovement.EnableMovement(false);
        yield return new WaitForSeconds(delay);
        _enemyMovement.EnableMovement(true);
    }

    /// <summary>
    /// Waits until enemy reaches target
    /// </summary>
    /// <param name="stopDistance"></param>
    /// <returns></returns>
    protected virtual IEnumerator WalkToTarget(float stopDistance = 1f)
    {
        _enemyMovement.EnableMovement(true);
        while (GetDistFromPlayer() > stopDistance)
        {
            yield return null;
        }
    }
    protected virtual IEnumerator WalkRandomly()
    {
        SelectRandomPathTarget();
        yield return WalkToTarget();
    }
    /// <summary>
    /// Patrolling / doing nothind
    /// </summary>
    protected virtual IEnumerator IdleBehaviour()
    {
        yield return null;
        //yield return new WaitForSeconds(idleWaitDelay);
        //yield return WalkRandomly();
    }

    protected virtual void CastAttack(EnemyAttack attack)
    {
        if (attack.onCooldown) return;
        attack.Activate();
        StartCoroutine(WaitAttackCooldown(attack));
    }
    private IEnumerator WaitAttackCooldown(EnemyAttack attack)
    {
        yield return new WaitForSeconds(attack.cooldown);
        attack.onCooldown = false;
    }
    protected virtual bool CanSeePlayer()
    {
        if(_state == EnemyState.Idle)
        {
            return GetDistFromPlayer() <= _visionRange;
        }
        else
        {
            return GetDistFromPlayer() <= _followRange;
        }
    }
    protected float GetDistFromPlayer()
    {
        Vector2 from = GetPosition();
        Vector2 to = _player.GetPosition();
        return (to - from).magnitude;
    }
    protected virtual bool CanAttackPlayer()
    {
        return CanSeePlayer() && GetDistFromPlayer() <= _attackRange;
    }
    /// <summary>
    /// Sets path target to where player was last seen, returns true if player is visible
    /// </summary>
    protected void UpdatePlayerPathTarget()
    {
        bool visible = CanSeePlayer();
        if (visible)
        {
            _pathTarget = _player.GetPosition();
        }
        //return visible;
    }
    /// <summary>
    /// Attacking player
    /// </summary>
    protected virtual IEnumerator AttackBehaviour()
    {
        yield return null;
    }
    /// <summary>
    /// Trying to find player
    /// </summary>
    protected virtual IEnumerator SearchBehaviour()
    {
        yield return new WaitForSeconds(_searchTime);
        SetEnemyState(EnemyState.Idle);
    }

    protected virtual void Die()
    {
        SetEnemyState(EnemyState.Dead);
    }
    protected virtual IEnumerator DeadBehaviour()
    {
        Destroy(gameObject);
        yield break;
    }

    public virtual Vector2 GetPosition()
    {
        return transform.position;
    }
    public virtual Vector2 GetCastPosition()
    {
        return transform.position;
    }

    public virtual float GetCastAngle()
    {
        Vector2 from = GetCastPosition();
        Vector2 to = _player.GetPosition();
        float angle = DirectionHandler.Vector2ToAngle(to - from);
        return angle;
    }

    public EnemyState GetEnemyState()
    {
        return _state;
    }

    public void TakeDamage(float damage)
    {
        ShowDamage();
        health = Mathf.Max(0f, health - damage);
        if(health <= 0f)
        {
            Die();
        }
    }
    private void ShowDamage()
    {
        _enemyDamageColor.Hit();
    }
}
