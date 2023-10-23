using System.Collections;
using UnityEngine;

public class EnemyVisual : MonoBehaviour
{
    private const string WALK_BOOL = "IsWalk";
    private const string RUN_BOOL = "IsRun";
    private const string HIT_TRIGGER = "IsHit";

    [SerializeField] private EnemyMovement _enemyMovement;
    [SerializeField] private EnemyDamage _enemyDamage;

    private Animator _animator;
    private float _hitInterval = 2.19f;
    private bool _isAnimatingHit = false;

    private void Start()
    {
        _animator = GetComponent <Animator>();
        _enemyMovement.OnPlayerReached += OnPlayerReached;
    }

    private void OnPlayerReached()
    {
        if (!_isAnimatingHit)
        {
            _animator.SetTrigger(HIT_TRIGGER);
            _isAnimatingHit = true;

            StartCoroutine(EnemysHit());
        }
    }

    private void Update()
    {
        _animator.SetBool(WALK_BOOL, _enemyMovement.IsWalk());
        _animator.SetBool(RUN_BOOL, _enemyMovement.IsAngre());
    }

    private IEnumerator EnemysHit()
    {
        yield return new WaitForSeconds(_hitInterval);
        _isAnimatingHit = false;
    }
}