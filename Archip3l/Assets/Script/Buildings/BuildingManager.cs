using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class BuildingManager : MonoBehaviour
{

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
                this.buildingList.Add(building);

                Vector3 newPosition;
                if (buildingType != TypeBuilding.Harbor)
                {
                    newPosition = Camera.main.ScreenToWorldPoint(position);
                    newPosition.z = -1;
                }
                else
                    newPosition = position;
                //Vector3 newPosition = Camera.main.ScreenToWorldPoint(position);
                //newPosition.z = -1;
                building.transform.position = newPosition;
                //rotation of image according to the place of the island
                char id = minorIsland.nameMinorIsland[minorIsland.nameMinorIsland.Length - 1];
                if (id == '1' || id == '2')
                    building.transform.Rotate(Vector3.forward * 180);
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator destroyBuilding(TypeBuilding buildingType)
    {
        Building buildingToDestroy = this.getBuilding(buildingType);
        GameObject objectToDestroy = GameObject.Find(this.minorIsland.nameMinorIsland + "_" + buildingType.ToString());
        //update resource production in island resource manager
        this.minorIsland.resourceManager.changeResourceProduction(buildingToDestroy.resourceProduced.TypeResource, -buildingToDestroy.quantityProduced);
        buildingToDestroy.quantityProduced = 0;
        //remove the building in the list of this manager
        this.buildingList.Remove(this.getBuilding(buildingType));
        //Delete game object
        yield return StartCoroutine(fadeAndDestroy(objectToDestroy));
    }
    IEnumerator fadeAndDestroy(GameObject obj)
    {
        Color color;
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.001f);

            color = obj.GetComponent<SpriteRenderer>().material.color;
            color.a -= 0.01f;
            obj.GetComponent<SpriteRenderer>().material.color = color;
        }
        Destroy(obj);
    }
}
