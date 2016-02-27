using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Tuto_MinorIsland : MonoBehaviour {

    public Tuto_BuildingManager tuto_buildingManager { get; private set; }

    public Transform tuto_buildingManagerPrefab;

    public string nameTuto_MinorIsland;

    //communication with WheelIcon, BuildingInfo & Tuto_ChallengeBuild scripts + Popups & TouchBuilding
    public Vector2 placeToConstruct;
    public bool wheelPresent = false;                   //wheel present on the island
    public bool buildingInfoPresent = false;            //buildingInfo present on the island
    public bool moveBuilding = false;                   //moving a building
    public string nameBuildingTouchCanvas;
    public string buildingClicked;
    public int nbPopupPresent;

    //steps of tuto
    public bool harborBuilt = false;
    public bool harborMoved = false;
    public bool harborUpgraded = false;
    public bool harborRemoved = false;


    void Awake()
    {
        nbPopupPresent = 0;
        
        var buildingManagerTransform = Instantiate(tuto_buildingManagerPrefab) as Transform;
        Tuto_BuildingManager mytuto_buildingManager = buildingManagerTransform.GetComponent<Tuto_BuildingManager>();
        if (mytuto_buildingManager != null)
        {
            mytuto_buildingManager.init(this);
            mytuto_buildingManager.transform.SetParent(this.transform);
            mytuto_buildingManager.name = "BuildingManager_" + this.nameTuto_MinorIsland;
            this.tuto_buildingManager = mytuto_buildingManager;
        }

        StartCoroutine(this.destroyPopup(this.createPopup("Appuyez n'importe où puis créez le port."), 5));

    }

    public void createTuto_ChallengeBuild(string buildingClicked)
    {
        this.tuto_buildingManager.createBuilding(this.placeToConstruct);
    }

    public void createTuto_ChallengeUpgrade(Tuto_Building tuto_building)
    {
        tuto_building.level += 1;
        tuto_building.buildState = 0;
        StartCoroutine(tuto_building.launchUpgradeAnimation());
    }


    //returns the name of the Popup (GameObject) created
    public string createPopup(string popupText)
    {

        Canvas popupCanvasPrefab = Resources.Load<Canvas>("Prefab/Tuto/PopupCanvasTuto");
        Canvas popupCanvas = Instantiate(popupCanvasPrefab);
        this.nbPopupPresent++;
        popupCanvas.name = "PopupCanvas" + nbPopupPresent.ToString() + "_" + this.nameTuto_MinorIsland;
        popupCanvas.transform.SetParent(GameObject.Find(this.nameTuto_MinorIsland).transform);
        Vector3 vector3 = GameObject.Find(this.nameTuto_MinorIsland).transform.position;
        vector3.z = (-1) * nbPopupPresent;
        popupCanvas.transform.position = vector3;
        //popupCanvas.transform.position = GameObject.Find(this.nameTuto_MinorIsland).transform.position;
        //rotation of image according to the place of the island
        char id = this.nameTuto_MinorIsland[this.nameTuto_MinorIsland.Length - 1];
        if (id == '1' || id == '2')
            popupCanvas.transform.Rotate(Vector3.forward * 180);

        popupCanvas.GetComponentInChildren<Text>().text = popupText;

        //name + island passed to get the Canvas to destroy
        popupCanvas.GetComponentInChildren<Tuto_Popup>().namePopupCanvas = popupCanvas.name;
        popupCanvas.GetComponentInChildren<Tuto_Popup>().island = this;

        return popupCanvas.name;
    }

    public IEnumerator destroyPopup(string namePopup, int timer)
    {
        SpriteRenderer popupImage = GameObject.Find(namePopup).GetComponentInChildren<SpriteRenderer>();
        
        yield return new WaitForSeconds(timer);
        Color color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);

            color = popupImage.color;
            color.a -= 0.01f;
            popupImage.color = color;

        }

        Destroy(GameObject.Find(namePopup));
        this.nbPopupPresent--;
    }


    public void createBuildingTouch(Tuto_Building building)
    {
        this.nameBuildingTouchCanvas = building.name;

        Canvas touchBuildingCanvasPrefab = Resources.Load<Canvas>("Prefab/Tuto/touchBuildingCanvasTuto");
        Canvas touchBuildingCanvas = Instantiate(touchBuildingCanvasPrefab);
        touchBuildingCanvas.transform.SetParent(this.transform);
        touchBuildingCanvas.name = "touchBuilding_" + this.nameBuildingTouchCanvas;
        touchBuildingCanvas.transform.position = GameObject.Find(this.nameBuildingTouchCanvas).transform.position;

        foreach(Tuto_TouchBuilding touchBuilding in touchBuildingCanvas.GetComponentsInChildren<Tuto_TouchBuilding>())
        {
            touchBuilding.island = this;
            touchBuilding.building = building;
        }
        //touchBuildingCanvas.GetComponent<TouchBuilding>().island = this;

        //rotation of image according to the place of the island
        char id = this.nameTuto_MinorIsland[this.nameTuto_MinorIsland.Length - 1];
        if (id == '1' || id == '2')
            touchBuildingCanvas.transform.Rotate(Vector3.forward * 180);
        
    }



    // Update is called once per frame
    void Update () {

        //when all islands have finished the tuto, change scene

        if (GameObject.Find("sous_ile_1").GetComponent<Tuto_MinorIsland>().harborRemoved &&
            GameObject.Find("sous_ile_2").GetComponent<Tuto_MinorIsland>().harborRemoved &&
            GameObject.Find("sous_ile_3").GetComponent<Tuto_MinorIsland>().harborRemoved &&
            GameObject.Find("sous_ile_4").GetComponent<Tuto_MinorIsland>().harborRemoved)
                Application.LoadLevel("playingScene");
    }


    void OnMouseDown()
    {

        //moving a building
        if (moveBuilding)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -1;
            GameObject.Find(this.nameBuildingTouchCanvas).transform.position = pos;
            this.moveBuilding = false;
            this.nameBuildingTouchCanvas = string.Empty;
            this.harborMoved = true;
            StartCoroutine(this.destroyPopup(this.createPopup("Maintenant, améliorez le port."), 5));
        }
        else
        {
            if (this.nameBuildingTouchCanvas != String.Empty)
            {
                Destroy(GameObject.Find("touchBuilding_" + this.nameBuildingTouchCanvas));
                this.nameBuildingTouchCanvas = String.Empty;
            }
            else
            {
                if (!wheelPresent && !harborBuilt)  //if the wheel is not on the island
                {
                    this.placeToConstruct = Input.mousePosition;

                    //Wheel appearance
                    Canvas prefabWheelCanvas = Resources.Load<Canvas>("Prefab/Tuto/WheelCanvasTuto");
                    Canvas wheelCanvas = Instantiate(prefabWheelCanvas);
                    wheelCanvas.name = "WheelCanvas_" + nameTuto_MinorIsland;
                    //parent : island
                    wheelCanvas.transform.SetParent(GameObject.Find(nameTuto_MinorIsland).transform);
                    SpriteRenderer wheelImage = wheelCanvas.GetComponentInChildren<SpriteRenderer>();
                    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePosition.z = 0;
                    //position of wheel where it was clicked on
                    wheelImage.transform.position = mousePosition;
                    //rotation of image according to the place of the island
                    char id = this.nameTuto_MinorIsland[this.nameTuto_MinorIsland.Length - 1];
                    if (id == '1' || id == '2')
                        wheelImage.transform.Rotate(Vector3.forward * 180);

                    //disable specific buildings
                    foreach (SpriteRenderer sr in wheelImage.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (sr.name != "wheelIcon_Harbor") {    
                            sr.sprite = Resources.Load<Sprite>("Building/Icons_Disabled/" + sr.name + "_disabled");
                        }
                    }

                    this.wheelPresent = true;
                }
                else
                {
                    if (!buildingInfoPresent)       //if the wheel is on the island, but not the buildingInfo
                    {
                        //destruction of the wheel if clic somewhere else in the island
                        Destroy(GameObject.Find("WheelCanvas_" + nameTuto_MinorIsland));
                        this.wheelPresent = false;
                    }
                }
            }
        }
        
    }
    
}
