using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinorIsland : Island
{
    public string canvasName;

    public static MinorIsland CreateComponent(GameObject where, int argId)
    {
        MinorIsland minorIsland = where.AddComponent<MinorIsland>();

        minorIsland.buildings = new List<Building>();
        minorIsland.ressources = new List<Ressource>();

        minorIsland.id = argId;
        minorIsland.canvasName = "sous_ile_" + argId.ToString();
        return minorIsland;
    }
}   