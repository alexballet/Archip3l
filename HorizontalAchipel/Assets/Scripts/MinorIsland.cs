using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinorIsland : Island
{
    public string canvasName;

    public MinorIsland(int argId) : base()  //initialize ressources & buildings from Island constructor
    {
        id = argId;
        canvasName = "sous_ile_" + argId.ToString();
    }
}