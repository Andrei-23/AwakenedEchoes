using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Shooter))]
public class Spider : BaseEnemy
{
    private EnemyAttack _attack;
    protected override void Initialize()
    {
        base.Initialize();
        _attack = new EnemyAttack(gameObject, 0.6f, new List<BaseAction> {
            new ShootCircleAction(0, true, 6),
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

        if (CanAttackPlayer() && !_attack.onCooldown)
        {
            CastAttack(_attack);
            _enemyMovement.EnableMovement(false);
            yield return new WaitForSeconds(1);
            _enemyMovement.EnableMovement(true);
        }
        yield break;
    }
}
