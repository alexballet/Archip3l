using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace TouchScript.Examples.Cube
{
    
    public class main : MonoBehaviour
    {

        public const int nbChallengesMax = 3;
        public const int nbNotificationsMax = 5;

        void Awake()
        {
            //hidding of challenges and notifications at the beginning
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

        static public bool addNotification(string text)
        {
            for (int i = 1; i <= nbNotificationsMax; i++)
                if (GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text == string.Empty)
                {
                    GameObject.Find("Notif" + i.ToString()).GetComponent<Text>().text = text;
                    GameObject.Find("Notif" + i.ToString()).GetComponent<BoxCollider>().enabled = true;
                    return true;
                }
            return false;
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



        void Start()
        {
            //tests ---------------------------------

            /*for (int i = 0; i <= 5; i++)
            {
                //way of adding notifications
                if (!main.addNotification("gaga " + i.ToString()))
                    Debug.Log(i.ToString());
            }*/

            string toto = "Répondre VRAI;Proposition0;;VRAI;FAUX";
            string[] r = toto.Split(';');
            addChallenge(r, TypeChallenge.VraiFaux);

            //--------------------------------------------------
        }

        void Update()
        {
            //when receive a challenge : addChallenge
        }
    }
}
