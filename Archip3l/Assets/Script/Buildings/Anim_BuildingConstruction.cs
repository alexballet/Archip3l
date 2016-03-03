using UnityEngine;
using System.Collections;

public class Anim_BuildingConstruction : MonoBehaviour
{
    public Transform hammerPrefab;

    private Quaternion startQuaternion;
    private float interval;

    void Start()
    {
        this.startQuaternion = transform.GetChild(0).rotation;
        this.interval = 0.02f;

        //Instantiate(hammerPrefab).SetParent(this.transform);
        StartCoroutine("Rotate");
    }
    IEnumerator Rotate()
    {
        for (;;)
        {
            SoundPlayer.Instance.playConstructionSound();
            for (int i = 0; i < 23; i++)
            {
                RotateLeft();
                yield return new WaitForSeconds(this.interval);
            }
            RotateInit();
            yield return new WaitForSeconds(this.interval);
        }

    }
    void RotateLeft()
    {
        transform.GetChild(0).transform.Rotate(Vector3.forward * 2);
    }
    void RotateInit()
    {
        transform.GetChild(0).transform.rotation = this.startQuaternion;
    }
}
