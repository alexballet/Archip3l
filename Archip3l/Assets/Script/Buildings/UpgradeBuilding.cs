using UnityEngine;
using System.Collections;

public class UpgradeBuilding : MonoBehaviour {

    public MinorIsland island;
    public Building building;
    
    void OnMouseDown()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        building = GameObject.Find(island.nameBuildingTouchCanvas).GetComponent<Building>();

        if (this.name == "Upgrade")
        {
            if (building.level < 3)
            {

                island.buildingInfoPresent = false;
                island.createChallengeUpgrade(building);
                island.challengePresent = true;
            }
            else
            {
                StartCoroutine(island.destroyPopup(island.createPopup("Ce bâtiment est déjà au niveau maximal !"), 3));
            }
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
