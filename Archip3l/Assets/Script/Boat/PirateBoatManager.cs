using UnityEngine;
using System.Collections;

public class PirateBoatManager : MonoBehaviour
{
    private float secondsBeforeFirstBoat = 10;
    private float interval = 1;
    private float raisingRate = 0.99f;
    private int boatId = 0;

    public Transform pirateBoatPrefab;

    private TouchScript.Examples.Cube.MinorIsland island1;
    private TouchScript.Examples.Cube.MinorIsland island2;
    private TouchScript.Examples.Cube.MinorIsland island3;
    private TouchScript.Examples.Cube.MinorIsland island4;

    private Vector3 targetPosition;
    private Vector3 initPosition;
    //private Vector2 initDirection;
    //private Quaternion initRotation;
    //private float boatScale;
    //private float boatSpeed;

    private static System.Random rnd;

    void Awake()
    {
        rnd = new System.Random();
        this.island1 = GameObject.Find("sous_ile_1").GetComponent<TouchScript.Examples.Cube.MinorIsland>();
        this.island2 = GameObject.Find("sous_ile_2").GetComponent<TouchScript.Examples.Cube.MinorIsland>();
        this.island3 = GameObject.Find("sous_ile_3").GetComponent<TouchScript.Examples.Cube.MinorIsland>();
        this.island4 = GameObject.Find("sous_ile_4").GetComponent<TouchScript.Examples.Cube.MinorIsland>();
    }
    void Start()
    {
        StartCoroutine("StartLaunchingPirateBoats");
    }
    IEnumerator StartLaunchingPirateBoats()
    {
        for(;;)
        {
            launchPirateBoat();

            this.boatId += 1;
            this.interval *= raisingRate;
            yield return new WaitForSeconds(this.interval);
        }
    }
    private void launchPirateBoat()
    {
        //System.Random ran = new System.Random();
        switch (rnd.Next(1, 5))
        {
            case 1:
                this.targetPosition = new Vector3(this.island1.GetComponent<MeshCollider>().bounds.center.x, this.island1.GetComponent<MeshCollider>().bounds.center.y, 0); break;
            case 2:
                this.targetPosition = new Vector3(this.island2.GetComponent<MeshCollider>().bounds.center.x, this.island2.GetComponent<MeshCollider>().bounds.center.y, 0); break;
            case 3:
                this.targetPosition = new Vector3(this.island3.GetComponent<MeshCollider>().bounds.center.x, this.island3.GetComponent<MeshCollider>().bounds.center.y, 0); break;
            case 4:
                this.targetPosition = new Vector3(this.island4.GetComponent<MeshCollider>().bounds.center.x, this.island4.GetComponent<MeshCollider>().bounds.center.y, 0); break;
        }

        //pirateBoat.transform.SetParent(this.transform);
        this.initPosition = getNewBoatPosition(); // Camera.main.ScreenToWorldPoint(new Vector3(0, 100, -4));

        var pirateBoatTransform = Instantiate(pirateBoatPrefab) as Transform; //, initPosition, initRotation) as Transform;
        pirateBoatTransform.name = "PrateBoat_" + this.boatId;
        MovePirateBoat pirateBoat = pirateBoatTransform.GetComponent<MovePirateBoat>();
        if (pirateBoat != null)
        {
            pirateBoat.init(this.initPosition, this.targetPosition);

        }
    }
    private Vector3 getNewBoatPosition()
    {
        //System.Random rnd = new System.Random();
        switch (rnd.Next(1, 5))
        {
            case 1:
                return new Vector3(rnd.Next(-500, -430), rnd.Next(0, 400), 0);
            case 2:
                return new Vector3(rnd.Next(430, 500), rnd.Next(0, 400), 0);
            case 4:
                return new Vector3(rnd.Next(-500, -430), rnd.Next(-400, 0), 0);
            case 3:
                return new Vector3(rnd.Next(430, 500), rnd.Next(-400, 0), 0);
        }
        return new Vector3(0, 0);
    }
}
