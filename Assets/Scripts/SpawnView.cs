using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine;

public class SpawnView : MonoBehaviour
{
    public GameObject AsterPrefab;
    public GameObject EnemyPrefab;

    private List<AsterView> asteroids;
    private List<EnemyView> enemies;

    private void Start()
    {
        asteroids = new List<AsterView>();
        enemies = new List<EnemyView>(); 
    }

    public void SpawnAster(PointF pos, bool isBig)
    {
        GameObject aster = Instantiate(AsterPrefab, new Vector3(pos.X, 1, pos.Y), Quaternion.identity);
        aster.GetComponent<AsterView>().isBig = isBig;
        asteroids.Add(aster.GetComponent<AsterView>());
    }

    public void SpawnEnemy(PointF pos)
    {
        GameObject enemy = Instantiate(EnemyPrefab, new Vector3(pos.X, 1, pos.Y), Quaternion.identity);
        enemies.Add(enemy.GetComponent<EnemyView>());
    }

    public void MoveAster(List<PointF> pos)
    {
        for(int i=0; i < pos.Count; i++)
            asteroids[i].Move(pos[i]);
    }

    public void MoveEnemy(List<PointF> pos)
    {
        for (int i = 0; i < pos.Count; i++)
            enemies[i].Move(pos[i]);
    }

    public void CheckAster(List<bool> check)
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            if (check[i])
            {
                asteroids[i].DestroyAster();
                if (asteroids[i].isBig)
                {
                    for (int J = 0; J < 5; J++)
                        SpawnAster(new PointF(asteroids[i].transform.position.x, asteroids[i].transform.position.z), false);
                }
                asteroids.RemoveAt(i);
                check.RemoveAt(i);
                i--;
            }
        }
    }

    public void CheckEnemy(List<bool> check)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (check[i])
            {
                enemies[i].DestroyEnemy();
                enemies.RemoveAt(i);
                check.RemoveAt(i);
                i--;
            }
        }
    }
}
