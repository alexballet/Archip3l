﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class MinorIsland : MonoBehaviour {

    public BuildingManager buildingManager { get; private set; }
    public ResourceManager resourceManager { get; private set; }

    public Transform buildingManagerPrefab;
    public Transform resourceManagerPrefab;

    public string nameMinorIsland;

    //communication with WheelIcon, BuildingInfo & ChallengeBuild scripts + Popups & TouchBuilding
    public Vector2 placeToConstruct;
    public bool wheelPresent = false;                   //wheel present on the island
    public bool buildingInfoPresent = false;            //buildingInfo present on the island
    public bool challengePresent = false;               //challenge present on the island
    public bool moveBuilding = false;                   //moving a building
    public string nameBuildingTouchCanvas;
    public string buildingClicked;
    public int nbPopupPresent;



    void Awake()
    {
        nbPopupPresent = 0;
        
        var buildingManagerTransform = Instantiate(buildingManagerPrefab) as Transform;
        BuildingManager buildingManager = buildingManagerTransform.GetComponent<BuildingManager>();
        if (buildingManager != null)
        {
            buildingManager.init(this);
            buildingManager.transform.SetParent(this.transform);
            this.buildingManager = buildingManager;
        }
        
        var resourceManagerTransform = Instantiate(resourceManagerPrefab) as Transform;
        ResourceManager resourceManager = resourceManagerTransform.GetComponent<ResourceManager>();
        if (resourceManager != null)
        {
            resourceManager.init(this);
            resourceManager.transform.SetParent(this.transform);
            this.resourceManager = resourceManager;
        }
        
        /*----------TEST--------*/
        

        //if(nameMinorIsland == "sous_ile_3")
        //{
        //    this.resourceManager.addResource(TypeResource.Gold, "Or", 10, 5);
        //}

        /*------------------*/
    }

    public void createChallengeBuild(string buildingClicked)
    {

        GameObject.Find(nameMinorIsland).GetComponent<PolygonCollider2D>().enabled = false;
        ChallengeBuild challengeBuild = GameObject.Find("Virtual_" + nameMinorIsland).AddComponent<ChallengeBuild>();

        //random type of ChallengeBuild
        TypeChallenge type;
        System.Random ran = new System.Random();
        int aleat = ran.Next(0, 2);
        if (aleat == 0)
            type = TypeChallenge.VraiFaux;
        else
            type = TypeChallenge.QCM;

        challengeBuild.init(type, this, (TypeBuilding)System.Enum.Parse(typeof(TypeBuilding), buildingClicked));      //TODO : adapt challenge to TypeBuilding

        GameObject.Find(nameMinorIsland).GetComponent<PolygonCollider2D>().enabled = true;
    }

    public void createChallengeUpgrade(Building building)
    {

        GameObject.Find(nameMinorIsland).GetComponent<PolygonCollider2D>().enabled = false;
        ChallengeUpgrade challengeUpgrade = GameObject.Find("Virtual_" + nameMinorIsland).AddComponent<ChallengeUpgrade>();

        //random type of ChallengeUpgrade
        TypeChallenge type;
        System.Random ran = new System.Random();
        int aleat = ran.Next(0, 2);
        if (aleat == 0)
            type = TypeChallenge.VraiFaux;
        else
            type = TypeChallenge.QCM;

        challengeUpgrade.init(type, this, building);             //TODO : adapt challenge to TypeBuilding

        GameObject.Find(nameMinorIsland).GetComponent<PolygonCollider2D>().enabled = true;
    }


    //returns the name of the Popup (GameObject) created
    public string createPopup(string popupText)
    {
        //TODO : gérer superposition des textes des Popup

        Canvas popupCanvasPrefab = Resources.Load<Canvas>("Prefab/PopupCanvas");
        Canvas popupCanvas = Instantiate(popupCanvasPrefab);
        this.nbPopupPresent++;
        popupCanvas.name = "PopupCanvas" + nbPopupPresent.ToString() + "_" + this.nameMinorIsland;
        popupCanvas.transform.SetParent(GameObject.Find(this.nameMinorIsland).transform);
        Vector3 vector3 = GameObject.Find(this.nameMinorIsland).transform.position;
        vector3.z = (-1) * nbPopupPresent;
        popupCanvas.transform.position = vector3;
        //popupCanvas.transform.position = GameObject.Find(this.nameMinorIsland).transform.position;
        //rotation of image according to the place of the island
        char id = this.nameMinorIsland[this.nameMinorIsland.Length - 1];
        if (id == '1' || id == '2')
            popupCanvas.transform.Rotate(Vector3.forward * 180);

        popupCanvas.GetComponentInChildren<Text>().text = popupText;

        //name + island passed to get the Canvas to destroy
        popupCanvas.GetComponentInChildren<Popup>().namePopupCanvas = popupCanvas.name;
        popupCanvas.GetComponentInChildren<Popup>().island = this;

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

    public void createBuildingTouch(Building building)
    {
        this.nameBuildingTouchCanvas = building.name;

        Canvas touchBuildingCanvasPrefab = Resources.Load<Canvas>("Prefab/touchBuildingCanvas");
        Canvas touchBuildingCanvas = Instantiate(touchBuildingCanvasPrefab);
        touchBuildingCanvas.transform.SetParent(this.transform);
        touchBuildingCanvas.name = "touchBuilding_" + this.nameBuildingTouchCanvas;
        touchBuildingCanvas.transform.position = GameObject.Find(this.nameBuildingTouchCanvas).transform.position;

        foreach(TouchBuilding touchBuilding in touchBuildingCanvas.GetComponentsInChildren<TouchBuilding>())
        {
            touchBuilding.island = this;
            touchBuilding.building = building;
        }
        //touchBuildingCanvas.GetComponent<TouchBuilding>().island = this;

        //rotation of image according to the place of the island
        char id = this.nameMinorIsland[this.nameMinorIsland.Length - 1];
        if (id == '1' || id == '2')
            touchBuildingCanvas.transform.Rotate(Vector3.forward * 180);
        
    }



    // Update is called once per frame
    void Update () {

        /*for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("toto " + i.ToString());
            }
        }*/
	}


    void OnMouseDown()
    {
        //Debug.Log(Input.mousePosition.ToString());

        //moving a building
        if (moveBuilding)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = -1;
            GameObject.Find(this.nameBuildingTouchCanvas).transform.position = pos;
            this.moveBuilding = false;
            this.nameBuildingTouchCanvas = string.Empty;
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
                if (!challengePresent)      //if any challenge is open on the island
                {
                    if (!wheelPresent)  //if the wheel is not on the island
                    {
                        this.placeToConstruct = Input.mousePosition;

                        //Wheel appearance
                        Canvas prefabWheelCanvas = Resources.Load<Canvas>("Prefab/WheelCanvas");
                        Canvas wheelCanvas = Instantiate(prefabWheelCanvas);
                        wheelCanvas.name = "WheelCanvas_" + nameMinorIsland;
                        //parent : island
                        wheelCanvas.transform.SetParent(GameObject.Find(nameMinorIsland).transform);
                        SpriteRenderer wheelImage = wheelCanvas.GetComponentInChildren<SpriteRenderer>();
                        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mousePosition.z = 0;
                        //position of wheel where it was clicked on
                        wheelImage.transform.position = mousePosition;
                        //rotation of image according to the place of the island
                        char id = this.nameMinorIsland[this.nameMinorIsland.Length - 1];
                        if (id == '1' || id == '2')
                            wheelImage.transform.Rotate(Vector3.forward * 180);

                        //disable specific buildings
                        List<string> list = getDisabledBuildings(this.nameMinorIsland);
                        foreach (SpriteRenderer sr in wheelImage.GetComponentsInChildren<SpriteRenderer>())
                        {
                            //Debug.Log(sr.name);
                            if (list.Contains(sr.name)) {    
                                sr.sprite = Resources.Load<Sprite>("Building/Icons/" + sr.name + "_disabled");
                                sr.GetComponent<PolygonCollider2D>().enabled = false;
                            }
                        }

                        this.wheelPresent = true;
                    }
                    else
                    {
                        if (!buildingInfoPresent)       //if the wheel is on the island, but not the buildingInfo
                        {
                            //destruction of the wheel if clic somewhere else in the island
                            Destroy(GameObject.Find("WheelCanvas_" + nameMinorIsland));
                            this.wheelPresent = false;
                        }
                    }
                }
            }
        }

        //pas ici ! le transfert se fait suite à un appui long sur l'ile ^^
        
        //Transfert test
        /*MinorIsland remote = GameObject.Find("sous_ile_3").GetComponent<MinorIsland>();
        if (this.resourceManager.donateResource(remote, TypeResource.Gold, 10))
        {
            Debug.Log("Transfer Resource " + TypeResource.Gold.ToString() + " from " + this.nameMinorIsland + " to " + remote.nameMinorIsland);
        }
        else
        {
            Debug.Log("Impossible to tranfert " + TypeResource.Gold.ToString() + " from " + this.nameMinorIsland + " to " + remote.nameMinorIsland);
        }

        */
        //Bulding creation test
        //this.buildingManager.createBuilding(TypeBuilding.GoldMine, Input.mousePosition);
    }

    //translation of the building's name to french
    public string translateBuildingName(string buildingName)
    {
        switch (buildingName)
        {
            case "GoldMine":
                return "Mine d'or";
            case "StoneMine":
                return "Mine de pierre";
            case "OilPlant":
                return "Puit de pétrole";
            case "Sawmill":
                return "Scierie";
            case "Factory":
                return "Usine";
            case "WindTurbine":
                return "Eolienne";
            case "Farm":
                return "Ferme";
            case "Lab":
                return "Laboratoire";
            case "Airport":
                return "Aéroport";
            case "Hotel":
                return "Hôtel";
            case "Harbor":
                return "Port";
            case "School":
                return "Ecole";
            case "Church":
                return "Eglise";
            case "Cinema":
                return "Cinéma";
            case "AmusementPark":
                return "Parc d'attraction";
            case "PowerPlant":
                return "Centrale électrique"; 
            default:
                return string.Empty;
        }
    }

    

    private List<string> getDisabledBuildings(string nameMinorIsland)
    {
        List<string> list = new List<string>();
        switch (nameMinorIsland)
        {
            case "sous_ile_1":
                list.Add("toto");
                break;
            case "sous_ile_2":
                list.Add("toto");
                break;
            case "sous_ile_3":
                list.Add("toto");
                break;
            case "sous_ile_4":
                list.Add("toto");
                break;
        }
        return list;
    }
}
