using UnityEngine;
using System.Collections;

public class ListIslands : MonoBehaviour {

    public MinorIsland island;

    void OnMouseDown()
    {
        if (this.name != "Close")
        {
            Debug.Log(this.name);
            island.islandToSend = this.name;
        }
        Destroy(GameObject.Find(this.transform.parent.name));
        island.otherWindowOpen = false;
    }


    // Use this for initialization
    void Start () {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
