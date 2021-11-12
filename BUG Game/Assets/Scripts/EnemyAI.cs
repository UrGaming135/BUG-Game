using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterManager))]
public class EnemyAI : MonoBehaviour
{
    public EnemyState state = EnemyState.Chasing;
    public EnemyType enemyType = EnemyType.Melee;

    [SerializeField]
    private float attackRange = 5f;
    [SerializeField]
    private float attackSpeed = 2f;
    [SerializeField]
    private float damage = 10f;

    private GameObject target;
    private CharacterManager targetManager;
    private NavMeshAgent agent;

    private Coroutine attackCoroutine;

    private float nextAttack;

    private void Awake()
    {
        target = GameManager.instance.player;
        targetManager = target.GetComponent<CharacterManager>();
        agent = GetComponent<NavMeshAgent>();
        nextAttack = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            agent.SetDestination(target.transform.position);

            var distanceToTarget = (target.transform.position - transform.position).magnitude;
            if (distanceToTarget <= attackRange)
            {
                state = EnemyState.Attacking;
                attackCoroutine = StartCoroutine(AttackCoroutine());
            } else
            {
                state = EnemyState.Chasing;
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private IEnumerator AttackCoroutine()
    {
        while(state == EnemyState.Attacking && target)
        {
            ManualRotationUpdate();
            if (nextAttack <= Time.time) {
                Attack();
                nextAttack = Time.time + (1 / attackSpeed);
            }
            yield return null;
        }
    }

    private void ManualRotationUpdate()
    {
        var direction = target.transform.position - transform.position;
        direction.y = 0;
        var rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, agent.angularSpeed);
    }

    private void Attack()
    { 
        print("This enemy doth attacketh y'unz");
        if (enemyType == EnemyType.Melee)
        {
            MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        if (targetManager)
        {
            targetManager.Damage(damage);
        }
    }
}
