using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ResourceManager : MonoBehaviour {

    public MinorIsland minorIsland { get; private set; }
    public List<Resource> Resources;

    public void init(MinorIsland island)
    {
        this.minorIsland = island;

        //this.Resources = new List<Resource>();

        /*this.Resources.Add(ScriptableObject.CreateInstance<Resource>());
        this.Resources[-1].init(TypeResource.Bois, "Bois");
        this.Resources.Add(ScriptableObject.CreateInstance<Resource>());
        this.Resources[-1].init(TypeResource.Bois, "Or");
        this.Resources.Add(ScriptableObject.CreateInstance<Resource>());
        this.Resources[-1].init(TypeResource.Bois, "Métal");
        this.Resources.Add(ScriptableObject.CreateInstance<Resource>());*/
        //this.Resources[-1].init(TypeResource.Bois, "Nourriture");
        
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
            GameObject myGameObject = new GameObject();
            myGameObject.transform.SetParent(GameObject.Find("Virtual_" + minorIsland.nameMinorIsland).transform);
            this.Resources.Add(new Resource(resourceType, name, quantity, production));
            return true;
        }
    }
    public bool changeResourceProduction(TypeResource resourceType, int value)
    {
        Resource resource = this.getResource(resourceType);
        bool result = resource.changeProduction(value);
        return result;
    }
    public bool changeResourceStock(TypeResource resourceType, int value)
    {
        Resource resource = this.getResource(resourceType);
        bool result = resource.changeStock(value);
        return result;
    }
    public bool checkWithdrawPossibility(TypeResource resourceType, int value)
    {
        return this.getResource(resourceType).checkChangeStockPossibility(value);
    }
    public Resource getResource(TypeResource resourceType)
    {
        foreach (Resource item in this.Resources)
        {
            if (item.TypeResource == resourceType)
            {
                return item;
            }
        }
        return null;
    }

    IEnumerator updateStocks()
    {
        for(;;)
        {
            foreach(Resource res in this.Resources)
            {
                res.changeStock(res.Production);
                Debug.Log("Island : " + this.minorIsland + "\tStock  : " + res.Name + " : " + res.Stock);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}

