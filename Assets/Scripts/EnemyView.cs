using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.up);
    }

    public void Move(PointF pos)
    {
        transform.position = new Vector3(pos.X, 1, pos.Y);
    }

    public void DestroyAster()
    {
        Destroy(gameObject);
    }
}
