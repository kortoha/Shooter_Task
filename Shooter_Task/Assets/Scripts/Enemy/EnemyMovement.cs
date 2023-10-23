using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private const string PLAYER_LAYER = "Player";

    [SerializeField] private Transform[] _destinationArray;

    [NonSerialized] public bool isPlayerDead = false;

    private NavMeshAgent _agent;
    private bool _isWalk = false;
    private bool _isAngre = false;
    private int _currentPatrolIndex = 0;
    private float _currentSpeed = 1f;
    private float _fightModSpeed = 3.6f;
    private float _attackRange = 1.0f;
    

    public event System.Action OnPlayerReached;

    private void Start()
    {
        _agent = transform.GetComponentInParent<NavMeshAgent>();

        StartPatrol();
    }

    private void Update()
    {
        Patrolling();

        if (_agent.speed == _currentSpeed)
        {
            _isWalk = true;
            _isAngre = false;
        }
        else if(_agent.speed == _fightModSpeed)
        {
            _isWalk = false;
            _isAngre = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer(PLAYER_LAYER))
        {
            MoveToPlayer(other.transform.position);
        }
    }

    private void MoveToPlayer(Vector3 playerPosition)
    {
        if (!isPlayerDead)
        {
            _agent.speed = _fightModSpeed;
            _agent.SetDestination(playerPosition);

            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
            if (distanceToPlayer < _attackRange)
            {
                OnPlayerReached?.Invoke();
                _agent.destination = transform.position;
                _agent.transform.LookAt(playerPosition);
            }
            else
            {
                Patrolling();
            }
        }
    }

    private void StartPatrol()
    {
        _agent.speed = _currentSpeed;
        if (_destinationArray.Length > 0)
        {
            Vector3 initialPatrolPoint = _destinationArray[_currentPatrolIndex].position;
            _agent.SetDestination(initialPatrolPoint);
        }
    }

    private void Patrolling()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.1f)
        {
            _agent.speed = _currentSpeed;
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _destinationArray.Length;
            Vector3 nextPatrolPoint = _destinationArray[_currentPatrolIndex].position;
            _agent.SetDestination(nextPatrolPoint);
        }
    }

    public bool IsWalk()
    {
        return _isWalk;
    }

    public bool IsAngre()
    {
        return _isAngre;
    }
}
