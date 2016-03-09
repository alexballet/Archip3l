using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    private Timer Timer;
    private Client Client;
    private GlobalResourceManager GlobalResourceManager;

    void Awake()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageSystemStartOfGame += Client_MessageSystemStartOfGame;

        this.Timer = gameObject.GetComponent<Timer>();
        this.Timer.Init(0.1f * 60f);
        this.Timer.FinalTick += Timer_FinalTick;

        this.GlobalResourceManager = GameObject.Find("Resources").GetComponent<GlobalResourceManager>();

        StartGame();
    }

    private void Client_MessageSystemStartOfGame(object sender, MessageEventArgs e)
    {
        Debug.Log("Starting game");
        this.StartGame();
    }
    void StartGame()
    {
        this.GlobalResourceManager.initResources();
        this.Timer.StartTimer();
    }
    private void Timer_FinalTick(object sender, System.EventArgs e)
    {
        this.Client.sendData("@30002");
    }
}
