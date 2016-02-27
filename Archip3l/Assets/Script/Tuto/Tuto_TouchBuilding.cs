using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tuto_TouchBuilding : MonoBehaviour {

    public Tuto_MinorIsland island;
    public Tuto_Building building;

    void OnMouseDown()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<Tuto_MinorIsland>();
        building = GameObject.Find(island.nameTuto_MinorIsland + "_Harbor").GetComponent<Tuto_Building>();
        if (building == null)
            Debug.Log("toto");

        switch (this.name)
        {
            case "Upgrade":
                if (island.harborMoved)
                {
                    Destroy(GameObject.Find(this.transform.parent.parent.name));
                    Canvas upgradeTuto_BuildingWindowCanvasPrefab = Resources.Load<Canvas>("Prefab/Tuto/UpgradeBuildingWindowCanvasTuto");
                    Canvas upgradeTuto_BuildingWindowCanvas = Instantiate(upgradeTuto_BuildingWindowCanvasPrefab);
                    upgradeTuto_BuildingWindowCanvas.name = "UpgradeBuildingWindowCanvas_" + building.name;
                    upgradeTuto_BuildingWindowCanvas.transform.SetParent(this.transform.parent.parent.parent);  //parent : sous_ile
                    upgradeTuto_BuildingWindowCanvas.transform.position = island.transform.position;
                    //rotation of image according to the place of the island
                    char id = island.nameTuto_MinorIsland[island.nameTuto_MinorIsland.Length - 1];
                    if (id == '1' || id == '2')
                        upgradeTuto_BuildingWindowCanvas.transform.Rotate(Vector3.forward * 180);
                    //modification of the content of the different Text Children of the Canvas
                    foreach (Text textInCanvas in upgradeTuto_BuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<Text>())
                    {
                        switch (textInCanvas.name)
                        {
                            case "Name":
                                textInCanvas.text = "Amélioration 1";
                                break;
                            case "CostValue1":
                                textInCanvas.text = "0";
                                break;
                            case "CostValue2":
                                textInCanvas.text = "0";
                                break;
                        }
                    }
                    //modification of the background of the different Image Children of the Canvas
                    foreach (Image imageInCanvas in upgradeTuto_BuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<Image>())
                    {
                        switch (imageInCanvas.name)
                        {
                            case "CostImage1":
                                imageInCanvas.sprite = null;
                                break;
                            case "CostImage2":
                                imageInCanvas.sprite = null;
                                break;
                            //mêmes images
                            case "ProductionImage":
                                imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/FoodIcon");
                                break;
                            case "ProductionImage2":
                                imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/FoodIcon");
                                break;
                        }
                    }
                }
                else
                {
                    StartCoroutine(island.destroyPopup(island.createPopup("Commencez par déplacer le port."), 5));
                }
                break;
            case "Remove":
                if (island.harborUpgraded)
                {
                    StartCoroutine(building.tuto_minorIsland.tuto_buildingManager.destroyBuilding());
                    Destroy(GameObject.Find("touchBuilding_" + this.island.nameTuto_MinorIsland + "_Harbor"));
                }
                else
                {
                    if(island.harborMoved)
                        StartCoroutine(island.destroyPopup(island.createPopup("Maintenant, améliorez le port."), 5));
                    else
                        StartCoroutine(island.destroyPopup(island.createPopup("Commencez par déplacer le port."), 5));
                }
                break;
            case "Move":
                if (!island.harborMoved)
                {
                    Destroy(GameObject.Find(this.transform.parent.parent.name));
                    StartCoroutine(island.destroyPopup(island.createPopup("Appuyez sur l'endroit où placer le batiment"), 3));
                    island.moveBuilding = true;
                }
                else
                {
                    if (island.harborUpgraded)
                        StartCoroutine(island.destroyPopup(island.createPopup("Pour finir le tutoriel, supprimez le port."), 5));
                    else
                        StartCoroutine(island.destroyPopup(island.createPopup("Maintenant, améliorez le port."), 5));
                }
                break;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
