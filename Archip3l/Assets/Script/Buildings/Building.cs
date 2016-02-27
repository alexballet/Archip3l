using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Building : MonoBehaviour {

    public TypeBuilding TypeBuilding { get; private set; }
    public Resource resourceProduced { get; private set; }
    public int buildState { get; private set; }
    public int constructionTime { get; private set; }
    public MinorIsland minorIsland { get; private set; }
    public List<Tuple<TypeResource, int>> constructionResourceNeeded { get; private set; }
    public List<Tuple<TypeResource, int>> upgrade1ResourceNeeded { get; private set; }
    public List<Tuple<TypeResource, int>> upgrade2ResourceNeeded { get; private set; }
    public List<Tuple<TypeResource, int>> upgrade3ResourceNeeded { get; private set; }
    private string texturePath;
    public int level;       //possible levels : 0-1-2-3

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

        this.resourceProduced = ScriptableObject.CreateInstance<Resource>();
        this.texturePath = "Assets/Resources/Building/Icons/wheelIcon_" + TypeBuilding.ToString() + ".png";

        this.constructionResourceNeeded = getConstructionResourcesNeeded(TypeBuilding.ToString());

        switch (TypeBuilding)
        {
            case TypeBuilding.GoldMine:
                this.resourceProduced.init(TypeResource.Gold, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 2));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 2));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 5));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 7));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 7));
                this.constructionTime = 5;
                break;
            case TypeBuilding.StoneMine:
                this.resourceProduced.init(TypeResource.Stone, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 2));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 2));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 5));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 7));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 7));
                this.constructionTime = 5;
                break;
            case TypeBuilding.OilPlant:
                this.resourceProduced.init(TypeResource.Oil, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 10));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 20));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 20));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 40));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 25));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 50));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Sawmill:
                this.resourceProduced.init(TypeResource.Wood, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 2));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 2));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 5));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 5));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 7));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 7));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Factory:
                this.resourceProduced.init(TypeResource.Manufacture, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 25));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 50));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 63));
                this.constructionTime = 5;
                break;
            case TypeBuilding.WindTurbine:
                this.resourceProduced.init(TypeResource.Electricity, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 5));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 10));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 10));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 20));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 13));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 25));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Farm:
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 2));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 2));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 5));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 7));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 7));
                this.resourceProduced.init(TypeResource.Food, 0, 5);
                this.constructionTime = 5;
                break;
            case TypeBuilding.Harbor:
                this.resourceProduced.init(TypeResource.Food, 0, 10);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 10));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 20));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 20));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 40));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 25));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 50));
                this.constructionTime = 5;
                break;
            case TypeBuilding.PowerPlant:
                this.resourceProduced.init(TypeResource.Electricity, 0, 10);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 10));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 20));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 20));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 40));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 25));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 50));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Lab:
                this.resourceProduced.init(TypeResource.Health, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 2));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 5));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 5));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 10));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 7));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 13));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Airport:
                this.resourceProduced.init(TypeResource.Tourism, 0, 20);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 100));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 100));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 200));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 200));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 250));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 250));
                this.constructionTime = 30;
                break;
            case TypeBuilding.Hotel:
                this.resourceProduced.init(TypeResource.Tourism, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 10));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 20));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 20));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 40));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 25));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 50));
                this.constructionTime = 30;
                break;
            case TypeBuilding.School:
                this.resourceProduced.init(TypeResource.Education, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 10));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 10));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 20));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 20));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 25));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Food, 25));
                this.constructionTime = 5;
                break;
            case TypeBuilding.Church:
                this.resourceProduced.init(TypeResource.Religion, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 20));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 40));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 80));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Wood, 50));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Stone, 100));
                this.constructionTime = 10;
                break;
            case TypeBuilding.Cinema:
                this.resourceProduced.init(TypeResource.Happiness, 0, 5);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 15));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 25));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 30));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 50));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 38));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 63));
                this.constructionTime = 15;
                break;
            case TypeBuilding.AmusementPark:
                this.resourceProduced.init(TypeResource.Happiness, 0, 10);
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 30));
                this.upgrade1ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 50));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 60));
                this.upgrade2ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 100));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 75));
                this.upgrade3ResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Oil, 125));
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
            StartCoroutine("updateStocks");
            Destroy(buildingConstructionTransform.gameObject);
        }
        else
        {
            //Not enough resources
            Debug.Log("Not enough resources");
        }
    }
    public bool changeProduction(int value)
    {
        this.minorIsland.resourceManager.changeResourceProduction(this.resourceProduced.TypeResource, value);
        return this.resourceProduced.changeProduction(value); // resourceManager.changeResourceProduction(resourceType, value);

    }
    public bool changeStock(int value)
    {
        return this.minorIsland.resourceManager.changeResourceStock(this.resourceProduced.TypeResource, value);
    }
    IEnumerator updateStocks()
    {
        for (;;)
        {
            this.changeStock(this.resourceProduced.Production);
            yield return new WaitForSeconds(5f);
        }
    }

    void OnMouseDown()
    {

        if ((this.buildState == 1) && !minorIsland.wheelPresent && !minorIsland.buildingInfoPresent && !minorIsland.challengePresent && !minorIsland.moveBuilding && minorIsland.nameBuildingTouchCanvas == String.Empty)
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
