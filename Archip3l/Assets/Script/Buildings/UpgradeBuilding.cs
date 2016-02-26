using UnityEngine;
using System.Collections;

public class UpgradeBuilding : MonoBehaviour {

    public MinorIsland island;
    public Building building;
    
    void OnMouseDown()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        building = GameObject.Find(island.nameBuildingTouchCanvas).GetComponent<Building>();

        if (this.name == "Build")
        {
            island.buildingInfoPresent = false;
            island.createChallengeUpgrade(building);
            island.challengePresent = true;
        }

        Destroy(GameObject.Find(this.transform.parent.parent.name));
        island.nameBuildingTouchCanvas = string.Empty;
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
