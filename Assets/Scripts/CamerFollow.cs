using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 15f, -20f);
    [SerializeField] private float _smoothSpeed = 5f;

    private void LateUpdate()
    }
        Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
        transform.LookAt(_target);
    }
}