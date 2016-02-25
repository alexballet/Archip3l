using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MinorIsland : MonoBehaviour {

    public BuildingManager buildingManager { get; private set; }
    public ResourceManager resourceManager { get; private set; }

    public Transform buildingManagerPrefab;
    public Transform resourceManagerPrefab;

    public string nameMinorIsland;

    //communication with WheelIcon, BuildingInfo & Challenge scripts
    public Vector2 placeToConstruct;
    public bool wheelPresent = false;           //wheel present on the island
    public bool buildingInfoPresent = false;    //buildingInfo present on the island
    public bool challengePresent = false;       //challenge present on the island
    public string buildingClicked;

    void Awake()
    {
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


        //if (nameMinorIsland == "sous_ile_4")
        //    createChallenge();

        //if(nameMinorIsland == "sous_ile_3")
        //{
        //    this.resourceManager.addResource(TypeResource.Gold, "Or", 10, 5);
        //}

        /*------------------*/
    }
    public void createChallenge()
    {

        GameObject.Find(nameMinorIsland).GetComponent<PolygonCollider2D>().enabled = false;
        Challenge challenge = GameObject.Find("Virtual_" + nameMinorIsland).AddComponent<Challenge>();

        //random type of Challenge
        TypeChallenge type;
        System.Random ran = new System.Random();
        int aleat = ran.Next(0, 2);
        if (aleat == 0)
            type = TypeChallenge.VraiFaux;
        else
            type = TypeChallenge.QCM;

        challenge.init(type, this, TypeBuilding.GoldMine);      //pb with TypeStat

        GameObject.Find(nameMinorIsland).GetComponent<PolygonCollider2D>().enabled = true;
    }

	// Update is called once per frame
	void Update () {

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("toto " + i.ToString());
            }
        }
	}
    void OnMouseDown()
    {

        //Debug.Log(Input.mousePosition.ToString());

        if (!challengePresent)  //if any challenge is open on the island
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

                wheelPresent = true;
            }
            else
            {
                if (!buildingInfoPresent)       //if the wheel is on the island, but not he buildingInfo
                {
                    //destruction of the wheel if clic somewhere else in the island
                    Destroy(GameObject.Find("WheelCanvas_" + nameMinorIsland));
                    this.wheelPresent = false;
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
            default:
                return string.Empty;
        }
    }
}
