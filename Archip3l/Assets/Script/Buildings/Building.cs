using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Building : MonoBehaviour {

    public TypeBuilding TypeBuilding { get; private set; }
    public ResourceManagerBuilding resourceManager { get; private set; }
    public int buildState { get; private set; }
    public int constructionTime { get; private set; }
    public MinorIsland minorIsland { get; private set; }
    private List<Tuple<TypeResource, int>> constructionResourceNeeded;
    public int level;       //possible levels : 0-1-2-3

    public Transform resourceManagerPrefab;
    public Transform buildingConstructionPrefab;

    public void init(TypeBuilding TypeBuilding, MinorIsland island)
    {
        this.level = 0;
        this.TypeBuilding = TypeBuilding;
        this.buildState = 0;
        this.minorIsland = island;
        this.constructionResourceNeeded = new List<Tuple<TypeResource, int>>();
        this.name = this.minorIsland.nameMinorIsland + "_" + this.TypeBuilding.ToString();

        var resourceManagerTransform = Instantiate(resourceManagerPrefab) as Transform;
        ResourceManagerBuilding resourceManager = resourceManagerTransform.GetComponent<ResourceManagerBuilding>();
        if (resourceManager != null)
        {
            resourceManager.init(this);
            resourceManager.transform.SetParent(this.transform);
            this.resourceManager = resourceManager;
        }

        //string texturePath = "Assets/Resources/Building/Icons/mapIcon" + TypeBuilding.ToString() + ".png";
        string texturePath = "Assets/Resources/Building/Icons/wheelIcon" + TypeBuilding.ToString() + ".png";

        switch (TypeBuilding)
        {
            case TypeBuilding.GoldMine:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 5));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 5));
                //addResource(TypeResource resourceType, string name, int quantity, int production)
                this.resourceManager.addResource(TypeResource.Gold, "Gold", 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.StoneMine:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 5));
                //this.resourceManager.addResource(TypeResource.Or, "Or", 0, 1);
                this.constructionTime = 5;
                break;
            case TypeBuilding.OilPlant:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 20));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 40));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Sawmill:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 5));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 5));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Factory:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 50));
                this.constructionTime = 5;
                break;
            case TypeBuilding.WindTurbine:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 10));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 20));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Farm:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 5));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Harbor:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 20));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                this.constructionTime = 5;
                break;
            case TypeBuilding.PowerPlant:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 20));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 40));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Lab:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 10));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Airport:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 200));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 200));
                this.constructionTime = 30;
                break;
            case TypeBuilding.Hotel:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 20));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 40));
                this.constructionTime = 30;
                break;
            case TypeBuilding.School:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 20));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 20));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Church:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 40));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 80));
                this.constructionTime = 10;
                break;
            case TypeBuilding.Cinema:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 30));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 50));
                this.constructionTime = 15;
                break;
            case TypeBuilding.AmusementPark:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 100));
                this.constructionTime = 30;
                break;
        }

        Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
        GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //GetComponent<Transform>().localScale = new Vector3(100f, 100f, 1f);
        GetComponent<Transform>().localScale = new Vector3(9f, 9f, 1f);

        StartCoroutine("build");
        //Debug.Log("Construction " + this.TypeBuilding);
    }
    IEnumerator build()
    {
        bool flag = true;
        //check needed resources
        foreach (Tuple<TypeResource, int> item in this.constructionResourceNeeded)
        {
            //avoid null references
            Resource resource = this.minorIsland.resourceManager.getResource(item.First);
            if ((resource == null) || (item.Second < resource.Stock))
            {
                flag = false;
            }
        }

        if(flag)
        {
            foreach (Tuple<TypeResource, int> item in this.constructionResourceNeeded)
            {
                this.minorIsland.resourceManager.changeResourceStock(item.First, -item.Second);
            }
            //Animation
            
            var buildingConstructionTransform = Instantiate(buildingConstructionPrefab) as Transform;
            buildingConstructionTransform.name = "BuildingAnnimation_" + minorIsland.nameMinorIsland;
            Anim_BuildingConstruction anim_BuildingConstruction = buildingConstructionTransform.GetComponent<Anim_BuildingConstruction>();
            if (anim_BuildingConstruction != null)
            {
                anim_BuildingConstruction.transform.SetParent(this.transform);
            }

            yield return new WaitForSeconds(this.constructionTime);
            this.buildState = 1;
            Destroy(buildingConstructionTransform.gameObject);
        }
        else
        {
            //Not enough resources
        }
    }
    public bool changeProduction(TypeResource resourceType, int value)
    {
        return this.resourceManager.changeResourceProduction(resourceType, value);
    }

    void OnMouseDown()
    {

        if (!minorIsland.wheelPresent && !minorIsland.buildingInfoPresent && !minorIsland.challengePresent && !minorIsland.moveBuilding && minorIsland.nameBuildingTouchCanvas == String.Empty)
        {
            minorIsland.createBuildingTouch(this);
        }

    }
}
