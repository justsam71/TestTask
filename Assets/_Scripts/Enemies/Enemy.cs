using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private enum State { Idle, Run, Attack, Wander }
    private State _currentState;

    [Header("References")]
    [SerializeField] private Transform enemyBody;
    [SerializeField] private Animator animator;

    [Header("Stats")]
    [SerializeField] private float damage = 5f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float attackRange = 1.2f;

    [Header("Wander")]
    private Vector3 wanderPoint;
    [SerializeField] private float wanderRadius = 3f;
    [SerializeField] private float wanderSpeed = 1.5f;

    private NavMeshAgent _agent;
    private Transform _target;
    private bool _canAttack = true;

    private static readonly int VelocityHash = Animator.StringToHash("velocity");
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Start()
    {
        _target = GameObject.FindWithTag("Player")?.transform;
        ChangeState(State.Idle);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<DiedEvent>(OnPlayerDie);
    }

    private void Update()
    {
        if (_target == null) return;

        float distance = Vector2.Distance(transform.position, _target.position);

        switch (_currentState)
        {
            case State.Idle:
                animator.SetFloat(VelocityHash, 0);
                if (distance <= chaseRange)
                    ChangeState(State.Run);
                break;

            case State.Run:
                animator.SetFloat(VelocityHash, 1);
                _agent.isStopped = false;
                _agent.SetDestination(_target.position);

                if (distance > chaseRange)
                    ChangeState(State.Idle);
                else if (distance <= attackRange && _canAttack)
                    ChangeState(State.Attack);
                else
                {
                    float horizontalDir = _target.position.x - transform.position.x;
                    enemyBody.FlipTowards(horizontalDir);
                }
                break;

            case State.Attack:
                _agent.isStopped = true;
                break;
        }

    }

    private void ChangeState(State newState)
    {
        _currentState = newState;

        switch (newState)
        {
            case State.Idle:
                _agent.isStopped = true;
                break;

            case State.Run:
                _agent.isStopped = false;
                break;

            case State.Attack:
                _agent.isStopped = true;
                animator.SetTrigger(AttackHash);
                StartCoroutine(PerformAttack());
                break;
            case State.Wander:
                StartCoroutine(WanderRoutine());
                break;

        }
    }

    private IEnumerator PerformAttack()
    {
        _canAttack = false;

        AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
        float attackDuration = animState.length > 0 ? animState.length : 0.5f;

        yield return new WaitForSeconds(attackDuration);

        if (_target != null && Vector2.Distance(transform.position, _target.position) <= attackRange)
        {
            Health playerHealth = _target.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        if (_target == null)
            yield break;

        float distance = Vector2.Distance(transform.position, _target.position);
        if (distance > stopDistance && distance <= chaseRange)
            ChangeState(State.Run);
        else
            ChangeState(State.Idle);

        _canAttack = true;
    }

    private IEnumerator WanderRoutine()
    {
        while (_currentState == State.Wander)
        {
            if (Vector2.Distance(transform.position, wanderPoint) < 0.2f)
            {
                wanderPoint = transform.position +
                    (Vector3)Random.insideUnitCircle * wanderRadius;
            }

            Vector2 dir = (wanderPoint - transform.position).normalized;

            if (dir.x != 0)
                transform.FlipTowards(dir.x);

            transform.position = Vector2.MoveTowards(
                transform.position,
                wanderPoint,
                wanderSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    void OnPlayerDie(DiedEvent e)
    {
        if (e.owner.CompareTag("Player"))
        {
            _target = null;
            ChangeState(State.Wander);
        }
    }
}