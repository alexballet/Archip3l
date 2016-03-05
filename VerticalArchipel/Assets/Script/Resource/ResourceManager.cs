using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ResourceManager : MonoBehaviour
{
    public List<Resource> Resources;
    public event EventHandler<ChangeResourceStockEventArgs> ChangeResourceStockEvent;

    private Client client;

    void Awake()
    {
        this.Resources = new List<Resource>();
        this.client = GameObject.Find("Network").GetComponent<Client>();
        this.client.MessageResourceStockUpdateEvent += ((sender, e) => changeResourceStock(sender, e, (TypeResource)Enum.Parse(typeof(TypeResource), (string)e.message.Split('@').GetValue(2)), Int32.Parse((string)e.message.Split('@').GetValue(3))));

        //Add all resources
        foreach (TypeResource resourceType in Enum.GetValues(typeof(TypeResource)))
        {
            this.addResource(resourceType, 0);
        }
    }

    public bool addResource(TypeResource resourceType, int quantity)
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
    public bool changeResourceStock(object sender, EventArgs e, TypeResource resourceType, int value)
    {
        Resource resource = this.getResource(resourceType);
        bool result = false;
        if (resource != null)
        {
            result = resource.changeStock(value);
        }
        else
        {
            if (value >= 0)
            {
                this.addResource(resourceType, value);
                result = true;
            }
            else
            {
                return false;
            }

        }
        if(this.ChangeResourceStockEvent != null)
        {

            this.ChangeResourceStockEvent(this, new ChangeResourceStockEventArgs { resourceType = resourceType, stock = resource.Stock });
        }
        return result;
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
}
public class ChangeResourceStockEventArgs : EventArgs
{
    public TypeResource resourceType;
    public int stock;
}