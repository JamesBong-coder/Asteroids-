using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserView : MonoBehaviour
{
    public void Move(float[] pos, float angle)
    {
        transform.position = new Vector3(pos[0], 1, pos[1]);
        transform.rotation = Quaternion.Euler(0, -angle, 0);
    }
}
