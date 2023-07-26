using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private const float Y_ANGLE_MIN = 4.0f;
    private const float Y_ANGLE_MAX = 50.0f;
    private const float DISTANCE_MIN = 10.0f;
    private const float DISTANCE_MAX = 20.0f;
    private const float SMOOTH_TIME = 0.1f;

    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    private float distance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private float sensivityX = 10.0f;
    private float sensivityY = 1.0f;

    private Vector3 smoothVelocity = Vector3.zero;

    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensivityY;

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        distance = Mathf.Clamp(distance, DISTANCE_MIN, DISTANCE_MAX);
    }

    private void Update()
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 targetPosition = lookAt.position - rotation * Vector3.forward * distance;

        camTransform.position = Vector3.SmoothDamp(camTransform.position, targetPosition, ref smoothVelocity, SMOOTH_TIME);
        camTransform.LookAt(lookAt.position);
    }
}
