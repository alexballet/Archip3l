using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

    void OnMouseDown()
    {
        //The message must start with @ because ip sender automaticaly added
        //See the GDrive to get code signification
        //Message Format : @code

        //GameObject.Find("Network").GetComponent<Client>().sendData("@21111");
        GameObject.Find("Network").GetComponent<Client>().sendData("@21354@Gold@20");
        //GameObject.Find("Network").GetComponent<Client>().sendData("@TABLE@1@BUILDING@CONSTRUCTION@SUCCESS");
        //GameObject.Find("Server").GetComponent<Server>().SendTest();
    }
}
