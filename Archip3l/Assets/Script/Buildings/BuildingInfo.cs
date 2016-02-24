using UnityEngine;
using System.Collections;

public class BuildingInfo : MonoBehaviour {
    
    void OnMouseDown()
    {
        Debug.Log(this.name);
        MinorIsland island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        if (this.name == "Build")
        {
            Destroy(GameObject.Find("WheelCanvas_" + this.transform.parent.parent.parent.name));
            island.wheelPresent = false;
            island.createChallenge();
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
