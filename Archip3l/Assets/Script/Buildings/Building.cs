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
    public List<Tuple<TypeResource, int>> constructionResourceNeeded { get; private set; }
    public List<Tuple<TypeResource, int>> upgrade1ResourceNeeded { get; private set; }
    public List<Tuple<TypeResource, int>> upgrade2ResourceNeeded { get; private set; }
    public List<Tuple<TypeResource, int>> upgrade3ResourceNeeded { get; private set; }
    private string texturePath;
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
        this.upgrade1ResourceNeeded = new List<Tuple<TypeResource, int>>();
        this.upgrade2ResourceNeeded = new List<Tuple<TypeResource, int>>();
        this.upgrade3ResourceNeeded = new List<Tuple<TypeResource, int>>();
        this.name = this.minorIsland.nameMinorIsland + "_" + this.TypeBuilding.ToString();

        var resourceManagerTransform = Instantiate(resourceManagerPrefab) as Transform;
        ResourceManagerBuilding resourceManager = resourceManagerTransform.GetComponent<ResourceManagerBuilding>();
        if (resourceManager != null)
        {
            resourceManager.init(this);
            resourceManager.transform.SetParent(this.transform);
            this.resourceManager = resourceManager;
        }

        this.texturePath = "Assets/Resources/Building/Icons/wheelIcon_" + TypeBuilding.ToString() + ".png";

        this.constructionResourceNeeded = getConstructionResourcesNeeded(TypeBuilding.ToString());

        switch (TypeBuilding)
        {
            case TypeBuilding.GoldMine:
                this.resourceManager.addResource(TypeResource.Gold, 0, 5);
                this.resourceManager.addResource(TypeResource.Gold, 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.StoneMine:
                this.resourceManager.addResource(TypeResource.Stone, 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.OilPlant:
                this.resourceManager.addResource(TypeResource.Oil, 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.Sawmill:
                this.resourceManager.addResource(TypeResource.Wood, 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.Factory:
                this.resourceManager.addResource(TypeResource.Gold, 0, 5);
                this.resourceManager.addResource(TypeResource.Manufacture, 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.WindTurbine:
                this.resourceManager.addResource(TypeResource.Electricity, 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.Farm:
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 2));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 2));
                this.resourceManager.addResource(TypeResource.Food, 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.Harbor:
                this.resourceManager.addResource(TypeResource.Food, 0, 10);
                this.constructionTime = 5;
                break;
            case TypeBuilding.PowerPlant:
                this.resourceManager.addResource(TypeResource.Electricity, 0, 10);
                this.constructionTime = 5;
                break;
            case TypeBuilding.Lab:
                this.resourceManager.addResource(TypeResource.Health, 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.Airport:
                this.resourceManager.addResource(TypeResource.Tourism, 0, 20);
                this.constructionTime = 30;
                break;
            case TypeBuilding.Hotel:
                this.resourceManager.addResource(TypeResource.Tourism, 0, 5);
                this.constructionTime = 30;
                break;
            case TypeBuilding.School:
                this.resourceManager.addResource(TypeResource.Education, 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.Church:
                this.resourceManager.addResource(TypeResource.Religion, 0, 5);
                this.constructionTime = 10;
                break;
            case TypeBuilding.Cinema:
                this.resourceManager.addResource(TypeResource.Happiness, 0, 5);
                this.constructionTime = 15;
                break;
            case TypeBuilding.AmusementPark:
                this.resourceManager.addResource(TypeResource.Happiness, 0, 10);
                this.constructionTime = 30;
                break;
        }

        GetComponent<Transform>().localScale = new Vector3(9f, 9f, 1f);

        StartCoroutine("build");
    }
    IEnumerator build()
    {
        bool flag = true;
        //check needed resources
        foreach (Tuple<TypeResource, int> item in this.constructionResourceNeeded)
        {
            //avoid null references
            Resource resource = this.minorIsland.resourceManager.getResource(item.First);
            if (item.Second > resource.Stock)
            {
                flag = false;
            }
        }

        if (flag == true)
        {
            foreach (Tuple<TypeResource, int> item in this.constructionResourceNeeded)
            {
                this.minorIsland.resourceManager.changeResourceStock(item.First, -item.Second);
            }

            //texture
            Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(this.texturePath, typeof(Texture2D));
            GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

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
            Debug.Log("Not enough resources");
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

    static public List<Tuple<TypeResource, int>> getConstructionResourcesNeeded(string nameBuilding)
    {
        List<Tuple<TypeResource, int>> constructionResourcesNeeded = new List<Tuple<TypeResource, int>>();

        switch (nameBuilding)
        {
            case "GoldMine":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 5));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 5));
                break;
            case "StoneMine":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 5));
                break;
            case "OilPlant":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 20));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 40));
                break;
            case "Sawmill":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 5));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 5));
                break;
            case "Factory":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 50));
                break;
            case "WindTurbine":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 10));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 20));
                break;
            case "Farm":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 5));
                break;
            case "Harbor":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 20));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                break;
            case "PowerPlant":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 20));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 40));
                break;
            case "Lab":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 10));
                break;
            case "Airport":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 200));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 200));
                break;
            case "Hotel":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 20));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 40));
                break;
            case "School":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 20));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 20));
                break;
            case "Church":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 40));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 80));
                break;
            case "Cinema":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 30));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 50));
                break;
            case "AmusementPark":
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                constructionResourcesNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 100));
                break;
        }
        return constructionResourcesNeeded;
    }


    //returns the name of the resource (or stat) produced
    static public string getNameResourceOrStatProduced(string buildingName)
    {
        switch (buildingName)
        {
            case "GoldMine":
                return "Gold";
            case "StoneMine":
                return "Stone";
            case "OilPlant":
                return "Oil";
            case "Sawmill":
                return "Wood";
            case "Factory":
                return "Manufacture";
            case "WindTurbine":
                return "Electricity";
            case "Farm":
                return "Food";
            case "Lab":
                return "Health";
            case "Airport":
                return "Tourism";
            case "Hotel":
                return "Tourism";
            case "Harbor":
                return "Food";
            case "School":
                return "Education";
            case "Church":
                return "Religion";
            case "Cinema":
                return "Happiness";
            case "AmusementPark":
                return "Happiness";
            case "PowerPlant":
                return "Electricity";
            default:
                return string.Empty;
        }
    }

}
