using UnityEngine;
using System.Collections;

public class MinorIsland : MonoBehaviour {

    public BuildingManager buildingManager { get; private set; }
    public ResourceManager resourceManager { get; private set; }
    public string nameMinorIsland;

    void Awake()
    {
        buildingManager = ScriptableObject.CreateInstance<BuildingManager>();
        buildingManager.init(this);

        /*----------TEST--------*/

        if (nameMinorIsland == "sous_ile_1")
        {
            Challenge challenge = ScriptableObject.CreateInstance<Challenge>();
            challenge.init(TypeChallenge.VraiFaux, this);
        }
        

        /*------------------*/
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
