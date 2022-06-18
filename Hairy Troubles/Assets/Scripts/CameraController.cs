using UnityEngine;
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float smoothSpeed = 0.125f;
    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;
    [SerializeField] bool lookAt;
    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + positionOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        if(lookAt)
        {
            transform.LookAt(player);
        }
        else
        {
            transform.rotation = Quaternion.Euler(rotationOffset.x, rotationOffset.y, rotationOffset.z);
        }
    }
}
 