using UnityEngine;
using System.Collections;

public class WheelIcon : MonoBehaviour {    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        string buildingName = this.name.Split('_')[1];
        Debug.Log(buildingName);

        MinorIsland island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        if (island.buildingInfoPresent == false)        //if any buildingInfo is open (not more than one at the same time)
        {
            Canvas buildingInfoPrefab = Resources.Load<Canvas>("Prefab/BuildingInfoWindowCanvas");
            Canvas buildingInfo = Instantiate(buildingInfoPrefab);
            buildingInfo.name = "BuildingInfo_" + this.name;
            buildingInfo.transform.SetParent(this.transform.parent.parent.parent);  //parent : minorIsland
            buildingInfo.transform.position = island.transform.position;
            //rotation of image according to the place of the island
            char id = island.nameMinorIsland[island.nameMinorIsland.Length - 1];
            if (id == '1' || id == '2')
                buildingInfo.transform.Rotate(Vector3.forward * 180);

            island.buildingInfoPresent = true;
        }

    }

    

    
}
