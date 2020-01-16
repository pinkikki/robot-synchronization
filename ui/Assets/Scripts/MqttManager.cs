using System;
using MQTTnet;
using MQTTnet.Client;

public class MqttManager
{
    private IMqttClient _mqttClient;

    private static MqttManager _instance;

    private IMqttClientOptions _options;
    public static MqttManager Instance => _instance ?? (_instance = new MqttManager());

    public void OnConnected(EventHandler<MqttClientConnectedEventArgs> handler)
    {
        _mqttClient.Connected += handler;
    }

    public void OnDisConnected(EventHandler<MqttClientDisconnectedEventArgs> handler)
    {
        _mqttClient.Disconnected += handler;
    }

    public void OnApplicationMessageReceived(EventHandler<MqttApplicationMessageReceivedEventArgs> handler)
    {
        _mqttClient.ApplicationMessageReceived += handler;
    }

    public async void Connect()
    {
        var factory = new MqttFactory();
        _mqttClient = factory.CreateMqttClient();

        _options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", /* PortNumber = */ 1883)
            .WithClientId(Guid.NewGuid().ToString())
            // .WithCredentials("your_MQTT_username", "your_MQTT_password")
            // .WithTls()
            .Build();

        await _mqttClient.ConnectAsync(_options);
    }

    public async void Subscribe(string topic)
    {
        await _mqttClient.SubscribeAsync(
            new TopicFilterBuilder()
                .WithTopic(topic)
                .Build());
    }

    public async void Publish(string topic, string payload)
    {
        await _mqttClient.PublishAsync(topic, payload);
    }

    public async void Disconnect()
    {
        await _mqttClient.DisconnectAsync();
    }
}