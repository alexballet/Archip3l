using UnityEngine;
using System.Collections;

public class Notification : MonoBehaviour {

    void OnMouseDown()
    {
        Debug.Log("Clic on " + this.name);
        StartCoroutine(main.removeNotification(this.gameObject));
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
