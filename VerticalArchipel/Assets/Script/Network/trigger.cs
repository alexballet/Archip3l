﻿using UnityEngine;
using System.Collections;

public class trigger : MonoBehaviour {

    void OnMouseDown()
    {
        GameObject.Find("Network").GetComponent<Client>().sendData("@21354@Gold@20");
        //GameObject.Find("Server").GetComponent<Server>().SendTest();
    }
}