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

        initializeRessources();
        
    }

    void OnMouseDown()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 mousePositionConverted = Camera.main.ScreenToWorldPoint(mousePosition);
        Debug.Log("Position : x = " + mousePositionConverted.x.ToString() + " et y = " + mousePositionConverted.y.ToString());
        int islandTouched = getIslandTouched(mousePositionConverted.x, mousePositionConverted.y);
        Debug.Log("islandTouched : " + islandTouched.ToString());
        StartCoroutine(getIsland(islandTouched).createBuilding("scierie", mousePositionConverted.x, mousePositionConverted.y, "sous_ile_" + islandTouched.ToString()));
        StartCoroutine(wait(20));
    }

    private int getIslandTouched(float x, float y)
    {
        if (x < 0 && y > 0)
            return 1;
        if (x < 0 && y < 0)
            return 2;
        if (x > 0 && y > 0)
            return 3;
        return 4;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Island getIsland(int id)
    {
        return islands[id - 1];
    }

    private void initializeRessources()     //to be completed
    {
        RessourceManager rm = new RessourceManager();
        Island island;
        for (int i = 1; i <= 4; i++)
        {
            island = getIsland(i);
            rm.giveRessource("or", island, 50);
        }
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
