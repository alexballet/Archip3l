using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

    public MinorIsland minorIsland { get; private set; }
    public Transform buildingPrefab;
    public List<Building> buildingList { get; private set; }

    public void init(MinorIsland island)
    {
        this.minorIsland = island;
        buildingList = new List<Building>();
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

    public bool createBuilding(TypeBuilding buildingType, Vector3 position)
    {
        if (this.getBuilding(buildingType) == null)
        {
            var buildingTransform = Instantiate(buildingPrefab) as Transform;
            Building building = buildingTransform.GetComponent<Building>();
            if (building != null)
            {
                building.init(buildingType, this.minorIsland);
                building.transform.SetParent(this.transform);

                Vector3 newPosition = Camera.main.ScreenToWorldPoint(position);
                newPosition.z = 0;
                building.transform.position = newPosition;
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public bool destroyBuilding(TypeBuilding buildingType)
    {
        //delete the gameobject child (to do)
        return (this.buildingList.Remove(this.getBuilding(buildingType)));
    }
}
