using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainWindow : MonoBehaviour
{

    private List<MinorIsland> islands;

    // Use this for initialization
    void Start()
    {
        islands = new List<MinorIsland>
            {
                MinorIsland.CreateComponent(GameObject.Find("sous_ile_1"), 1),
                MinorIsland.CreateComponent(GameObject.Find("sous_ile_2"), 2),
                MinorIsland.CreateComponent(GameObject.Find("sous_ile_3"), 3),
                MinorIsland.CreateComponent(GameObject.Find("sous_ile_4"), 4)
            };
        
    }

    void OnMouseDown()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mousePositionConverted = Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log("Position : x = " + mousePositionConverted.x.ToString() + " et y = " + mousePositionConverted.y.ToString());
        RessourceManager rm = new RessourceManager();
        rm.giveRessource("or", islands[1], 50);
        StartCoroutine(islands[1].createBuilding("scierie", mousePositionConverted.x, mousePositionConverted.y, "sous_ile_1"));
        StartCoroutine(wait(20));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Island getIsland(int id)
    {
        return islands[id];
    }
    

    //function for tests with await
    private void toto()
    {
        //await Task.Delay(TimeSpan.FromSeconds(12));
        StartCoroutine(wait(20));
    }

    IEnumerator wait(int sec)
    {
        yield return new WaitForSeconds(sec);
        islands[1].getBuilding("scierie").state = 2;    //remove scierie
        //Debug.Log("batiment supprimé !");
    }

}
