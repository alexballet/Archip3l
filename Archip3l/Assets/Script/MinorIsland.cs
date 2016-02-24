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

        /*resourceManager = GameObject.Find(nameMinorIsland).AddComponent<ResourceManager>();
        resourceManager.init(this);*/

        /*----------TEST--------*/

        createChallenge();

        /*------------------*/
    }

    public void createChallenge()
    {
        /*----------TEST--------*/
        if (nameMinorIsland == "sous_ile_1")
        {
            Challenge challenge = GameObject.Find("Virtual_" + nameMinorIsland).AddComponent<Challenge>();
            challenge.init(TypeChallenge.QCM, this);
        }
        if (nameMinorIsland == "sous_ile_4")
        {
            Challenge challenge = GameObject.Find("Virtual_" + nameMinorIsland).AddComponent<Challenge>();
            challenge.init(TypeChallenge.VraiFaux, this);
        }
        /*------------------*/
    }


    // Use this for initialization
    void Start () {
	    
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

        if (GameObject.Find("WheelCanvas_" + nameMinorIsland) != null)
        {
            Debug.Log("already a wheelCanvas");
        }
        else
        {
            Canvas prefabWheelCanvas = Resources.Load<Canvas>("Prefab/WheelCanvas");
            Canvas wheelCanvas = Instantiate(prefabWheelCanvas);
            wheelCanvas.name = "WheelCanvas_" + nameMinorIsland;
            wheelCanvas.transform.SetParent(GameObject.Find(nameMinorIsland).transform);
            SpriteRenderer wheelImage = wheelCanvas.GetComponentInChildren<SpriteRenderer>();
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            wheelImage.transform.position = mousePosition;
        }
        


        //this.buildingManager.createBuilding(TypeBuilding.Mine, Input.mousePosition);
    }
}
