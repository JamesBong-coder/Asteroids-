using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.up);
    }

    public void Move(float[] pos)
    {
        transform.position = new Vector3(pos[0], 1, pos[1]);
    }

    public void DestroyAster()
    {
        Destroy(gameObject);
    }
}
