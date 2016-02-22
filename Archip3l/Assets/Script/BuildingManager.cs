using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : ScriptableObject {

    public MinorIsland minorIsland { get; private set; }
    public List<Building> BuildingList { get; private set; }

    public void init(MinorIsland island)
    {
        this.minorIsland = island;
    }

    public Building getBuilding(TypeBuilding buildingType)
    {
        foreach (Building item in this.BuildingList)
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
            Building newBuilding = new Building(buildingType, this.minorIsland);
            this.BuildingList.Add(newBuilding);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool destroyBuilding(TypeBuilding buildingType)
    {
        return (this.BuildingList.Remove(this.getBuilding(buildingType)));
    }

}
