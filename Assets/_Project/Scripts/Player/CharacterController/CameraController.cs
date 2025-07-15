using KBCore.Refs;
using UnityEngine;

public class CameraController : ValidatedMonoBehaviour
{
    #region Fields
    float currentXAngle;
    float currentYAngle;

    [Range(0f, 90f)] public float VerticalLimit = 35f;

    public float cameraSpeed = 50f;
    public bool smoothCameraRotation;
    [Range(1f, 50f)] public float cameraSmoothingFactor = 25f;
    [SerializeField, Anywhere] Transform cam;
    [SerializeField, Anywhere] InputReader inputReader;
    #endregion

    void Awake()
    {
        currentXAngle = transform.localRotation.eulerAngles.x;
        currentYAngle = transform.localRotation.eulerAngles.y;
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inputReader.EnablePlayerActions();
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inputReader.DisablePlayerActions();
    }
    private void OnDestroy()
    {
        OnDisable();
    }
    void Update()
    {
        RotateCamera(inputReader.LookDirection.x, -inputReader.LookDirection.y);
    }
    void RotateCamera(float horizontalInput, float verticalInput)
    {
        if (smoothCameraRotation)
        {
            horizontalInput = Mathf.Lerp(0, horizontalInput, Time.deltaTime * cameraSmoothingFactor);
            verticalInput = Mathf.Lerp(0, verticalInput, Time.deltaTime * cameraSmoothingFactor);
        }

        currentXAngle += verticalInput * cameraSpeed * Time.deltaTime;
        currentYAngle += horizontalInput * cameraSpeed * Time.deltaTime;

        currentXAngle = Mathf.Clamp(currentXAngle, -VerticalLimit, VerticalLimit);

        transform.localRotation = Quaternion.Euler(0, currentYAngle, 0);
        cam.localRotation = Quaternion.Euler(currentXAngle, 0, 0);
    }
}