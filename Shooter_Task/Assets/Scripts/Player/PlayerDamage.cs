using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private const string ENEMY_LAYER = "Enemy";

    [SerializeField] private WeaponStateVisual _weaponStateVisual;
    [SerializeField] private AudioSource _pistolShot;
    [SerializeField] private AudioSource _rifleShot;
    [SerializeField] private AudioSource _knifeHit;
    [SerializeField] private AudioSource _reloadSound;
    [SerializeField] private GameObject _aimTarget;
    [SerializeField] private GameObject _sparkle;
    [SerializeField] private GameObject _muzzle;
    [SerializeField] private Transform _pistolMuzzlePos;
    [SerializeField] private Transform _rifleMuzzlePos;
    [SerializeField] private WeaponSO _pistolSO;
    [SerializeField] private WeaponSO _rifleSO;
    [SerializeField] private WeaponSO _knifeSO;

    private CameraMouseLook _cameraMouseLook;
    private Player _player;
    private float _pistolRecoil = 3f;
    private float _rifleRecoil = 5f;
    private float _reloadTime = 3f;
    private float _knifeHitTime = 1f;
    private bool _isReload = false;
    private bool _isHit = false;
    private Vector3 _playerDirection;

    private void Start()
    {
        _cameraMouseLook = GetComponent<CameraMouseLook>();
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (_weaponStateVisual.currentState == WeaponStateVisual.WeaponState.Pistol && Input.GetMouseButtonDown(0))
        {
            PistolDamage();
        }
        else if (_weaponStateVisual.currentState == WeaponStateVisual.WeaponState.Knife && Input.GetMouseButtonDown(0))
        {
            KnifeDamage();
        }
        else if (_weaponStateVisual.currentState == WeaponStateVisual.WeaponState.Rifle && Input.GetMouseButtonDown(0))
        {
            RifleDamage();
        }

        _playerDirection = transform.forward;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(ENEMY_LAYER))
        {
            if (_weaponStateVisual.currentState == WeaponStateVisual.WeaponState.Knife && Input.GetMouseButtonDown(0))
            {
                EnemyDamage enemyDamage = other.GetComponent<EnemyDamage>();
                enemyDamage.GetDamage(_knifeSO.damage);
            }
        }
    }

    private void KnifeDamage()
    {
        if (Time.timeScale != 0 && !_isHit)
        {
            StartCoroutine(KnifeHit());
        }
    }

    private void PistolDamage()
    {
        WeaponShot(_pistolShot, _weaponStateVisual.pistolBulletsCount, _weaponStateVisual.maxPistolBulletsCount,
            _weaponStateVisual.pistolClipCount, _pistolRecoil, _pistolMuzzlePos, _pistolSO);
    }

    private void RifleDamage()
    {
        WeaponShot(_rifleShot, _weaponStateVisual.rifleBulletsCount, _weaponStateVisual.maxRifleBulletsCount,
            _weaponStateVisual.rifleClipCount, _rifleRecoil, _rifleMuzzlePos, _rifleSO);
    }

    private void WeaponShot(AudioSource audioSource, int bullets, int maxBullets, int clip, float recoil, Transform transform, WeaponSO weaponSO)
    {
        if (bullets > 0 && !_isReload && clip >= 0)
        {
            if (Time.timeScale != 0)
            {
                bullets--;

                MuzzleShot(transform);

                Vector3 screenPos = Camera.main.WorldToScreenPoint(_aimTarget.transform.position);

                Ray ray = Camera.main.ScreenPointToRay(screenPos);

                RaycastHit hitInfo;

                int layerMask = LayerMask.GetMask(ENEMY_LAYER) | LayerMask.GetMask("Ground") | LayerMask.GetMask("Default");
                if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layerMask, QueryTriggerInteraction.Ignore))
                {
                    if (hitInfo.collider != null)
                    {
                        Instantiate(_sparkle, hitInfo.point, Quaternion.identity);

                        if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer(ENEMY_LAYER))
                        {
                            EnemyDamage enemyDamage = hitInfo.collider.GetComponent<EnemyDamage>();
                            enemyDamage.GetDamage(weaponSO.damage);
                        }
                    }
                }

                if (bullets == 0 && clip > 0)
                {
                    StartCoroutine(Reload());
                }

                if (_weaponStateVisual.currentState == WeaponStateVisual.WeaponState.Pistol)
                {
                    _weaponStateVisual.pistolBulletsCount = bullets;
                    _weaponStateVisual.pistolClipCount = clip;
                }
                else if (_weaponStateVisual.currentState == WeaponStateVisual.WeaponState.Rifle)
                {
                    _weaponStateVisual.rifleBulletsCount = bullets;
                    _weaponStateVisual.rifleClipCount = clip;
                }

                audioSource.Play();
                _cameraMouseLook.ApplyUpwardRecoil(recoil);
            }
        }
    }

    private IEnumerator Reload()
    {
        _aimTarget.SetActive(false);
        _isReload = true;
        _reloadSound.Play();
        yield return new WaitForSeconds(_reloadTime);

        if (_weaponStateVisual.currentState == WeaponStateVisual.WeaponState.Pistol)
        {
            _weaponStateVisual.pistolBulletsCount = _weaponStateVisual.maxPistolBulletsCount;
            _weaponStateVisual.pistolClipCount--;
        }
        else if (_weaponStateVisual.currentState == WeaponStateVisual.WeaponState.Rifle)
        {
            _weaponStateVisual.rifleBulletsCount = _weaponStateVisual.maxRifleBulletsCount;
            _weaponStateVisual.rifleClipCount--;
        }

        _weaponStateVisual.UpdateUI();
        _isReload = false;
        _aimTarget.SetActive(true);
    }

    private IEnumerator KnifeHit()
    {
        _player.enabled = false;
        _knifeHit.Play();
        _isHit = true;
        yield return new WaitForSeconds(_knifeHitTime);
        _isHit = false;
        _player.enabled = true;
    }

    private void MuzzleShot(Transform transform)
    {
        Instantiate(_muzzle, transform.position, Quaternion.LookRotation(_playerDirection));
    }
}