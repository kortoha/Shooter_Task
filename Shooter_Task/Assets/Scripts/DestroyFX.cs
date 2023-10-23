using UnityEngine;

public class DestroyFX : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("DestroySelf", 0.1f);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
