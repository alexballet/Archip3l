using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using TouchScript.InputSources;


public class main : MonoBehaviour
{

    public const int nbChallengesMax = 3;
    public const int nbNotificationsMax = 5;

    void Awake()
    {
        //hiding challenges and notifications at the beginning
        for (int i = 1; i <= nbChallengesMax; i++)
        {
            GameObject.Find("Challenge" + i.ToString()).GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("Challenge" + i.ToString()).GetComponent<BoxCollider>().enabled = false;
        }
        for (int i = 1; i <= nbNotificationsMax; i++)
        {
            GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text = string.Empty;
            GameObject.Find("Notif" + i.ToString()).GetComponent<BoxCollider>().enabled = false;
        }

    }

    //trophy = trophies + medals + AirportMedal
    static public void unlockTrophy(string nameTrophyGameObject)
    {
        GameObject.Find(nameTrophyGameObject).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Trophies/" + nameTrophyGameObject);
    }


    static public bool addChallenge(string[] row, TypeChallenge tc)
    {
        for (int i = 1; i <= nbChallengesMax; i++)
            if (GameObject.Find("Challenge" + i.ToString()).GetComponent<SpriteRenderer>().enabled == false)
            {
                GameObject.Find("Challenge" + i.ToString()).GetComponent<SpriteRenderer>().enabled = true;
                GameObject.Find("Challenge" + i.ToString()).GetComponent<BoxCollider>().enabled = true;
                GameObject.Find("Challenge" + i.ToString()).GetComponent<ChallengeVertical>().rowSent = row;
                GameObject.Find("Challenge" + i.ToString()).GetComponent<ChallengeVertical>().typeChallenge = tc;
                return true;
            }
        return false;
    }

    static public void removeChallenge(GameObject go)
    {
        go.GetComponent<SpriteRenderer>().enabled = false;
        go.GetComponent<BoxCollider>().enabled = false;
    }

    static public void addNotification(string text)
    {
        for (int i = 1; i <= nbNotificationsMax; i++)
            if (GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text == string.Empty)
            {
                GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text = text;
                GameObject.Find("Notif" + i.ToString()).GetComponent<BoxCollider>().enabled = true;
                return;
            }
        //no place --> make some place to add this notification
        removeNotification(GameObject.Find("Notif1"));
        GameObject.Find("Notif5").GetComponent<Text>().text = text;
        GameObject.Find("Notif5").GetComponent<BoxCollider>().enabled = true;
    }

    static public void removeNotification(GameObject go)  //id : last character of the notification's name
    {
            
        //each notification below the removed one goes up
        for (int i = int.Parse(go.name[go.name.Length - 1].ToString()); i < nbNotificationsMax; i++)
        {
            GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text = GameObject.Find("Notif" + (i + 1).ToString()).GetComponent<Text>().text;
        }
        GameObject.Find("Notif" + nbNotificationsMax).GetComponent<Text>().text = string.Empty;
    }


    static public bool addDisturbance(/*TypeDisturbance disturbanceType*/)
    {
        if (!Disturbance.disturbanceWindowOpen)
        {
            Disturbance.disturbanceWindowOpen = true;
            Canvas eventCanvasPrefab = Resources.Load<Canvas>("Prefab/DisturbanceCanvas");
            Canvas eventCanvas = Instantiate(eventCanvasPrefab);
            eventCanvas.name = "DisturbanceCanvas";
            /*foreach (Disturbance e in eventCanvas.GetComponentsInChildren<Disturbance>())
            {
                e.disturbanceType = disturbanceType;
            }*/
               
        }
        return false;
    }

    static public void addEnigma()
    {
        Canvas enigmaPrefab = Resources.Load<Canvas>("Prefab/Enigma");
        Canvas enigma = Instantiate(enigmaPrefab);
        enigma.name = "Enigma";
        Enigma.enigmaWindowOpen = true;
    }



    void Start()
    {
        //tests ---------------------------------

        for (int i = 0; i < main.nbNotificationsMax; i++)
        {
            addNotification("gaga " + i.ToString());
        }

        string toto = "Répondre VRAI;Proposition0;;VRAI;FAUX";
        string[] r = toto.Split(';');
        addChallenge(r, TypeChallenge.VraiFaux);

        //addDisturbance();

        //--------------------------------------------------
    }

    void Update()
    {
        //when receive a challenge : addChallenge
    }
}
