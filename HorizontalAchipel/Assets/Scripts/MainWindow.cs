using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainWindow : MonoBehaviour {

    private List<Island> islands;

    // Use this for initialization
    void Start () {
        islands = new List<Island>
            {
                new MajorIsland(0),
                new MinorIsland(1),
                new MinorIsland(2),
                new MinorIsland(3),
                new MinorIsland(4)
            };

        /*----------- Tests -------------*/
        RessourceManager rm = new RessourceManager();
        rm.giveRessource("or", islands[1], 50);
        islands[1].createBuilding("scierie", 200, 100, CanIsl1);    //200 & 100 got by position of touch event
        toto();


        /*-------------------------------*/
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
    private async void toto()
    {
        await Task.Delay(TimeSpan.FromSeconds(12));
        islands[1].getBuilding("scierie").state = 2;    //remove scierie
    }


}
