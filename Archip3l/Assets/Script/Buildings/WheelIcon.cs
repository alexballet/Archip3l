using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WheelIcon : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    void OnMouseDown()
    {
        MinorIsland island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();

        island.buildingClicked = this.name.Split('_')[1];

        if (island.buildingInfoPresent == false)        //if any buildingInfo is open (not more than one at the same time)
        {
            Canvas buildingInfoPrefab = Resources.Load<Canvas>("Prefab/BuildingInfoWindowCanvas");
            Canvas buildingInfo = Instantiate(buildingInfoPrefab);

            buildingInfo.name = "BuildingInfo_" + this.name;
            buildingInfo.transform.SetParent(this.transform.parent.parent.parent);  //parent : minorIsland
            buildingInfo.transform.position = island.transform.position;
            //modification of the content of the different Text Children of the Canvas

            List<Tuple<TypeResource, int>> constructionResourceNeeded = Building.getConstructionResourcesNeeded(island.buildingClicked);

            foreach (Text textInCanvas in buildingInfo.GetComponent<Canvas>().GetComponentsInChildren<Text>())
            {
                switch (textInCanvas.name)
                {
                    case "Name":
                        textInCanvas.text = island.translateBuildingName(island.buildingClicked);
                        break;
                    case "CostValue1":
                        textInCanvas.text = constructionResourceNeeded[0].Second.ToString();
                        break;
                    case "CostValue2":
                        if (constructionResourceNeeded[1] != null)
                            textInCanvas.text = constructionResourceNeeded[1].Second.ToString();
                        else
                            textInCanvas.text = "-";
                        break;
                    case "ProductionValueGoodAnswer":
                        textInCanvas.text = (Building.getQuantityResourceOrStatProduced(island.buildingClicked) * 2).ToString();
                        break;
                    case "ProductionValueBadAnswer":
                        textInCanvas.text = (Building.getQuantityResourceOrStatProduced(island.buildingClicked)).ToString();
                        break;
                }
            }
            //modification of the background of the different Image Children of the Canvas
            foreach (SpriteRenderer imageInCanvas in buildingInfo.GetComponent<Canvas>().GetComponentsInChildren<SpriteRenderer>())
            {
                switch (imageInCanvas.name)
                {
                    case "CostImage1":
                        imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + constructionResourceNeeded[0].First.ToString() + "Icon");
                        break;
                    case "CostImage2":
                        if (constructionResourceNeeded[1] != null)
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + constructionResourceNeeded[1].First.ToString() + "Icon");
                        else
                            imageInCanvas.sprite = null;
                        break;
                    //mêmes images
                    case "ProductionImage":
                        imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + Building.getNameResourceOrStatProduced(island.buildingClicked) + "Icon");
                        break;
                    case "ProductionImage2":
                        imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + Building.getNameResourceOrStatProduced(island.buildingClicked) + "Icon");
                        break;
                    case "Build":
                        imageInCanvas.GetComponent<BoxCollider2D>().enabled = false;
                        Debug.Log(island.resourceManager.getResource(constructionResourceNeeded[0].First).Stock.ToString());
                        Debug.Log(island.resourceManager.getResource(constructionResourceNeeded[1].First).Stock.ToString());
                        if (island.resourceManager.getResource(constructionResourceNeeded[0].First).Stock < constructionResourceNeeded[0].Second)
                        {
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/boutonConstruireGrise");
                        }
                        else if (constructionResourceNeeded[1] != null)
                        {
                            if (island.resourceManager.getResource(constructionResourceNeeded[1].First).Stock < constructionResourceNeeded[1].Second)
                                imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/boutonConstruireGrise");
                        }


                            break;
                }
            }
            //rotation of image according to the place of the island
            char id = island.nameMinorIsland[island.nameMinorIsland.Length - 1];
            if (id == '1' || id == '2')
                buildingInfo.transform.Rotate(Vector3.forward * 180);

            island.buildingInfoPresent = true;
        }

    }

    

    
}
