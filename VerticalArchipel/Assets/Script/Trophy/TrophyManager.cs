using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TrophyManager : MonoBehaviour
{
    public List<Trophy> Trophies { get; private set; }
    private Client Client;
    private ResourceManager ResourceManager;
    public event EventHandler<TrophyObtainedEventArgs> TrophyObtained;

    public int airportsBuilt { get; private set; }

    void Awake()
    {
        this.airportsBuilt = 0;
        this.Client = GameObject.Find("Network").GetComponent<Client>();
        this.Client.MessageTrophyWonEvent += Client_MessageTrophyWonEvent;
        this.Client.MessageBuildingConstructionEvent += Client_MessageBuildingConstructionEvent;
        this.ResourceManager = GameObject.Find("Resources").GetComponent<ResourceManager>();
        this.Trophies = new List<Trophy>();

        //Trophy trophy1 = GameObject.Find("Medal1").GetComponent<Trophy>();  //new Trophy(1, "Trophée ressources", "Gain de ressources", 50, 50, null);
        //Trophy trophy2 = GameObject.Find("Medal2").GetComponent<Trophy>();  //new Trophy(2, "Trophée innovation", "Multiplication de la productivité par 1.5", 160, 50, null);
        //Trophy trophy3 = GameObject.Find("Medal3").GetComponent<Trophy>();  //new Trophy(3, "Trophée rapidité", "Multiplication de la productivité par 1.5", 270, 50, null);
        //Trophy trophy4 = GameObject.Find("Trophy1").GetComponent<Trophy>();  //new Trophy(4, "Trophée stratégie", "Une description", 380, 50, null);
        //Trophy trophy5 = GameObject.Find("Trophy2").GetComponent<Trophy>();   //new Trophy(5, "Trophée légende", "Une description", 490, 50, null);
        //Trophy trophy6 = GameObject.Find("Trophy3").GetComponent<Trophy>();
        //Trophy trophy7 = GameObject.Find("AirportMedal").GetComponent<Trophy>();

        //this.Trophies.Add(trophy1);
        //this.Trophies.Add(trophy2);
        //this.Trophies.Add(trophy3);
        //this.Trophies.Add(trophy4);
        //this.Trophies.Add(trophy5);

        this.Trophies.Add(GameObject.Find("Medal1").GetComponent<Trophy>());
        this.Trophies.Add(GameObject.Find("Medal2").GetComponent<Trophy>());
        this.Trophies.Add(GameObject.Find("Medal3").GetComponent<Trophy>());
        this.Trophies.Add(GameObject.Find("Trophy1").GetComponent<Trophy>());
        this.Trophies.Add(GameObject.Find("Trophy2").GetComponent<Trophy>());
        this.Trophies.Add(GameObject.Find("Trophy3").GetComponent<Trophy>());
        this.Trophies.Add(GameObject.Find("AirportMedal").GetComponent<Trophy>());
    }

    private void Client_MessageBuildingConstructionEvent(object sender, MessageEventArgs e)
    {
        if((string)e.message.Split('@').GetValue(2) == "Airport")
        {
            this.airportsBuilt += 1;
            if(this.airportsBuilt == 4)
            {
                changeTrophyToObtained(this.Trophies[6]);
            }
        }
    }
    private void Client_MessageTrophyWonEvent(object sender, MessageEventArgs e)
    {
        string trophyName = (string)e.message.Split('@').GetValue(2);

        //inifite loop if change to obtain because the network message is send by this instance
        //changeTrophyToObtained(this.getTrophy(trophyName));
    }
    public List<Trophy> getTrophies(bool status)
    {
        List<Trophy> value = new List<Trophy>();
        if (this.Trophies.Count < 1)
        {
            return value;
        }
        for (int i = 0; i < this.Trophies.Count - 1; i++)
        {
            if (this.Trophies[i].active == status)
            {
                value.Add(this.Trophies[i]);
            }
        }
        return value;
    }
    public Trophy getTrophy(string name)
    {
        foreach (Trophy t in this.Trophies)
        {
            if (t.trophyName == name)
            {
                return t;
            }
        }
        return null;
    }
    public void changeTrophyToObtained(Trophy trophy)
    {
        trophy.changeToObtained();
        switch(trophy.name)
        {
            case "Medal3":
            case "Trophy3":
                //Score to add must be checked
                this.Client.sendData("@30505@" + 200.ToString());
                break;
            case "Medal2":
            case "Trophy2":
                //Score to add must be checked
                this.Client.sendData("@30505@" + 300.ToString());
                break;
            case "Medal1":
            case "Trophy1":
                //Score to add must be checked
                this.Client.sendData("@30505@" + 400.ToString());
                break;
            case "Airport":
                //Score to add must be checked
                this.Client.sendData("@30505@" + 500.ToString());
                break;
        }
        if (this.TrophyObtained != null)
        {
            this.TrophyObtained(this, new TrophyObtainedEventArgs { Trophy = trophy });
        }
    }
    IEnumerator checkNewTrophyAvailable()
    {
        for(;;)
        {
            foreach (Trophy trophy in this.Trophies)
            {
                if(trophy.requirementVerified(this.ResourceManager))
                {
                    changeTrophyToObtained(trophy);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}



public class TrophyObtainedEventArgs : EventArgs
{
    public Trophy Trophy;
}
