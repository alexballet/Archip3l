using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class Building : MonoBehaviour {

    public TypeBuilding TypeBuilding { get; private set; }
    public ResourceManager resourceManager { get; private set; }
    public int buildState { get; private set; }
    public int constructionTime { get; private set; }
    public MinorIsland minorIsland { get; private set; }
    private List<Tuple<TypeResource, int>> constructionResourceNeeded;

    public Transform resourceManagerPrefab;
    public Transform buildingConstructionPrefab;

    public void init(TypeBuilding TypeBuilding, MinorIsland island)
    {
        this.TypeBuilding = TypeBuilding;
        this.buildState = 0;
        this.minorIsland = island;
        this.constructionResourceNeeded = new List<Tuple<TypeResource, int>>();

        var resourceManagerTransform = Instantiate(resourceManagerPrefab) as Transform;
        ResourceManager resourceManager = resourceManagerTransform.GetComponent<ResourceManager>();
        if (resourceManager != null)
        {
            //pb du manager d'ile et de batiment^^
            resourceManager.init(null);
            resourceManager.transform.SetParent(this.transform);
            this.resourceManager = resourceManager;
        }

        //must be set in the switch
        string texturePath = "Assets/Resources/Building/mapIconMine.png";

        Texture2D texture = (Texture2D) AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
        //SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        GetComponent<SpriteRenderer>().sprite =  Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        GetComponent<Transform>().localScale = new Vector3(100f, 100f, 1f);

        switch (TypeBuilding)
        {
            case TypeBuilding.GoldMine:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Gold, 3));
                //this.resourceManager.addResource(TypeResource.Bois, "bois", 0, 4);
                //null instance from now
                this.constructionTime = 5;
                break;
            case TypeBuilding.StoneMine:
                //this.resourceManager.addResource(TypeResource.Or, "Or", 0, 1);
                this.constructionTime = 5;
                break;
            case TypeBuilding.OilPlant:
                this.constructionTime = 5;
                break;
            case TypeBuilding.Sawmill:
                this.constructionTime = 5;
                break;
            case TypeBuilding.Factory:
                this.constructionTime = 5;
                break;
            case TypeBuilding.WindTurbine:
                this.constructionTime = 5;
                break;
            case TypeBuilding.Farm:
                this.constructionTime = 5;
                break;
        }
        StartCoroutine("build");
        Debug.Log("Construction " + this.TypeBuilding);
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
            this.buildState = 0;
            foreach (Tuple<TypeResource, int> item in this.constructionResourceNeeded)
            {
                this.minorIsland.resourceManager.changeResourceStock(item.First, -item.Second);
            }
        }

        //Animation
        var buildingConstructionTransform = Instantiate(buildingConstructionPrefab) as Transform;
        Anim_BuildingConstruction anim_BuildingConstruction = buildingConstructionTransform.GetComponent<Anim_BuildingConstruction>();
        if (anim_BuildingConstruction != null)
        {
            anim_BuildingConstruction.transform.SetParent(this.transform);
        }

        yield return new WaitForSeconds(this.constructionTime);
        Destroy(buildingConstructionTransform.gameObject);
    }
    public bool changeProduction(TypeResource resourceType, int value)
    {
        return this.resourceManager.changeResourceProduction(resourceType, value);
    }
}
