using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;


//this class concerns the thophies + the medals + AirportMedal
public class Trophy : MonoBehaviour
{
    public bool active = false;     //true if trophy unlocked
    public bool toBeActivated = false;
    public string trophyName;
    public string description;

    public Sprite wonSprite;

    void Awake()
    {
        this.trophyName = name;
    }
    void OnMouseDown()
    {
        Debug.Log("Clic on " + this.name);
    }

    void Update()
    {
        if(this.toBeActivated)
        {
            if (!this.active)
            {
                this.active = true;
                switch (this.trophyName)
                {
                    case "Medal1":
                    case "Medal2":
                    case "Medal3":
                        transform.localScale = new Vector3(5.6f, 5f, 1);
                        break;
                    case "Trophy1":
                    case "Trophy2":
                    case "Trophy3":
                        transform.localScale = new Vector3(3.2f, 3.8f, 1);
                        break;
                    case "AirportMedal":
                        transform.localScale = new Vector3(7f, 8f, 1);
                        break;
                }
            }
            gameObject.GetComponent<SpriteRenderer>().sprite = wonSprite;
        }
    }

    public bool changeToObtained()
    {
        this.toBeActivated = true;
        return true;
    }
}