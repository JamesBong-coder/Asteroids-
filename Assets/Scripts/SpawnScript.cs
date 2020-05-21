using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids;

public class SpawnScript : MonoBehaviour
{
    public GameObject Asteroid;
    public GameObject Enemy;

    public int minDelay = 1, maxDelay = 4; //for Asteroids
    private float NextAster;

    public GameObject[] Spawn; // _____  стены спавна
                               //|  1  |
                               //|2   3|
                               //|__0__|
    System.Random rnd; //рандом для выборастены
    private Vector3 NewPos;
    private int SpawnInd;

    public float DelayEnemy = 10f; //for Enemy

    private SpawnStatus status;

    void Start()
    {
        status = new SpawnStatus(minDelay, maxDelay, DelayEnemy);
    }

    public void Update()
    {
        if(status.CheckAster()) SpawnAster();
        if (status.CheckEnemy()) SpawnEnemy();
    }

    private void SpawnAster()
    {
        GetPosition();
        int isB = status.GetSpawnIndex();

        GameObject Aster = Instantiate(Asteroid, NewPos, Spawn[SpawnInd].transform.rotation) as GameObject;
        if (isB == 3) Aster.GetComponent<AsteroidScript>().IsBig = true;
        Aster.GetComponent<AsteroidScript>().Direction = Spawn[SpawnInd].transform.forward; // задаем направление для нашего астероида которое зависит от стены  
        status.UpdateAsterTime();
        
    }

    private void SpawnEnemy()
    {
        GetPosition();
        Instantiate(Enemy, NewPos, Spawn[SpawnInd].transform.rotation);
        status.UpdateEnemyTime();
    }

    private void GetPosition()
    {
        SpawnInd = status.GetSpawnIndex(); //выбираем случаюную из стен, создаем на ее области астероид и передаем ему направление движения
        float position = UnityEngine.Random.Range(-Spawn[SpawnInd].transform.localScale.x / 2, Spawn[SpawnInd].transform.localScale.x / 2);
        if (SpawnInd == 0 || SpawnInd == 1)
            NewPos = new Vector3(position, 1, Spawn[SpawnInd].transform.position.z);
        else
            NewPos = new Vector3(Spawn[SpawnInd].transform.position.x, 1, position);
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}   
        