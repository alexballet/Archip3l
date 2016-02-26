using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManagerBuilding : MonoBehaviour {

    public Building building { get; private set; }
    public List<Resource> Resources { get; private set;}

    public void init(Building building)
    {
        this.building = building;
        this.Resources = new List<Resource>();
    }
    void Start()
    {
        StartCoroutine("updateStocks");
    }

    public bool addResource(TypeResource resourceType, string name, int quantity, int production)
    {
        bool flag = false;
        foreach (Resource item in this.Resources)
        {
            if (item.TypeResource == resourceType)
            {
                flag = true;
            }
        }
        if (flag == true)
        {
            return false;
        }
        else
        {
            //GameObject myGameObject = new GameObject();
            //myGameObject.transform.SetParent(GameObject.Find("Virtual_" + minorIsland.nameMinorIsland).transform);
            Resource res = ScriptableObject.CreateInstance<Resource>();
            res.init(resourceType, name, quantity, production);
            this.Resources.Add(res);
            return true;
        }
    }
    public bool changeResourceProduction(TypeResource resourceType, int value)
    {
        Resource resource = this.getResource(resourceType);
        bool result = resource.changeProduction(value);
        if(result)
        {
            this.building.minorIsland.resourceManager.changeResourceProduction(resourceType, value);
        }
        return result;
    }
    //Resource stock at 0 every time (ressource stock managed in resource manager of the island)
    //public bool changeResourceStock(TypeResource resourceType, int value)
    //{
    //    Resource resource = this.getResource(resourceType);
    //    bool result = resource.changeStock(value);
    //    return result;
    //}
    //public bool checkWithdrawPossibility(TypeResource resourceType, int value)
    //{
    //    return this.getResource(resourceType).checkChangeStockPossibility(value);
    //}
    public Resource getResource(TypeResource resourceType)
    {
        foreach (Resource item in this.Resources)
        {
            Debug.Log("+++ " + item.Name);
            if (item.TypeResource == resourceType)
            {
                return item;
            }
        }
        return null;
    }

    IEnumerator updateStocks()
    {
        for (;;)
        {
            foreach (Resource res in this.Resources)
            {
                //this.building.resourceManager.changeResourceStock(res.TypeResource, res.Production);
                res.changeStock(res.Production);
                //Debug.Log("Building : " + this.building.name + "\tProduction : " + res.Production + "\tStock  : " + res.Name + " : " + res.Stock);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}

