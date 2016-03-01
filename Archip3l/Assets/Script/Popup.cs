using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

    public string namePopupCanvas;
    public MinorIsland island;

    void OnMouseDown()
    {
        string name = this.gameObject.transform.parent.name;
        this.namePopupCanvas = name;
        string[] nameSplitted = name.Split('_');
        this.island = GameObject.Find(nameSplitted[1] + "_" + nameSplitted[2] + "_" + nameSplitted[3]).GetComponent<MinorIsland>();
        StartCoroutine(island.destroyPopup(namePopupCanvas, 0));
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
