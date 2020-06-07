using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsterView : MonoBehaviour
{
    public bool isBig;

    void Start()
    {
        if (isBig)
            transform.localScale *= 3;
    }

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
