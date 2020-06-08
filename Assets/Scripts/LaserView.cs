using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine;

public class LaserView : MonoBehaviour
{
    public void Move(PointF pos, float angle)
    {
        transform.position = new Vector3(pos.X, 1, pos.Y);
        transform.rotation = Quaternion.Euler(0, -angle, 0);
    }
}
