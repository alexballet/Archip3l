using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    private Client.DelegateEvent delegateEvent;

    void Start()
    {
        Debug.Log("New test class");
        Client client = GameObject.Find("Network").GetComponent<Client>();
        this.delegateEvent = processEvent;
        client.MessageEvent += delegateEvent;
    }

    private void processEvent(string message)
    {
        Debug.Log("Processing event by test class");
    }
}
