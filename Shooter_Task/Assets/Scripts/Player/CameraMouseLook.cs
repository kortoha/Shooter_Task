using UnityEngine;

public class CameraMouseLook : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 1.5f;
    [SerializeField] private float _smoothing = 1.5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform lookAtPoint;

    private Vector2 mouseLook;
    private Vector2 smoothV;

    void Update()
    {
        var md = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        md = Vector2.Scale(md, new Vector2(_sensitivity * _smoothing, _sensitivity * _smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / _smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / _smoothing);
        mouseLook += smoothV;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -40f, 40f);

        transform.localRotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);

        lookAtPoint.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

        cameraTransform.LookAt(lookAtPoint);
    }

    public void ApplyUpwardRecoil(float recoilAmount)
    {
        mouseLook.y += recoilAmount;
    }
}