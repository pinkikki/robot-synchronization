using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10.0f)] private float speed = 10.0f;

    [SerializeField, Range(30.0f, 150.0f)] private float mouseSensitive = 90.0f;

    private Vector3 _startMousePos;

    private Vector3 _presentCamRotation;

    private Vector3 _presentCamPos;

    private Quaternion _initialCamRotation;

    void Start()
    {
        _initialCamRotation = transform.rotation;
    }

    void Update()
    {
        Reset();
        Rotate();
        Slide();
        Move();
    }

    private void Reset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.rotation = _initialCamRotation;
        }
    }

    private void Rotate()
    {
        if (!Input.GetMouseButton(0)) return;

        var angle = new Vector3(
            Input.GetAxis("Mouse X") * speed,
            Input.GetAxis("Mouse Y") * speed,
            0
        );

        transform.RotateAround(Vector3.zero, Vector3.up, angle.x);
        transform.RotateAround(Vector3.zero, transform.right, angle.y);
    }

    private void Slide()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _startMousePos = Input.mousePosition;
            _presentCamPos = transform.position;
        }

        if (!Input.GetMouseButton(1)) return;
        var x = (_startMousePos.x - Input.mousePosition.x) / Screen.width;
        var y = (_startMousePos.y - Input.mousePosition.y) / Screen.height;

        x *= speed;
        y *= speed;

        var velocity = transform.rotation * new Vector3(x, y, 0);
        velocity += _presentCamPos;
        transform.position = velocity;
    }

    private void Move()
    {
        var campos = transform.position;

        if (Input.GetKey(KeyCode.H))
        {
            campos += transform.right * (Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.L))
        {
            campos -= transform.right * (Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.J))
        {
            campos += transform.up * (Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.K))
        {
            campos -= transform.up * (Time.deltaTime * speed);
        }

        transform.position = campos;
    }
}