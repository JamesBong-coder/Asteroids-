using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject Asteroid;
    public GameObject Enemy;

    public float minDelay = 1f, maxDelay = 4f; //for Asteroids
    private float NextAster;

    public GameObject[] Spawn; // _____  стены спавна
                               //|  1  |
                               //|2   3|
                               //|__0__|
    System.Random rnd; //рандом для выборастены
    private Vector3 NewPos;
    private int SpawnInd;

    public float DelayEnemy = 10f; //for Enemy
    private float NextEnemy;
    void Start()
    {
        rnd = new System.Random();
        NextAster = Time.time;
        NextEnemy = Time.time + DelayEnemy;
    }

    public void Update()
    {
        if(NextAster < Time.time) SpawnAster();
        if (NextEnemy < Time.time) SpawnEnemy();
    }

    private void SpawnAster()
    {
        GetPosition();
        int isBig = rnd.Next(0, 5);
        GameObject Aster = Instantiate(Asteroid, NewPos, Spawn[SpawnInd].transform.rotation) as GameObject;

        if (isBig == 4)
            Aster.GetComponent<AsteroidScript>().IsBig = true;
        else
            Aster.GetComponent<AsteroidScript>().IsBig = false;
        Aster.GetComponent<AsteroidScript>().Direction = Spawn[SpawnInd].transform.forward; // задаем направление для нашего астероида которое зависит от стены  
        NextAster = Time.time + UnityEngine.Random.Range(minDelay, maxDelay);
    }

    private void SpawnEnemy()
    {
        GetPosition();
        Instantiate(Enemy, NewPos, Spawn[SpawnInd].transform.rotation);
        NextEnemy = Time.time + DelayEnemy;
    }

    private void GetPosition()
    {
        SpawnInd = rnd.Next(0, 4); //выбираем случаюную из стен, создаем на ее области астероид и передаем ему направление движения
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
        