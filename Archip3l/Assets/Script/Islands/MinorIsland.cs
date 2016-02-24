using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MinorIsland : MonoBehaviour {

    public BuildingManager buildingManager { get; private set; }
    public ResourceManager resourceManager { get; private set; }

    public Transform buildingManagerPrefab;
    public Transform resourceManagerPrefab;

    public string nameMinorIsland;

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


        if (nameMinorIsland == "sous_ile_4")
            createChallenge();

        //if(nameMinorIsland == "sous_ile_3")
        //{
        //    this.resourceManager.addResource(TypeResource.Gold, "Or", 10, 5);
        //}

        /*------------------*/
    }
    public void createChallenge()
    {
        //avoid creation on wheel by clicking on Island

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

        GameObject.Find(nameMinorIsland).GetComponent<PolygonCollider2D>().enabled = false;
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
        Debug.Log(Input.mousePosition.ToString());

        //Wheel appearance
        //if (GameObject.Find("WheelCanvas_" + nameMinorIsland) != null)
        //{
        //    Debug.Log("already a wheelCanvas");
        //}
        //else
        //{
        //    Canvas prefabWheelCanvas = Resources.Load<Canvas>("Prefab/WheelCanvas");
        //    Canvas wheelCanvas = Instantiate(prefabWheelCanvas);
        //    wheelCanvas.name = "WheelCanvas_" + nameMinorIsland;
        //    wheelCanvas.transform.SetParent(GameObject.Find(nameMinorIsland).transform);
        //    SpriteRenderer wheelImage = wheelCanvas.GetComponentInChildren<SpriteRenderer>();
        //    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    mousePosition.z = 0;
        //    wheelImage.transform.position = mousePosition;
        //}
        
        this.buildingManager.createBuilding(TypeBuilding.GoldMine, Input.mousePosition);
    }
}
