using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TouchBuilding : MonoBehaviour {

    public MinorIsland island;
    public Building building;

    void OnMouseDown()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        building = GameObject.Find(island.nameBuildingTouchCanvas).GetComponent<Building>();

        switch (this.name)
        {
            case "Upgrade":
                Destroy(GameObject.Find(this.transform.parent.parent.name));
                Canvas upgradeBuildingWindowCanvasPrefab = Resources.Load<Canvas>("Prefab/UpgradeBuildingWindowCanvas");
                Canvas upgradeBuildingWindowCanvas = Instantiate(upgradeBuildingWindowCanvasPrefab);
                upgradeBuildingWindowCanvas.name = "UpgradeBuildingWindowCanvas_" + building.name;
                upgradeBuildingWindowCanvas.transform.SetParent(this.transform.parent.parent.parent);  //parent : sous_ile
                upgradeBuildingWindowCanvas.transform.position = island.transform.position;
                //rotation of image according to the place of the island
                char id = island.nameMinorIsland[island.nameMinorIsland.Length - 1];
                if (id == '1' || id == '2')
                    upgradeBuildingWindowCanvas.transform.Rotate(Vector3.forward * 180);
                //modification of the content of the different Text Children of the Canvas
                foreach (Text textInCanvas in upgradeBuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<Text>())
                {
                    switch (textInCanvas.name)
                    {
                        case "Name":
                            textInCanvas.text = "Amélioration" + "";                            //TODO !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            break;
                        //write in a script functions which return these values (long switch)
                        case "CostValue":
                            textInCanvas.text = "5";                                            //linked to the level of upgrade --> TODO
                            break;
                    }
                }
                //modification of the background of the different Image Children of the Canvas
                foreach (Image imageInCanvas in upgradeBuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<Image>())
                {
                    switch (imageInCanvas.name)
                    {
                        case "CostImage":
                            //imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + getResourceConsumedFromBuilding(island.buildingClicked) + "Icon");
                            break;
                        //mêmes images
                        case "ProductionImage":
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + island.getNameResourceOrStatProduced(building.name.Split('_')[3]) + "Icon");
                            break;
                        case "ProductionImage2":
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + island.getNameResourceOrStatProduced(building.name.Split('_')[3]) + "Icon");
                            break;
                    }
                }

                break;
            case "Remove":
                //TODO
                break;
            case "Move":
                Destroy(GameObject.Find(this.transform.parent.parent.name));
                StartCoroutine(island.destroyPopup(island.createPopup("Appuyez sur l'endroit où placer le batiment"), 3));
                island.moveBuilding = true;
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
