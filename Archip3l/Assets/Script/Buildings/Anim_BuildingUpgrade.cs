using UnityEngine;
using System.Collections;

public class Anim_BuildingUpgrade : MonoBehaviour
{
    private float interval;

    void Start()
    {
        this.interval = 0.01f;
        StartCoroutine("Move");
    }

    IEnumerator Move()
    {
        for (;;)
        {
            SoundPlayer.Instance.playUpgradeSound();
            for (int i = 0; i < 7; i++)
            {

                MoveUp();
                yield return new WaitForSeconds(this.interval);
                MoveLeft();
                yield return new WaitForSeconds(this.interval);
                MoveDown();
                yield return new WaitForSeconds(this.interval);
                MoveRight();
                yield return new WaitForSeconds(this.interval);
            }
        }
    }
    void MoveUp()
    {
        Vector3 pos = transform.position;
        pos.y += 1;
        transform.GetChild(0).transform.position = pos;
    }
    void MoveDown()
    {
        Vector3 pos = transform.position;
        pos.y -= 1;
        transform.GetChild(0).transform.position = pos;
    }
    void MoveLeft()
    {
        Vector3 pos = transform.position;
        pos.x -= 1;
        transform.GetChild(0).transform.position = pos;
    }
    void MoveRight()
    {
        Vector3 pos = transform.position;
        pos.x += 1;
        transform.GetChild(0).transform.position = pos;
    }
}
