using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    [SerializeField] private GameObject _weaponSelector;
    [SerializeField] private CameraMouseLook _mouseLook;
    private bool _selectorVisible = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ShowSelector();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            HideSelector();
        }
    }

    private void ShowSelector()
    {
        _selectorVisible = true;
        Time.timeScale = 0f;
        _weaponSelector.SetActive(true);
        _mouseLook.enabled = false;
    }

    private void HideSelector()
    {
        _selectorVisible = false;
        Time.timeScale = 1f;
        _weaponSelector.SetActive(false);
        _mouseLook.enabled = true;
    }
}