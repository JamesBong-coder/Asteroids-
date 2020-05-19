using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private GameObject Player;
    public GameObject Explotion;
    public float speed;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");    
    }


    void Update()
    {
        if (Player != null)
        {
            transform.LookAt(Player.transform);
            transform.Translate(0, 0, speed * Time.deltaTime); //преследование за игроком
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Asteroid") return;
        if (other.tag == "Player")
        {
            GameObject exp = Instantiate(Explotion, other.transform.position, Quaternion.identity) as GameObject;
            exp.transform.localScale *= 4;
        }
        if (other.tag != "Laser")
            Destroy(other.gameObject);
        Instantiate(Explotion, transform.position, Quaternion.identity);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ScoreUP(50);
        Destroy(gameObject);


    }
}
