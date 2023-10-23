using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemies;
    public float _activationInterval = 15f; 
    private EnemysDeathList _deathList;

    private int _enemyIndex = 0; 
    private int _enemiesActivated = 0; 

    private void Start()
    {
        _deathList = EnemysDeathList.Instance;

        for (int i = 0; i < 3; i++)
        {
            ActivateEnemy();
        }

        _enemiesActivated = 3;
    }

    private void ActivateEnemy()
    {
        if (_enemyIndex < _enemies.Length)
        {
            _enemies[_enemyIndex].SetActive(true);
            _enemyIndex++;
        }
    }

    private void Update()
    {
        int enemiesKilled = _deathList.enemysDeathCount;

        if (enemiesKilled == 3 && _enemiesActivated == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                ActivateEnemy();
            }

            _enemiesActivated += 3;
        }
        else if (enemiesKilled == 6 && _enemiesActivated == 6)
        {
            for (int i = 0; i < 4; i++)
            {
                ActivateEnemy();
            }

            _enemiesActivated += 4;
        }
        else if (enemiesKilled == 10 && _enemiesActivated == 10)
        {
            for (int i = 0; i < 5; i++)
            {
                ActivateEnemy();
            }

            _enemiesActivated += 5;

            StartCoroutine(ActivateEnemiesPeriodically());
        }
    }

    private IEnumerator ActivateEnemiesPeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(_activationInterval);

            for (int i = 0; i < 5; i++)
            {
                ActivateEnemy();
            }
        }
    }
}
