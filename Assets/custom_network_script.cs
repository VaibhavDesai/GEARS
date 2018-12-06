using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class custom_network_script : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
        {
            Destroy(this);
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
