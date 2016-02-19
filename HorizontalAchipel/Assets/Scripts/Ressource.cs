using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ressource
{
    public string name;
    public int stock;

    public Ressource(string argName)
    {
        name = argName;
        stock = 0;
    }

    public Ressource(string argName, int quantity)
    {
        name = argName;
        stock = quantity;
    }

}
