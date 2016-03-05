using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    void Awake()
    {
        Client client = GameObject.Find("Network").GetComponent<Client>();
        client.MessageBuildingConstructionEvent += Client_MessageConstructionBuildingEvent;
    }

    private void Client_MessageConstructionBuildingEvent(object sender, MessageEventArgs e)
    {
        Debug.Log("Processing event by test class : " + e.message);
    }
}
