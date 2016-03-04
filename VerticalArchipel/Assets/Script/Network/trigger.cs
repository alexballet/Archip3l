using UnityEngine;
using System.Collections;

public class trigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        GameObject.Find("Network").GetComponent<Client>().sendData("Message from client on server machine");
        //GameObject.Find("Server").GetComponent<Server>().SendTest();
    }
}
