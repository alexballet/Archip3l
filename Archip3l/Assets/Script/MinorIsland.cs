using UnityEngine;
using System.Collections;

public class MinorIsland : MonoBehaviour {

    public BuildingManager buildingManager { get; private set; }
    public ResourceManager resourceManager { get; private set; }
    public string nameMinorIsland;

    void Awake()
    {
        buildingManager = GameObject.Find(nameMinorIsland).GetComponent<BuildingManager>();
        buildingManager.init(this);

        /*resourceManager = GameObject.Find(nameMinorIsland).AddComponent<ResourceManager>();
        resourceManager.init(this);*/

        /*----------TEST--------*/

        createChallenge();
        

        /*------------------*/
    }

    public void createChallenge()
    {
        if (nameMinorIsland == "sous_ile_1" || nameMinorIsland == "sous_ile_4")
        {
            Challenge challenge = GameObject.Find("Virtual_" + nameMinorIsland).AddComponent<Challenge>();
            challenge.init(TypeChallenge.QCM, this);
        }
    }


    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("toto " + i.ToString());
            }
        }
	}
}
