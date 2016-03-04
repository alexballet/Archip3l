using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

    void OnMouseDown()
    {
        GameObject.Find("Network").GetComponent<Client>().sendData("@Part1@Part2@Part3");
        //GameObject.Find("Server").GetComponent<Server>().SendTest();
    }
}
