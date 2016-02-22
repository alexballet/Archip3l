using UnityEngine;
using System.Collections;

public class Anim_BuildingConstruction : MonoBehaviour
{
    private Quaternion startQuaternion;
    private float interval;

    void Start()
    {
        this.startQuaternion = transform.rotation;
        this.interval = 0.5f;
    }

    void Update()
    {
        if (Input.GetKeyDown("left"))
        {
            StartCoroutine("Rotate");
        }
    }

    IEnumerator Rotate()
    {
        for (;;)
        {
            for (int i = 0; i < 2; i++)
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
        transform.Rotate(Vector3.forward * 20);
    }
    void RotateInit()
    {
        transform.rotation = this.startQuaternion;
    }
}
