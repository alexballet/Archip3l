using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

    public string namePopupCanvas;
    public MinorIsland island;

    void OnMouseDown()
    {
        StartCoroutine(island.destroyPopup(namePopupCanvas, 1));
    }
}
