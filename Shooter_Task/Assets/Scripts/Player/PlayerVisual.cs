using System;
using System.Collections;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private const string WALK_BOOL = "IsWalk";
    private const string RUN_BOOL = "IsRun";
    private const string JUMP_TRIGGER = "IsJump";
    private const string KNIFE_HIT_TRIGGER = "IsHit";
    private const string DEATH_TRIGGER = "Death";

    private WeaponStateVisual _weaponStateVisual;
    private Animator _animator;
    private float _knifeHitTime = 1.15f;
    private bool _isHit = false;
    private bool _isDead = false;

    [SerializeField] private Player _player;
    [SerializeField] private CameraMouseLook _cameraMouseLook;
    [SerializeField] private PlayerDamage _playerDamage;
    [SerializeField] private GameObject _weaponIcon;
    [SerializeField] private GameObject _bulletIcon;
    [SerializeField] private GameObject _aim;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;

    [NonSerialized] public bool isJumpOnce = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _weaponStateVisual = GetComponent<WeaponStateVisual>();
    }

    private void Update()
    {
        _animator.SetBool(WALK_BOOL, _player.IsWalk());
        _animator.SetBool(RUN_BOOL, _player.IsRun());

        if (!_player.IsGrounded() && !isJumpOnce)
        {
            _animator.SetTrigger(JUMP_TRIGGER);
            isJumpOnce = true;
        }

        if (_player.health <= 0 && !_isDead)
        {
            Death();
        }


        if (_weaponStateVisual.currentState == WeaponStateVisual.WeaponState.Knife && Input.GetMouseButtonDown(0))
        {
            if (!_isHit)
            {
                StartCoroutine(KnifeHit());
            }
        }

        if(EnemysDeathList.Instance.enemysDeathCount == 30)
        {
            Win();
        }
    }

    private IEnumerator KnifeHit()
    {
        _isHit = true;
        _animator.SetTrigger(KNIFE_HIT_TRIGGER);
        yield return new WaitForSeconds(_knifeHitTime);
        _isHit = false;
    }

    private void Death()
    {  
        _animator.SetTrigger(DEATH_TRIGGER);
        _isDead = true;
        _player.gameObject.SetActive(false);
        _cameraMouseLook.enabled = false;
        _playerDamage.enabled = false;
        _weaponIcon.SetActive(false);
        _bulletIcon.SetActive(false);
        _aim.SetActive(false);
        _losePanel.SetActive(true);
    }

    private void Win()
    {
        _winPanel.SetActive(true);
        _cameraMouseLook.enabled = false;
        _playerDamage.enabled = false;
        _weaponIcon.SetActive(false);
        _bulletIcon.SetActive(false);
        _aim.SetActive(false);
        _player.enabled = false;

        _animator.SetBool(WALK_BOOL, false);
        _animator.SetBool(RUN_BOOL, false);
    }
}