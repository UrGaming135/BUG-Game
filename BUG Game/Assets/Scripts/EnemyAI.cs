using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(CharacterManager))]
public class EnemyAI : MonoBehaviour
{
    public EnemyState state = EnemyState.Chasing;
    public EnemyType enemyType = EnemyType.Melee;
    public float attackRange = 5f;

    [SerializeField]
    private float attackSpeed = 2f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float stoppingDistance = 5f;
    [SerializeField]
    private Transform eyeLineTransform;
    [SerializeField]
    private LayerMask enemyLayer;

    private GameObject target;
    private CharacterManager targetManager;
    private NavMeshAgent agent;
    private CoverPointNavigation coverNavigation;
    private EnemyAttack rangedAttack;

    private Coroutine attackCoroutine;

    private float nextAttack;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        coverNavigation = GetComponent<CoverPointNavigation>();
        rangedAttack = GetComponent<EnemyAttack>();
        nextAttack = Time.time;
    }

    private void Start()
    {
        target = GameManager.instance.player;
        targetManager = target.GetComponent<CharacterManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            // Get navigation target.
            Transform navTarget;
            if (coverNavigation.closestCoverPoint)
            {
                navTarget = coverNavigation.closestCoverPoint;
                agent.stoppingDistance = 0;
            } else
            {
                navTarget = target.transform;
                agent.stoppingDistance = stoppingDistance;
            }

            agent.SetDestination(navTarget.position);

            var distanceToTarget = (target.transform.position - transform.position).magnitude;
            var targetCenter = target.transform.position + new Vector3(0, 1f, 0);
            var targetTop = target.transform.position + new Vector3(0, 1.9f, 0);
            //Debug.DrawLine(eyeLineTransform.position, targetCenter, Color.red);
            //Debug.DrawLine(eyeLineTransform.position, targetTop, Color.black);
            var hasLineOfSight = !Physics.Linecast(eyeLineTransform.position, targetCenter, enemyLayer) || !Physics.Linecast(eyeLineTransform.position, targetTop, enemyLayer);
            if (distanceToTarget <= attackRange && hasLineOfSight)
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
        //print("This enemy doth attacketh y'unz");
        if (enemyType == EnemyType.Melee)
        {
            MeleeAttack();
        }
        else
        {
            rangedAttack.ShootGun(target.transform);
        }
    }

    private void MeleeAttack()
    {
        if (targetManager)
        {
            print("Melee attack");
            targetManager.Damage(damage);
        }
    }
}
