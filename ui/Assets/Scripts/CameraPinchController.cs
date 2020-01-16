using UnityEngine;

public class CameraPinchController : MonoBehaviour
{
    private Camera _camera;

    private const float VMin = 1.0f;
    private const float VMax = 5.0f;

    //直前の2点間の距離.
    private float _backDist;

    //初期値
    private float _v = 15.0f;

    [SerializeField] private float fluctuationVal = 1.0f;


    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _v += fluctuationVal;
            _camera.orthographicSize = _v;
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            _v -= fluctuationVal;
            _camera.orthographicSize = _v;
        }

        if (Input.touchCount < 2) return;
        var t1 = Input.GetTouch(0);
        var t2 = Input.GetTouch(1);

        if (t2.phase == TouchPhase.Began)
        {
            _backDist = Vector2.Distance(t1.position, t2.position);
        }
        else if (t1.phase == TouchPhase.Moved && t2.phase == TouchPhase.Moved)
        {
            var newDist = Vector2.Distance(t1.position, t2.position);
            _v += (newDist - _backDist) / 1000.0f;

            if (_v > VMax)
            {
                _v = VMax;
            }
            else if (_v < VMin)
            {
                _v = VMin;
            }

            _camera.orthographicSize = _v;
        }
    }
}