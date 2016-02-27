using UnityEngine;
using System.Collections;

public class Tuto_UpgradeBuilding : MonoBehaviour {

    public Tuto_MinorIsland island;
    public Tuto_Building building;
    
    void OnMouseDown()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<Tuto_MinorIsland>();
        building = GameObject.Find(island.nameBuildingTouchCanvas).GetComponent<Tuto_Building>();

        if (this.name == "Upgrade")
        {
            island.buildingInfoPresent = false;
            island.createTuto_ChallengeUpgrade(building);
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
