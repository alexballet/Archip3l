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
            //rotation of image according to the place of the island
            char id = tuto_minorIsland.nameTuto_MinorIsland[tuto_minorIsland.nameTuto_MinorIsland.Length - 1];
            if (id == '1' || id == '2')
                building.transform.Rotate(Vector3.forward * 180);
            tuto_minorIsland.harborBuilt = true;
            return true;
        }
        else
        {
            Debug.Log("toto");
        }
        return false;
    }

    public IEnumerator destroyBuilding()
    {
        GameObject objectToDestroy = GameObject.Find(this.tuto_minorIsland.nameTuto_MinorIsland + "_Harbor");
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
        tuto_minorIsland.harborRemoved = true;
    }
}
