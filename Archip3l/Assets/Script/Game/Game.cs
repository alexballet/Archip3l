using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    public Client Client;

    void Awake()
    {
        //this.Client = GameObject.Find("Network").GetComponent<Client>();
        //this.Client.MessageSystemEndOfGameEvent += Client_MessageSystemEndOfGameEvent;
    }

    void Start()
    {
        //this.StartGame();
    }
    void StartGame()
    {
        this.Client.sendData("@30001");
    }
    private void Client_MessageSystemEndOfGameEvent(object sender, MessageEventArgs e)
    {
        //End of the game
    }
}
