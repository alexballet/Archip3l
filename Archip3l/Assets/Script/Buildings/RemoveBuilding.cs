using UnityEngine;
using System.Collections;

public class RemoveBuilding : MonoBehaviour {

    public MinorIsland island;
    public Building building;

    void OnMouseDown()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        building = GameObject.Find(island.nameBuildingTouchCanvas).GetComponent<Building>();

        if (this.name == "Remove")
        {

            TypeBuilding typeBuilding = building.TypeBuilding;
            StartCoroutine(building.minorIsland.buildingManager.destroyBuilding(building.TypeBuilding));
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
