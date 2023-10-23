using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    public float health { get; private set; } = 100f;
    public float maxHealth { get; private set; } = 100f;

    private float _damage = 10f;
    private float _hitInterval = 2f;
    private bool _canDamage = true;

    private Player _player;
    private EnemyMovement _enemyMovement;

    private void Start()
    {
        _player = FindObjectOfType<Player>();

        if (_player == null)
        {
            Debug.LogError("Player not found");
        }

        _enemyMovement = GetComponentInChildren<EnemyMovement>();
        _enemyMovement.OnPlayerReached += OnPlayerReached;
    }

    private void Update()
    {
        if (health <= 0)
        {
            Death();
        }

        if (_player.health <= 0)
        {
            _enemyMovement.OnPlayerReached -= OnPlayerReached;
            _enemyMovement.isPlayerDead = true;
        }
    }

    private void OnPlayerReached()
    {
        if (_canDamage)
        {
            SetDamage();
        }
    }

    public void GetDamage(float damage)
    {
        health -= damage;
    }

    public void SetDamage()
    {
        _canDamage = false;
        StartCoroutine(StartSetDamage(_player));
    }

    private IEnumerator StartSetDamage(Player player)
    {
        yield return new WaitForSeconds(_hitInterval);
        player.GetDamage(_damage);
        _canDamage = true;
    }

    private void Death()
    {
        EnemysDeathList.Instance.enemysDeathCount++;
        Destroy(gameObject);
    }
}