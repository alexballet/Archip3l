using UnityEngine;
using System.Collections;

public class titi
{

    // Use this for initialization
    void Start()
    {
        
    }

    public void tata()
    {
        //StartCoroutine(wait(5));
    }

    IEnumerator wait(int sec)
    {
        yield return new WaitForSeconds(sec);
        Debug.Log("2");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        Debug.Log("1");
        tata();
    }
}
