using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Grpc.Core;
using Helloworld;
using System;
using Program;

public class Cube : MonoBehaviour
{
    Channel channel;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button> ();
        btn.onClick.AddListener (OnClick);
        Debug.Log("Start Running.");
        // OpenMatchClient.exec();
    }

    private async void OnClick()
    {
        Debug.Log("This is a successful sending.");
        OpenMatchClient.exec();
        // HelloWorld();
        Debug.Log("End Sending.");
    }

    private void HelloWorld()
    {
        try
        {
            string ip = "localhost:50051";
            channel = new Channel(ip, ChannelCredentials.Insecure);
            var client = new Greeter.GreeterClient(channel);

            HelloReply reply = client.SayHello(new HelloRequest { Name = "user" });
            Debug.Log(reply.Message);
        }
        catch (Exception e)
        {
           Debug.Log(e.Message);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
