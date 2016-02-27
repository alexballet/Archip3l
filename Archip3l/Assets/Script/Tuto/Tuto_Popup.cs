using UnityEngine;
using System.Collections;

public class Tuto_Popup : MonoBehaviour {

    public string namePopupCanvas;
    public Tuto_MinorIsland island;

    void OnMouseDown()
    {
        StartCoroutine(island.destroyPopup(namePopupCanvas, 1));
    }
}
