using UnityEngine;
using UnityEngine.UI;
using UniversalNetworkInput.Network;
using UniversalNetworkInput.Network.Internal;

public class ServerInitializer : MonoBehaviour {

    public Text label;

	// Use this for initialization
	void Start () {
        //Initialize Server
        UNServer.Start(4, 25565, UNNetwork.GetLocalIPAddress());

        //Set ip label
        label.text += UNServer.ip_address;
	}

}