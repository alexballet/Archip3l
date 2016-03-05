using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

    void OnMouseDown()
    {
        GameObject.Find("Network").GetComponent<Client>().sendData("@TABLE@1@BUILDING@CONSTRUCTION@SUCCESS");
        //GameObject.Find("Server").GetComponent<Server>().SendTest();
    }
}
