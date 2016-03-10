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
        this.Client.MessageSystemStartOfGameEvent += Client_MessageSystemStartOfGame;
        this.Client.MessageSystemStartInitOfGameEvent += Client_MessageSystemStartInitOfGame;

        this.Timer = gameObject.GetComponent<Timer>();
        this.Timer.Init(0.1f * 60f);
        this.Timer.FinalTick += Timer_FinalTick;

        this.GlobalResourceManager = GameObject.Find("Resources").GetComponent<GlobalResourceManager>();
        this.GlobalResourceManager.MessageInitialized += GlobalResourceManager_MessageInitialized;
    }

    

    //void Start()
    //{
    //    StartGame();
    //}
    private void Client_MessageSystemStartOfGame(object sender, MessageEventArgs e)
    {
        Debug.Log("Starting game");
        this.Timer.StartTimer();
    }
    private void Client_MessageSystemStartInitOfGame(object sender, MessageEventArgs e)
    {
        Debug.Log("Start initializing game");
        StartCoroutine(this.GlobalResourceManager.initResources());
    }
    private void GlobalResourceManager_MessageInitialized(object sender, System.EventArgs e)
    {
        this.Client.sendData("@30087");
    }
    private void Timer_FinalTick(object sender, System.EventArgs e)
    {
        this.Client.sendData("@30002");
    }
}
