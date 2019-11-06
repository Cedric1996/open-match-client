using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Grpc.Core;
using UnityEngine.Ucg.Matchmaking;

public class Cube : MonoBehaviour
{
    Channel channel;
    private string endpoint;
    private Matchmaker matchmaker;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button> ();
        btn.onClick.AddListener (OnClick);
        endpoint = "172.17.129.6:30593";
        Debug.Log("Start Running.");
        matchmaker = new Matchmaker(endpoint);
        // OpenMatchClient.exec();
    }

    private async void OnClick()
    {
        matchmaker.RequestMatch("123");
    }

    private void HelloWorld()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
