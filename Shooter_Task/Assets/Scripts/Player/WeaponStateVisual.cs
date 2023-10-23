using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStateVisual : MonoBehaviour
{
    public enum WeaponState
    {
        Pistol,
        Knife,
        Rifle
    }

    [NonSerialized] public int pistolBulletsCount;
    [NonSerialized] public int maxPistolBulletsCount;
    [NonSerialized] public int pistolClipCount;
    [NonSerialized] public int rifleBulletsCount;
    [NonSerialized] public int rifleClipCount;
    [NonSerialized] public int maxRifleBulletsCount;

    private Animator _animator;
    private int _pistolLayer = 0;
    private int _rifleLayer = 1;
    private int _knifeLayer = 2;

    [SerializeField] private GameObject _pistol;
    [SerializeField] private GameObject _bulletIcon;
    [SerializeField] private Image _weaponIcon;
    [SerializeField] private Text _ammoText;
    [SerializeField] private WeaponSO[] _weaponSOArray;

    [NonSerialized] public WeaponState currentState;

    private void Start()
    {
        _animator = GetComponent <Animator>();

        pistolBulletsCount = _weaponSOArray[(int)WeaponState.Pistol].bulletsCountInClip;
        maxPistolBulletsCount = _weaponSOArray[(int)WeaponState.Pistol].maxBulletsCountInClip;
        pistolClipCount = _weaponSOArray[(int)WeaponState.Pistol].clipCount;
        rifleBulletsCount = _weaponSOArray[(int)WeaponState.Rifle].bulletsCountInClip;
        maxRifleBulletsCount = _weaponSOArray[(int)WeaponState.Rifle].maxBulletsCountInClip;
        rifleClipCount = _weaponSOArray[(int)WeaponState.Rifle].clipCount;

        ChangeState(WeaponState.Pistol);
    }

    private void Update()
    {
        UpdateUI();
    }

    public void ChangeState(WeaponState newState)
    {
        _pistol.SetActive(false);
        _bulletIcon.SetActive(false);

        switch (newState)
        {
            case WeaponState.Pistol:
                _pistol.SetActive(true);
                SetLayerWeight(_pistolLayer, 1.0f);
                SetLayerWeight(_knifeLayer, 0f);
                SetLayerWeight(_rifleLayer, 0f);
                _weaponIcon.sprite = _weaponSOArray[(int)WeaponState.Pistol].icon;
                _ammoText.text = GetAmmoText(pistolBulletsCount, pistolClipCount);
                _bulletIcon.SetActive(true);
                break;

            case WeaponState.Knife:
                SetLayerWeight(_knifeLayer, 1.0f);
                SetLayerWeight(_rifleLayer, 0f);
                _weaponIcon.sprite = _weaponSOArray[(int)WeaponState.Knife].icon;
                break;

            case WeaponState.Rifle:
                SetLayerWeight(_rifleLayer, 1.0f);
                SetLayerWeight(_knifeLayer, 0f);
                _weaponIcon.sprite = _weaponSOArray[(int)WeaponState.Rifle].icon;
                _ammoText.text = GetAmmoText(rifleBulletsCount, rifleClipCount);
                _bulletIcon.SetActive(true);
                break;
        }

        currentState = newState;
    }

    private void SetLayerWeight(int layerIndex, float weight)
    {
        _animator.SetLayerWeight(layerIndex, weight);
    }

    public void UpdateUI()
    {
        if (currentState == WeaponState.Pistol)
        {
            _ammoText.text = GetAmmoText(pistolBulletsCount, pistolClipCount);
        }
        else if (currentState == WeaponState.Rifle)
        {
            _ammoText.text = GetAmmoText(rifleBulletsCount, rifleClipCount);
        }
    }

    private string GetAmmoText(int bullets, int clip)
    {
        return bullets.ToString() + " / " + clip.ToString();
    }
}