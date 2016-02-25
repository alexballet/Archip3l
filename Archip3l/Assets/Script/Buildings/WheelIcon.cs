using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WheelIcon : MonoBehaviour {    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        MinorIsland island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();

        island.buildingClicked = this.name.Split('_')[1];

        if (island.buildingInfoPresent == false)        //if any buildingInfo is open (not more than one at the same time)
        {
            Canvas buildingInfoPrefab = Resources.Load<Canvas>("Prefab/BuildingInfoWindowCanvas");
            Canvas buildingInfo = Instantiate(buildingInfoPrefab);
            buildingInfo.name = "BuildingInfo_" + this.name;
            buildingInfo.transform.SetParent(this.transform.parent.parent.parent);  //parent : minorIsland
            buildingInfo.transform.position = island.transform.position;
            //modification of the content of the different Text Children of the Canvas
            foreach(Text textInCanvas in buildingInfo.GetComponent<Canvas>().GetComponentsInChildren<Text>())
            {
                switch (textInCanvas.name)
                {
                    case "Name":
                        textInCanvas.text = island.buildingClicked;
                        break;
                }
            }

            //rotation of image according to the place of the island
            char id = island.nameMinorIsland[island.nameMinorIsland.Length - 1];
            if (id == '1' || id == '2')
                buildingInfo.transform.Rotate(Vector3.forward * 180);

            island.buildingInfoPresent = true;
        }

    }

    

    
}
