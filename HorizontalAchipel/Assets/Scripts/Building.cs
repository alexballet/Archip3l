using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
    public string buildingName;
    public string ressourceNeeded;
    public int consumptionCost;     //quantity consumed every 10 seconds
    public string ressourceProduced;
    public int productionCost;      //quantity produced every 10 seconds
    public int state; //0 : being built, 1 : built, 2 : closed
    public string imageBeingBuilt;
    public string imageBuilt;
    public int indexCanvas;
    public int coordX;
    public int coordY;
    public int constructionTime;    //time after which the building'state becomes 1
    public GameObject buldingGameObject;


    public static Building CreateComponent(string island, string argName, int x, int y)     //TODO : finir switch
    {
        Building building = GameObject.Find(island).AddComponent<Building>();
        building.buildingName = argName;


        switch (argName)
        {
            case "scierie":
                building.ressourceNeeded = "or";
                building.consumptionCost = 3;
                building.ressourceProduced = "bois";
                building.productionCost = 4;
                building.constructionTime = 5;
                break;
            case "mine":
                building.ressourceNeeded = null;
                building.consumptionCost = 0;
                building.ressourceProduced = "or";
                building.productionCost = 0;
                building.constructionTime = 0;
                break;
            case "usine":
                building.ressourceNeeded = null;
                building.consumptionCost = 0;
                building.ressourceProduced = "metal";
                building.productionCost = 0;
                building.constructionTime = 0;
                break;
            case "ferme":
                building.ressourceNeeded = null;
                building.consumptionCost = 0;
                building.ressourceProduced = "metal";
                building.productionCost = 0;
                building.constructionTime = 0;
                break;
        }
        building.coordX = x;
        building.coordY = y;
        building.imageBeingBuilt = "building-beingbuilt-" + argName;
        building.imageBuilt = "building-built-" + argName;

        return building;
    }
    

    public IEnumerator build(int time, int x, int y, string island)
    {
        state = 0;
        Debug.Log("La construction du batiment " + buildingName + " a commencé !");

        //creation of the image of the building in construction

        buldingGameObject = new GameObject(island + "_" + buildingName);
        buldingGameObject.AddComponent<SpriteRenderer>();
        buldingGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(imageBeingBuilt);
        buldingGameObject.GetComponent<SpriteRenderer>().transform.position = new Vector2(-5, 3); //new Vector2(x, y);


        //ATTENTE BLOQUANTE
        //StartCoroutine(wait(time));
        yield return new WaitForSeconds(time);
        state = 1;
        Debug.Log("Le batiment " + buildingName + " est construit !");
        //await Task.Delay(TimeSpan.FromSeconds(time));   //construction of the building
        //state = 1;
        //Debug.Log("Le batiment " + buildingName + " est construit !");

        ////modification of the image: the building is now built
        buldingGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(imageBuilt);
        yield break;

    }

    IEnumerator wait(int sec)
    {
        yield return new WaitForSeconds(sec);
    }


    //consume the ressourceNeedded and produce the ressourceProduced (only if there was still a stock of ressourceNeedded)
    //method called by an Island, each 10 seconds while the state of the building is 1
    public void consume_produce(Island island)
    {
        RessourceManager rm = new RessourceManager();
        //checks if the ressourceNeeded exists and if there is enough of its stock
        if ((island.getRessource(ressourceNeeded) != null) && (island.getRessource(ressourceNeeded).stock >= consumptionCost))
        {
            rm.withdrawRessource(ressourceNeeded, island, consumptionCost); //consumption
            rm.giveRessource(ressourceProduced, island, productionCost);    //production
            Debug.Log("Le batiment " + this.buildingName + " utilise " + this.consumptionCost.ToString() + " " + this.ressourceNeeded + " et produit " + this.productionCost.ToString() + " " + this.ressourceProduced + " sur l'ile " + island.id.ToString());
        }
        else
        {
            Debug.Log("Le batiment " + this.buildingName + " ne peut plus produire de " + this.ressourceProduced + " car il ne reste pas assez de " + ressourceNeeded + " sur l'ile " + island.id.ToString());
        }
    }

    public bool increaseProduction(int quantity)
    {
        productionCost += quantity;
        Debug.Log("Le batiment " + this.buildingName + " augmente sa production de " + quantity.ToString());
        return true;
    }

    public bool increaseConsumption(int quantity)
    {
        consumptionCost += quantity;
        Debug.Log("Le batiment " + this.buildingName + " augmente sa consommation de " + quantity.ToString());
        return true;

    }

    public bool decreaseProduction(int quantity)
    {
        if (productionCost == 0)
        {
            Debug.Log("Le batiment " + this.buildingName + " a deja un taux de production nul");
            return false;
        }
        else
        {
            productionCost -= quantity;
            if (productionCost < 0)
                productionCost = 0;
            Debug.Log("Le batiment " + this.buildingName + " a maintenant un taux de production de " + productionCost.ToString());
            return true;
        }
    }

    public bool decreaseConsumption(int quantity)
    {
        if (consumptionCost == 0)
        {
            Debug.Log("Le batiment " + this.buildingName + " a deja un taux de production nul");
            return false;
        }
        else
        {
            consumptionCost -= quantity;
            if (consumptionCost < 0)
                consumptionCost = 0;
            Debug.Log("Le batiment " + this.buildingName + " a maintenant un taux de consommation de " + productionCost.ToString());
            return true;
        }
    }

    public void moveBuilding(int x, int y)
    {
        this.coordX = x;
        this.coordY = y;
    }

}

