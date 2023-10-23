using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private WeaponStateVisual _weaponStateVisual;

    public void OnPistolButtonClick()
    {
        _weaponStateVisual.ChangeState(WeaponStateVisual.WeaponState.Pistol);
    }

    public void OnKnifeButtonClick()
    {
        _weaponStateVisual.ChangeState(WeaponStateVisual.WeaponState.Knife);
    }

    public void OnRifleButtonClick()
    {
        _weaponStateVisual.ChangeState(WeaponStateVisual.WeaponState.Rifle);
    }
}