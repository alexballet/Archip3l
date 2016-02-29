using UnityEngine;
using System.Collections;


//this class concerns the thophies + the medals + AirportMedal
public class Trophy : MonoBehaviour {

    public bool active = false;     //true if trophy unlocked

    void OnMouseDown()
    {
        Debug.Log("Clic on " + this.name);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
