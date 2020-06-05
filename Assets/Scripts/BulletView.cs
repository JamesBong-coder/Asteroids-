using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    public void Move(float[] Pos)
    {
        transform.position = new Vector3(Pos[0], 1, Pos[1]);
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
