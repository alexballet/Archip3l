using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour{

    public TypeResource TypeResource { get; private set; }
    public string Name { get; private set; }
    public int Stock { get; private set; }
    public int Production { get; private set; }

    public void init(TypeResource TypeResource, string argName)
    {
        this.TypeResource = TypeResource;
        this.Name = argName;
        this.Stock = 0;
        this.Production = 0;
    }
    public void init(TypeResource TypeResource, string argName, int quantity)
    {
        init(TypeResource, argName);
        if (quantity > 0)
        {
            this.Stock = quantity;
        }
    }
    public void init(TypeResource TypeResource, string argName, int quantity, int production)
    {
        init(TypeResource, argName, quantity);
        this.Production = production;
    }

    public bool changeProduction(int value)
    {
        if (this.Production + value >= 0)
        {
            this.Production += value;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool changeStock(int value)
    {

        if (this.Stock + value >= 0)
        {
            this.Stock += value;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool checkChangeProductionPossibility(int value)
    {
        return this.Production + value >= 0;
    }
    public bool checkChangeStockPossibility(int value)
    {
        return this.Stock + value >= 0;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
