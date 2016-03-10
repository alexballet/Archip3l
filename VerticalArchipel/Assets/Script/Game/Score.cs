using UnityEngine;
using System.Collections;
using System;

public class Score : MonoBehaviour {

    private Client Client;
    private int ScoreCount;
    void Awake()
    {
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageScoreUpdateEvent += Client_MessageScoreUpdateEvent;

        this.ScoreCount = 0;
    }

    private void Client_MessageScoreUpdateEvent(object sender, MessageEventArgs e)
    {
        int scoreToAdd = Int32.Parse((string)e.message.Split('@').GetValue(2));
        if(scoreToAdd > 0)
        {
            this.ScoreCount += scoreToAdd;
        }
    }
}
