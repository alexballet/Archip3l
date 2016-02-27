using UnityEngine;
using System.Collections;

public class BuildingInfo : MonoBehaviour {

    public string buildingClicked { get; private set; }
    public MinorIsland island { get; private set; }

    void OnMouseDown()
    {
        island = this.transform.parent.parent.parent.GetComponent<MinorIsland>();
        buildingClicked = island.buildingClicked;
        if (this.name == "Build")
        {
            Destroy(GameObject.Find("WheelCanvas_" + this.transform.parent.parent.parent.name));
            island.wheelPresent = false;
            island.createChallengeBuild(buildingClicked);
            island.challengePresent = true;
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
