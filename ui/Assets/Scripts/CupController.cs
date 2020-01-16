using System.Text;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class CupController : MonoBehaviour
{
    // private Vector3 _velocity;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private string topic = "/robot/topic";

    void Start()
    {
        var mqttManager = MqttManager.Instance;
        var context = SynchronizationContext.Current;
        mqttManager.Connect();
        mqttManager.OnConnected(
            async (s, e) =>
            {
                Debug.Log("connected to matt server");
                mqttManager.Subscribe(topic);
                // context.Post(_ => { InvokeRepeating("Publish", 0.0f, 2.0f); }, null);
            });

        mqttManager.OnDisConnected(
            async (s, e) => { Debug.Log("disconnected mqtt server"); }
        );

        mqttManager.OnApplicationMessageReceived(
            (s, e) =>
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Debug.Log($"subscribed. message=[{message}]");
                var robot = JsonUtility.FromJson<Robot>(message);
                Debug.Log($"x={robot.x}, y={robot.y}, z={robot.z}, emergency={robot.emergency}");
                context.Post(_ => { transform.localPosition = new Vector3(robot.x, robot.y, robot.z); }, null);
            }
        );
    }

    private void Publish()
    {
        var pos = transform.localPosition;
        MqttManager.Instance.Publish(topic, $"{{\"x\": {pos.x}, \"y\": {pos.y}, \"z\": {pos.z}}}");
    }

    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var _velocity = new Vector3(v, h, 0);
        _velocity = transform.TransformDirection(_velocity);
        if (v > 0.1 || h > 0.1)
        {
            _velocity *= speed;
        }
        else if (v < -0.1 || h < -0.1)
        {
            _velocity *= speed;
        }
        else
        {
            return;
        }

        transform.localPosition += _velocity * Time.fixedDeltaTime;
    }

    private void OnApplicationQuit()
    {
        MqttManager.Instance.Disconnect();
    }
}