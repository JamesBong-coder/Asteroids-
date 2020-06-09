using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public GameObject ExplotionPrefab;

    private void Update()
    {
        transform.Rotate(Vector3.up);
    }

    public void Move(PointF pos)
    {
        transform.position = new Vector3(pos.X, 1, pos.Y);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
        Instantiate(ExplotionPrefab, transform.position, Quaternion.identity);

    }
}
