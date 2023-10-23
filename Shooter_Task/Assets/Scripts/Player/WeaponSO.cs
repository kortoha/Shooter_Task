using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class WeaponSO : ScriptableObject
{
    public Sprite icon;

    public float damage;

    public int clipCount;

    public int bulletsCountInClip;

    public int maxBulletsCountInClip;
}
