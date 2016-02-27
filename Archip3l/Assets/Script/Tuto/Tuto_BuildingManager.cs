using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tuto_BuildingManager : MonoBehaviour {

    public Tuto_MinorIsland tuto_minorIsland { get; private set; }
    public Transform buildingPrefab;
    public Tuto_Building tuto_building { get; private set; }

    public void init(Tuto_MinorIsland island)
    {
        this.tuto_minorIsland = island;
    }
    

    public bool createBuilding(Vector3 position)
    {
        var buildingTransform = Instantiate(buildingPrefab) as Transform;
        Tuto_Building building = buildingTransform.GetComponent<Tuto_Building>();
        if (building != null)
        {
            building.init(this.tuto_minorIsland);
            building.transform.SetParent(this.transform);
            tuto_building = building;

            Vector3 newPosition = Camera.main.ScreenToWorldPoint(position);
            newPosition.z = -1;
            building.transform.position = newPosition;
            return true;
        }
        else
        {
            Debug.Log("toto");
        }
        return false;
    }

    public bool destroyBuilding(TypeBuilding buildingType)
    {
        GameObject.Find(this.tuto_minorIsland.nameTuto_MinorIsland + "_" + buildingType.ToString());
        return tuto_building;
    }
}
