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

        // Local rotation
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        // Position behind the player
        Vector3 targetPos = cameraPoint.position + cameraPoint.TransformDirection(cameraOffset);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 10f);

        // Optional: Adjust player facing direction
        // player.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
