using UnityEngine;
using System.Collections;

public class TouchBuilding : MonoBehaviour {

    public MinorIsland island;

    void OnMouseDown()
    {
        switch (this.name)
        {
            case "Upgrade":
                //TODO
                break;
            case "Remove":
                //TODO
                break;
            case "Move":
                Destroy(GameObject.Find(this.transform.parent.parent.name));
                StartCoroutine(island.destroyPopup(island.createPopup("Appuyez sur l'endroit où placer le batiment"), 5));
                island.moveBuilding = true;
                break;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
