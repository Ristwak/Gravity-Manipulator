using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public Transform cameraPoint;
    public float mouseSensitivity = 100f;
    public float maxLookUpAngle = 60f;
    public float maxLookDownAngle = -45f;
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, -3f);

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Awake()
    {
        transform.localPosition = cameraOffset;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, maxLookDownAngle, maxLookUpAngle);

        transform.position = Vector3.Lerp(transform.position, cameraPoint.position + cameraPoint.TransformDirection(cameraOffset)
        ,Time.deltaTime * 10f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        player.transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        AdjustCameraPosition();
    }

    private void AdjustCameraPosition()
    {
        Vector3 targetPosition = cameraPoint.position + cameraPoint.TransformDirection(cameraOffset);

        if (Physics.Linecast(cameraPoint.position, targetPosition, out RaycastHit hit))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = targetPosition;
        }
    }
}
