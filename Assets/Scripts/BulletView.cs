using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    public void Move(PointF Pos)
    {
        transform.position = new Vector3(Pos.X, 1, Pos.Y);
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
