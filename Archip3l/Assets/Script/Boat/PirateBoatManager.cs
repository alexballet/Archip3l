using UnityEngine;
using System.Collections;
using TouchScript.InputSources;

public class PirateBoatManager : MonoBehaviour
{
    private float secondsBeforeFirstBoat = 10;
    private float interval = 1f;
    private float raisingRate = 0.99f;
    private int boatId = 0;

    public Transform pirateBoatPrefab;

    private MinorIsland island1;
    private MinorIsland island2;
    private MinorIsland island3;
    private MinorIsland island4;

    private Vector3 targetPosition;
    private Vector3 initPosition;

    private static System.Random rnd;

    void Awake()
    {
        rnd = new System.Random();
        this.island1 = GameObject.Find("sous_ile_1").GetComponent<MinorIsland>();
        this.island2 = GameObject.Find("sous_ile_2").GetComponent<MinorIsland>();
        this.island3 = GameObject.Find("sous_ile_3").GetComponent<MinorIsland>();
        this.island4 = GameObject.Find("sous_ile_4").GetComponent<MinorIsland>();
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
        pirateBoatTransform.name = "PirateBoat_" + this.boatId;
        MovePirateBoat pirateBoat = pirateBoatTransform.GetComponent<MovePirateBoat>();
        if (pirateBoat != null)
        {
            pirateBoat.init(this.initPosition, this.targetPosition);

        }
    }
    private Vector3 getNewBoatPosition()
    {
        switch (rnd.Next(1, 5))
        {
            case 1:
                return new Vector3(rnd.Next(-500, -430), rnd.Next(0, 400), -7);
            case 2:
                return new Vector3(rnd.Next(430, 500), rnd.Next(0, 400), -7);
            case 4:
                return new Vector3(rnd.Next(-500, -430), rnd.Next(-400, 0), -7);
            case 3:
                return new Vector3(rnd.Next(430, 500), rnd.Next(-400, 0), -7);
        }
        return new Vector3(0, 0);
    }
}
