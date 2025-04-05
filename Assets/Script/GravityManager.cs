using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public float gravityStrength = 9.81f;
    public GameObject hologramPrefab;

    private Vector3 newGravityDirection = Vector3.down;
    private GameObject hologramInstance;
    private bool previewActive = false;

    private Vector3 gravityDir = Vector3.down;
    private float hologramRotationZ = 0f;

    void Start()
    {
        if (hologramPrefab)
        {
            hologramInstance = Instantiate(hologramPrefab, transform.position, Quaternion.identity);
            hologramInstance.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            SetPreview(new Vector3(0, 1, 0), 180f);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            SetPreview(new Vector3(0, -1, 0), 0f);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            SetPreview(new Vector3(-1, 0, 0), 90f);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SetPreview(new Vector3(1, 0, 0), -90f);

        if (Input.GetKeyDown(KeyCode.Return) && previewActive)
        {
            Physics.gravity = gravityDir.normalized * gravityStrength;
            Debug.Log("Gravity set to: " + Physics.gravity);

            if (hologramInstance)
                hologramInstance.SetActive(false);

            previewActive = false;

            ApplyPlayerOffset(gravityDir);
        }

        if (previewActive && hologramInstance)
        {
            float height = 2f;
            Vector3 headOffset = new Vector3(0, height, 0);
            hologramInstance.transform.position = transform.position + headOffset;
        }
    }

    void SetPreview(Vector3 direction, float rotationZ)
    {
        gravityDir = direction.normalized;
        hologramRotationZ = rotationZ;
        previewActive = true;

        if (hologramInstance)
        {
            hologramInstance.SetActive(true);

            float height = 2f;
            Vector3 headOffset = new Vector3(0, height, 0);
            hologramInstance.transform.position = transform.position + headOffset;

            Quaternion zTilt = Quaternion.AngleAxis(rotationZ, Vector3.forward);
            hologramInstance.transform.rotation = zTilt;
        }
    }

    void ApplyPlayerOffset(Vector3 direction)
    {
        float offsetDistance = 2f;
        transform.position += direction.normalized * offsetDistance;
    }
}
