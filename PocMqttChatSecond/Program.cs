﻿using System;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

static MqttClient ConnectMQTT(string broker, int port, string clientId, string username, string password)
{
    MqttClient client = new MqttClient(broker, port, false, MqttSslProtocols.None, null, null);
    client.Connect(clientId, username, password);
    if (client.IsConnected)
    {
        Console.WriteLine("Connected to MQTT Broker");
    }
    else
    {
        Console.WriteLine("Failed to connect");
    }
    return client;
}
static void Subscribe(MqttClient client, string topic)
{
    client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
    client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
}
static void Publish(MqttClient client, string topic, string msg)
{
    client.Publish(topic, System.Text.Encoding.UTF8.GetBytes(msg));
    Console.WriteLine("Send `{0}` to topic `{1}`", msg, topic);
}
static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
{
    string payload = System.Text.Encoding.Default.GetString(e.Message);
    Console.WriteLine("Received `{0}` from `{1}` topic", payload, e.Topic.ToString());
}

string broker = "broker.emqx.io";
int port = 1883;
string topic = "Csharp/mqtt";
string clientId = Guid.NewGuid().ToString();
string username = "emqx";
string password = "public";
MqttClient client = ConnectMQTT(broker, port, clientId, username, password);
Subscribe(client, topic);

Console.WriteLine("bSecond");
var response = "";
while (response != ":q")
{
    //Console.Clear();
    Console.Write("Qual a mensagem? ");
    response = Console.ReadLine();
    if (response != ":q") {
        Publish(client, topic, response ?? "Mensagem");
        response = "";
    }
}

Console.WriteLine("bye bye!");
