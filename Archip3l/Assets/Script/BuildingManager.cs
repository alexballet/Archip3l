using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

    public MinorIsland minorIsland { get; private set; }
    public List<Building> buildingList { get; private set; }

    public void init(MinorIsland island)
    {
        this.minorIsland = island;
        buildingList = new List<Building>();

        //test
        createBuilding(TypeBuilding.Ferme);
    }

    public Building getBuilding(TypeBuilding buildingType)
    {
        foreach (Building item in this.buildingList)
        {
            if (item.TypeBuilding == buildingType)
            {
                return item;
            }
        }
        return null;
    }

    public bool createBuilding(TypeBuilding buildingType)
    {
        if (this.getBuilding(buildingType) == null)
        {
            Building newBuildingPrefab = Resources.Load<Building>("Prefab/Building");      //new Building(buildingType, this.minorIsland);
            Building newBuilding = Instantiate(newBuildingPrefab);
            newBuilding.transform.SetParent(GameObject.Find(minorIsland.nameMinorIsland).transform);
            this.buildingList.Add(newBuilding);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool destroyBuilding(TypeBuilding buildingType)
    {
        return (this.buildingList.Remove(this.getBuilding(buildingType)));
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
