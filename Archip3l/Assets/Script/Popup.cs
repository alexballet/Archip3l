using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

    public string namePopupCanvas;
    public MinorIsland island;

    void OnMouseDown()
    {
        Debug.Log(namePopupCanvas + " +++ " + island.nameMinorIsland);
        StartCoroutine(island.destroyPopup(namePopupCanvas, 1));
        //Destroy(GameObject.Find(namePopupCanvas));
    }
}
