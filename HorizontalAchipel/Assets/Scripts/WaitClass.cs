using UnityEngine;
using System.Collections;

public class WaitClass : MonoBehaviour {

    public bool finished = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void waitFunction(int sec)
    {
        StartCoroutine(wait(sec));
    }

    IEnumerator wait(int sec)
    {
        yield return new WaitForSeconds(sec);
        finished = true;
        Debug.Log("3");
    }
}
