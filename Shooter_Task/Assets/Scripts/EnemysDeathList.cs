using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemysDeathList : MonoBehaviour
{
    [NonSerialized] public int enemysDeathCount = 0;

    [SerializeField] private Text _enemysCount;

    public static EnemysDeathList Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        EnemysDeathListUpdate();
    }

    private void EnemysDeathListUpdate()
    {
        _enemysCount.text = enemysDeathCount.ToString() + " / 30";
    }
}
