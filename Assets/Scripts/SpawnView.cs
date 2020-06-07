using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnView : MonoBehaviour
{
    public GameObject AsterPrefab;
    public GameObject EnemyPrefab;

    public List<AsterView> asteroids;
    public List<EnemyView> enemies;

    private void Start()
    {
        asteroids = new List<AsterView>();
        enemies = new List<EnemyView>(); 
    }

    public void SpawnAster(float[] pos, bool isBig)
    {
        GameObject aster = Instantiate(AsterPrefab, new Vector3(pos[0], 1, pos[1]), Quaternion.identity);
        aster.GetComponent<AsterView>().isBig = isBig;
        asteroids.Add(aster.GetComponent<AsterView>());
    }

    public void SpawnEnemy(float[] pos)
    {
        GameObject enemy = Instantiate(EnemyPrefab, new Vector3(pos[0], 1, pos[1]), Quaternion.identity);
        enemies.Add(enemy.GetComponent<EnemyView>());
    }

    public void MoveAster(List<float[]> pos)
    {
        for(int i=0; i < pos.Count; i++)
            asteroids[i].Move(pos[i]);
    }

    public void MoveEnemy(List<float[]> pos)
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
                asteroids.RemoveAt(i);
                check.RemoveAt(i);
                i--;
            }
        }
    }
}
