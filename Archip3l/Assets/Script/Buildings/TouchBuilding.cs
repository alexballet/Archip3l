﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TouchBuilding : MonoBehaviour {

    public MinorIsland island;
    public Building building;

    void OnMouseDown()
    {
        island = GameObject.Find(this.transform.parent.parent.parent.name).GetComponent<MinorIsland>();
        building = GameObject.Find(island.nameBuildingTouchCanvas).GetComponent<Building>();

        char id = island.nameMinorIsland[island.nameMinorIsland.Length - 1];

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
                if (id == '1' || id == '2')
                    upgradeBuildingWindowCanvas.transform.Rotate(Vector3.forward * 180);
                //modification of the content of the different Text Children of the Canvas
                foreach (Text textInCanvas in upgradeBuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<Text>())
                {
                    switch (textInCanvas.name)
                    {
                        case "Name":
                            textInCanvas.text = "Amélioration " + (building.level + 1).ToString();  
                            break;
                        case "CostValue1":
                            switch (building.level)
                            {
                                case 0:
                                    textInCanvas.text = building.upgrade1ResourceNeeded[0].Second.ToString();
                                    break;
                                case 1:
                                    textInCanvas.text = building.upgrade2ResourceNeeded[0].Second.ToString();
                                    break;
                                case 2:
                                    textInCanvas.text = building.upgrade3ResourceNeeded[0].Second.ToString();
                                    break;
                                default:
                                    textInCanvas.text = "-";
                                    break;
                            }                                          
                            break;
                        case "CostValue2":
                            switch (building.level)
                            {
                                case 0:
                                    if (building.upgrade1ResourceNeeded.Count == 2)
                                        textInCanvas.text = building.upgrade1ResourceNeeded[1].Second.ToString();
                                    else
                                        textInCanvas.text = "-";
                                    break;
                                case 1:
                                    if (building.upgrade2ResourceNeeded.Count == 2)
                                        textInCanvas.text = building.upgrade2ResourceNeeded[1].Second.ToString();
                                    else
                                        textInCanvas.text = "-";
                                    break;
                                case 2:
                                    if (building.upgrade3ResourceNeeded.Count == 2)
                                        textInCanvas.text = building.upgrade3ResourceNeeded[1].Second.ToString();
                                    else
                                        textInCanvas.text = "-";
                                    break;
                                default:
                                    textInCanvas.text = "-";
                                    break;
                            }
                            break;
                    }
                }
                //modification of the background of the different Image Children of the Canvas
                foreach (Image imageInCanvas in upgradeBuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<Image>())
                {
                    switch (imageInCanvas.name)
                    {
                        case "CostImage1":
                            switch (building.level)
                            {
                                case 0:
                                    imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + building.upgrade1ResourceNeeded[0].First.ToString() + "Icon");
                                    break;
                                case 1:
                                    imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + building.upgrade2ResourceNeeded[0].First.ToString() + "Icon");
                                    break;
                                case 2:
                                    imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + building.upgrade3ResourceNeeded[0].First.ToString() + "Icon");
                                    break;
                                default:
                                    imageInCanvas.sprite = null;
                                    break;
                            }
                            break;
                        case "CostImage2":
                            switch (building.level)
                            {
                                case 0:
                                    if (building.upgrade1ResourceNeeded.Count == 2)
                                        imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + building.upgrade1ResourceNeeded[1].First.ToString() + "Icon");
                                    else
                                        imageInCanvas.sprite = null;
                                    break;
                                case 1:
                                    if (building.upgrade2ResourceNeeded.Count == 2)
                                        imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + building.upgrade2ResourceNeeded[1].First.ToString() + "Icon");
                                    else
                                        imageInCanvas.sprite = null;
                                    break;
                                case 2:
                                    if (building.upgrade3ResourceNeeded.Count == 2)
                                        imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + building.upgrade3ResourceNeeded[1].First.ToString() + "Icon");
                                    else
                                        imageInCanvas.sprite = null;
                                    break;
                                default:
                                    imageInCanvas.sprite = null;
                                    break;
                            }
                            break;
                        //mêmes images
                        case "ProductionImage":
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + Building.getNameResourceOrStatProduced(building.name.Split('_')[3]) + "Icon");
                            break;
                        case "ProductionImage2":
                            imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + Building.getNameResourceOrStatProduced(building.name.Split('_')[3]) + "Icon");
                            break;
                    }

                    //check if upgrade is possible
                    foreach (SpriteRenderer sr in upgradeBuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (sr.name == "Upgrade")
                        {
                            switch (building.level)
                            {
                                case 0:
                                    if (island.resourceManager.getResource(building.upgrade1ResourceNeeded[0].First).Stock < building.upgrade1ResourceNeeded[0].Second)
                                    {
                                        sr.sprite = Resources.Load<Sprite>("infoBatiments/boutonAmeliorerGrise");
                                        sr.GetComponent<PolygonCollider2D>().enabled = false;
                                    }
                                    else if (building.upgrade1ResourceNeeded.Count == 2)
                                    {
                                        if (island.resourceManager.getResource(building.upgrade1ResourceNeeded[1].First).Stock < building.upgrade1ResourceNeeded[1].Second)
                                        {
                                            sr.sprite = Resources.Load<Sprite>("infoBatiments/boutonAmeliorerGrise");
                                            sr.GetComponent<PolygonCollider2D>().enabled = false;
                                        }
                                    }
                                    break;
                                case 1:
                                    if (island.resourceManager.getResource(building.upgrade2ResourceNeeded[0].First).Stock < building.upgrade2ResourceNeeded[0].Second)
                                    {
                                        sr.sprite = Resources.Load<Sprite>("infoBatiments/boutonAmeliorerGrise");
                                        sr.GetComponent<PolygonCollider2D>().enabled = false;
                                    }
                                    else if (building.upgrade2ResourceNeeded.Count == 2)
                                    {
                                        if (island.resourceManager.getResource(building.upgrade2ResourceNeeded[1].First).Stock < building.upgrade2ResourceNeeded[1].Second)
                                        {
                                            sr.sprite = Resources.Load<Sprite>("infoBatiments/boutonAmeliorerGrise");
                                            sr.GetComponent<PolygonCollider2D>().enabled = false;
                                        }
                                    }
                                    break;
                                case 2:
                                    if (island.resourceManager.getResource(building.upgrade3ResourceNeeded[0].First).Stock < building.upgrade3ResourceNeeded[0].Second)
                                    {
                                        sr.sprite = Resources.Load<Sprite>("infoBatiments/boutonAmeliorerGrise");
                                        sr.GetComponent<PolygonCollider2D>().enabled = false;
                                    }
                                    else if (building.upgrade3ResourceNeeded.Count == 2)
                                    {
                                        if (island.resourceManager.getResource(building.upgrade3ResourceNeeded[1].First).Stock < building.upgrade3ResourceNeeded[1].Second)
                                        {
                                            sr.sprite = Resources.Load<Sprite>("infoBatiments/boutonAmeliorerGrise");
                                            sr.GetComponent<PolygonCollider2D>().enabled = false;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }

                break;
            case "Remove":
                if (this.building.name.Split('_')[3] == TypeBuilding.Harbor.ToString())
                {
                    StartCoroutine(this.island.destroyPopup(this.island.createPopup("Impossible de supprimer le port !"), 3));
                }
                else {
                    Canvas removeBuildingWindowCanvasPrefab = Resources.Load<Canvas>("Prefab/RemoveBuildingWindowCanvas");
                    Canvas removeBuildingWindowCanvas = Instantiate(removeBuildingWindowCanvasPrefab);
                    removeBuildingWindowCanvas.name = "RemoveBuildingWindowCanvas_" + building.name;
                    removeBuildingWindowCanvas.transform.SetParent(this.transform.parent.parent.parent);  //parent : sous_ile
                    removeBuildingWindowCanvas.transform.position = island.transform.position;
                    //rotation of image according to the place of the island
                    if (id == '1' || id == '2')
                        removeBuildingWindowCanvas.transform.Rotate(Vector3.forward * 180);
                    //modification of the content of the different Text Children of the Canvas
                    foreach (Text textInCanvas in removeBuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<Text>())
                    {
                        switch (textInCanvas.name)
                        {
                            case "Question":
                                textInCanvas.text = "Etes vous sûr de vouloir détruire le bâtiment \"" + Building.translateBuildingName(building.TypeBuilding.ToString()) + "\" ?";
                                break;
                            case "GainValue1":
                                textInCanvas.text = (building.constructionResourceNeeded[0].Second / 2).ToString();
                                break;
                            case "GainValue2":
                                if (building.constructionResourceNeeded.Count == 2)
                                    textInCanvas.text = (building.constructionResourceNeeded[1].Second / 2).ToString();
                                else
                                    textInCanvas.text = "-";
                                break;
                        }
                    }
                    //modification of the background of the different Image Children of the Canvas
                    foreach (SpriteRenderer imageInCanvas in removeBuildingWindowCanvas.GetComponent<Canvas>().GetComponentsInChildren<SpriteRenderer>())
                    {
                        switch (imageInCanvas.name)
                        {
                            //mêmes images
                            case "GainImage1":
                                imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + building.constructionResourceNeeded[0].First.ToString() + "Icon");
                                break;
                            case "GainImage2":
                                if (building.constructionResourceNeeded.Count == 2)
                                    imageInCanvas.sprite = Resources.Load<Sprite>("infoBatiments/ResourcesIcons/" + building.constructionResourceNeeded[1].First.ToString() + "Icon");
                                else
                                    imageInCanvas.sprite = null;
                                break;
                        }
                    }
                    Destroy(GameObject.Find(this.transform.parent.parent.name));
                }
                break;
            case "Move":
                if (this.building.name.Split('_')[3] == TypeBuilding.Harbor.ToString())
                {
                    StartCoroutine(this.island.destroyPopup(this.island.createPopup("Impossible de déplacer le port !"), 3));
                }
                else {
                    Destroy(GameObject.Find(this.transform.parent.parent.name));

                    if (building.TypeBuilding.ToString() == "Harbor")
                    {
                        StartCoroutine(island.destroyPopup(island.createPopup("Le port ne peut pas être déplacé !"), 3));
                    }
                    else
                    {
                        StartCoroutine(island.destroyPopup(island.createPopup("Appuyez sur l'endroit où placer le batiment"), 3));
                        island.moveBuilding = true;
                    }
                }
                break;
        }
    }
}
