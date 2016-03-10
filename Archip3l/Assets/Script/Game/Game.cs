using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    public Client Client;

    void Awake()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemEndOfGameEvent += Client_MessageSystemEndOfGameEvent;
        this.Client.MessageSystemStartInitofGameAnswerEvent += Client_MessageSystemStartInitofGameAnswerEvent;
    }

    

    void Start()
    {
        //Launch initialization of the game
        this.Client.sendData("@30006");
    }
    private void Client_MessageSystemStartInitofGameAnswerEvent(object sender, MessageEventArgs e)
    {
        //Launch game
        this.Client.sendData("@30001");
    }
    private void Client_MessageSystemEndOfGameEvent(object sender, MessageEventArgs e)
    {
        //End of the game
    }
}
