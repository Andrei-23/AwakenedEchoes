using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Shooter))]
public class Goblin : BaseEnemy
{
    private EnemyAttack _attack;
    protected override void Initialize()
    {
        base.Initialize();
        _attack = new EnemyAttack(gameObject, 0.6f, new List<BaseAction> {
            new ShootAction(Direction.Up, 0, true),
        });
    }
    //protected override void Awake()
    //{
    //    Initialize();
    //}
    protected override IEnumerator IdleBehaviour()
    {
        _enemyMovement.EnableMovement(false);
        float delay = Random.Range(_idleWaitDelayMin, _idleWaitDelayMax);
        yield return new WaitForSeconds(delay);
        yield return WalkRandomly();
    }
    protected override IEnumerator AttackBehaviour()
    {
        _enemyMovement.EnableMovement(true);
        if (CanSeePlayer())
        {
            SetPlayerTarget();
        }

        if (CanAttackPlayer())
        {
            CastAttack(_attack);
        }
        yield break;
    }
}
