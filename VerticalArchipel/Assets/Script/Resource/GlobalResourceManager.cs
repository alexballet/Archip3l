using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class GlobalResourceManager : MonoBehaviour
{
    public List<Resource> Resources;
    public List<ResourceManager> ResourceManagers;

    void Awake()
    {
        this.Resources = new List<Resource>();

        this.ResourceManagers.Add(GameObject.Find("sous_ile_1").GetComponent<ResourceManager>());
        this.ResourceManagers.Add(GameObject.Find("sous_ile_2").GetComponent<ResourceManager>());
        this.ResourceManagers.Add(GameObject.Find("sous_ile_3").GetComponent<ResourceManager>());
        this.ResourceManagers.Add(GameObject.Find("sous_ile_4").GetComponent<ResourceManager>());

        foreach(ResourceManager rm in this.ResourceManagers)
        {
            rm.ChangeResourceStockEvent += _ChangeResourceStockEvent;
            rm.ChangeResourceProductionEvent += _ChangeResourceProductionEvent;
        }

        //Add all resources
        foreach (TypeResource resourceType in Enum.GetValues(typeof(TypeResource)))
        {
            this.addResource(resourceType, 0);
        }
    }

    private void _ChangeResourceProductionEvent(object sender, ChangeResourceProductionEventArgs e)
    {
        Resource resource = this.getResource(e.resourceType);
        if (resource != null)
        {
            resource.changeProduction(e.production);
        }
        else
        {
            if (e.production >= 0)
            {
                this.addResource(e.resourceType, e.production);
            }
        }
    }
    private void _ChangeResourceStockEvent(object sender, ChangeResourceStockEventArgs e)
    {
        Resource resource = this.getResource(e.resourceType);
        if (resource != null)
        {
            resource.changeStock(e.stock);
        }
        else
        {
            if (e.stock >= 0)
            {
                this.addResource(e.resourceType, e.stock);
            }
        }
    }
    public bool addResource(TypeResource resourceType, int quantity, int production = 0)
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
            Resource res = ScriptableObject.CreateInstance<Resource>();
            res.init(resourceType, quantity);
            this.Resources.Add(res);
            return true;
        }
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
    public void initResources()
    {
        //Sync all resource before the start of the game
        //sub resource manager
        foreach(ResourceManager rm in this.ResourceManagers)
        {
            rm.initResources();
        }

        //Fill this one now
        foreach(ResourceManager rm in this.ResourceManagers)
        {
            foreach (Resource resource in rm.Resources)
            {
                Resource resourceGlobalManager = getResource(resource.TypeResource);
                _ChangeResourceStockEvent(null, new ChangeResourceStockEventArgs { resourceType = resourceGlobalManager.TypeResource, stock = resource.Stock });
                _ChangeResourceProductionEvent(null, new ChangeResourceProductionEventArgs { resourceType = resourceGlobalManager.TypeResource, production = resource.Production });
                Debug.Log(resource.TypeResource.ToString() + ">> Stock : " + resource.Stock + " production : " + resource.Production);
            }
        }
    }
}