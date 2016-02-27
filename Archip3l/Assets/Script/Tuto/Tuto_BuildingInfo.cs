using UnityEngine;
using System.Collections;

public class Tuto_BuildingInfo : MonoBehaviour {

    public string buildingClicked;
    public Tuto_MinorIsland island;
    
    void OnMouseDown()
    {
        island = this.transform.parent.parent.parent.GetComponent<Tuto_MinorIsland>();
        buildingClicked = island.buildingClicked;
        if (this.name == "Build")
        {
            Destroy(GameObject.Find("WheelCanvas_" + this.transform.parent.parent.parent.name));
            island.wheelPresent = false;
            island.createTuto_ChallengeBuild(buildingClicked);
        }

        Destroy(GameObject.Find(this.transform.parent.parent.name));
        island.buildingInfoPresent = false;
    }


    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {

    }
}
