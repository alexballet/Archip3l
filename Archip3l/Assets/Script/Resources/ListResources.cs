using UnityEngine;
using System.Collections;

public class ListResources : MonoBehaviour {

    void OnMouseDown()
    {
        Debug.Log(this.name);
        ExchangeResource.resource = this.name;
        Destroy(GameObject.Find(this.transform.parent.name));
        ExchangeResource.otherWindowOpen = false;
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
