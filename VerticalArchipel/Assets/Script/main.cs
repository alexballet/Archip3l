using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class main : MonoBehaviour {

    public const int nbChallengesMax = 3;
    public const int nbNotificationsMax = 5;

    void Awake()
    {
        //hidding of challenges and notifications at the beginning
        for (int i = 1; i <= nbChallengesMax; i++)
        {
            GameObject.Find("Challenge" + i.ToString()).GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("Challenge" + i.ToString()).GetComponent<PolygonCollider2D>().enabled = false;
        }
        for (int i = 1; i <= nbNotificationsMax; i++)
        {
            GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text = string.Empty;
            GameObject.Find("Notif" + i.ToString()).GetComponent<BoxCollider2D>().enabled = false;
        }

    }

    //trophy = trophies + medals + AirportMedal
    static public void unlockTrophy(string nameTrophyGameObject)
    {
        GameObject.Find(nameTrophyGameObject).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Trophies/" + nameTrophyGameObject);
    }


    static public bool addChallenge(string[] row)
    {
        for (int i = 1; i <= nbChallengesMax; i++)
            if (GameObject.Find("Challenge" + i.ToString()).GetComponent<SpriteRenderer>().enabled == false)
            {
                GameObject.Find("Challenge" + i.ToString()).GetComponent<SpriteRenderer>().enabled = true;
                GameObject.Find("Challenge" + i.ToString()).GetComponent<PolygonCollider2D>().enabled = true;
                GameObject.Find("Challenge" + i.ToString()).GetComponent<ChallengeVertical>().rowSent = row;
                return true;
            }
        return false;        
    }

    static public bool addNotification(string text)
    {
        for (int i = 1; i <= nbNotificationsMax; i++)
            if (GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text == string.Empty)
            {
                GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text = text;
                GameObject.Find("Notif" + i.ToString()).GetComponent<BoxCollider2D>().enabled = true;
                return true;
            }
        return false;
    }

    static public IEnumerator removeNotification(GameObject go)  //id : last character of the notification's name
    {
        Text notif = go.GetComponent<Text>();
        Color color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            color = notif.material.color;
            color.a -= 0.01f;
            notif.material.color = color;
        }

        //reset alpha to 1 because bug --> all gameobjects touched          -->   TODO : manage this
        color = notif.material.color;
        color.a = 1;
        notif.material.color = color;

        //each notification below the removed one goes up
        for (int i = int.Parse(go.name[go.name.Length - 1].ToString()); i < nbNotificationsMax; i++)
        {
            GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text = GameObject.Find("Notif" + (i + 1).ToString()).GetComponent<Text>().text;
        }
        GameObject.Find("Notif" + nbNotificationsMax).GetComponent<Text>().text = string.Empty;
    }



    void Start () {
        for (int i = 0; i <= 5; i++)
        {
            if (!main.addNotification("gaga " + i.ToString()))
                Debug.Log(i.ToString());
        }


    }

    void Update () {
	
	}
}
