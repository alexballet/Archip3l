using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {

    public TypeBuilding TypeBuilding { get; private set; }
    public ResourceManager resourceManager { get; private set; }
    public int buildState { get; private set; }
    public int constructionTime { get; private set; }
    public MinorIsland minorIsland { get; private set; }
    private List<Tuple<TypeResource, int>> constructionResourceNeeded;

    public void init(TypeBuilding TypeBuilding, MinorIsland island)
    {
        this.TypeBuilding = TypeBuilding;
        this.resourceManager = new ResourceManager();
        this.buildState = 0;
        this.minorIsland = island;
        this.constructionResourceNeeded = new List<Tuple<TypeResource, int>>();

        switch (TypeBuilding)
        {
            case TypeBuilding.Scierie:
                this.constructionResourceNeeded.Add(new Tuple<TypeResource, int>(TypeResource.Or, 3));
                this.resourceManager.addResource(TypeResource.Bois, "bois", 0, 4);
                //ressourceNeeded = "or";
                //consumptionCost = 3;
                //ressourceProduced = "bois";
                //productionCost = 4;
                this.constructionTime = 5;
                break;
            case TypeBuilding.Mine:
                this.resourceManager.addResource(TypeResource.Or, "Or", 0, 1);
                this.constructionTime = 0;
                break;
            case TypeBuilding.Usine:
                this.constructionTime = 0;
                break;
            case TypeBuilding.Ferme:
                this.constructionTime = 0;
                break;
        }
        //this.build();
    }


    /*public async Task<bool> build()
    {
        //check needed resources
        foreach (Tuple<ResourceType, int> item in this.ConstructionResourceNeeded)
        {
            //avoid null references
            Resource resource = this.Island.ResourceManager.getResource(item.Item1);
            if ((resource == null) || (item.Item2 < resource.Stock))
            {
                return false;
            }
        }

        //start construction
        this.BuildState = 0;
        foreach (Tuple<ResourceType, int> item in this.ConstructionResourceNeeded)
        {
            this.Island.ResourceManager.changeResourceStock(item.Item1, -item.Item2);
        }
        if (this.BuildingConstructionStart != null)
        {
            this.BuildingConstructionStart(this, new BuildingConstructionStartEventArgs { TypeBuilding = this.TypeBuilding, Island = this.Island });
        }
        await Task.Delay(TimeSpan.FromSeconds(this.ConstructionTime));

        //end construction
        this.BuildState = 1;
        if (this.BuildingConstructionEnd != null)
        {
            this.BuildingConstructionEnd(this, new BuildingConstructionEndEventArgs { TypeBuilding = this.TypeBuilding, Island = this.Island });
        }
        return true;
    }*/

    public bool changeProduction(TypeResource resourceType, int value)
    {
        return this.resourceManager.changeResourceProduction(resourceType, value);
    }







    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
