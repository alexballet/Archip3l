using UnityEngine;
using System.Collections;

public class Resource : ScriptableObject {

    public TypeResource TypeResource { get; private set; }
    public int Stock { get; private set; }

    public void init(TypeResource TypeResource)
    {
        this.TypeResource = TypeResource;
        this.Stock = 0;
    }
    public void init(TypeResource TypeResource, int quantity)
    {
        init(TypeResource);
        if (quantity > 0)
        {
            this.Stock = quantity;
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
}
