using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotation;
    public float MaxSpeed, MinSpeed;
    public GameObject Explotion;
    Rigidbody asteroid;
    [HideInInspector]
    public Vector3 Direction;
    [HideInInspector]
    public bool IsBig;
    private Vector3 vel;
    private GameObject Aster;

    void Start()
    {
        Aster = gameObject;
        asteroid = GetComponent<Rigidbody>();
        asteroid.angularVelocity = Vector3.up * Random.Range(rotation * -1, rotation); ;//угловая скорость
        vel = Random.Range(MinSpeed, MaxSpeed) * new Vector3(Random.Range(Direction.x - 1, Direction.x + 1), 0, Random.Range(Direction.z - 1, Direction.z + 1));//направление и скорость астероида
        asteroid.velocity = vel;
        if (!IsBig)
            transform.localScale = Vector3.one * 0.4f;
        else
            transform.localScale = Vector3.one * 1.2f;

    }

    // Update is called once per frame
    void Update()
    {
        asteroid.velocity = vel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Asteroid" || other.tag == "GameBox" || other.tag == "Enemy")
        {
            return;
        }
        if(other.tag == "Player")
        {
            GameObject exp = Instantiate(Explotion, other.transform.position, Quaternion.identity) as GameObject;
            exp.transform.localScale *= 4;
        }
        if(other.tag!="Laser")
            Destroy(other.gameObject);
        if (IsBig)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject MiniAster = Instantiate(Aster, transform.position, Quaternion.identity) as GameObject;
                MiniAster.GetComponent<AsteroidScript>().Direction = Vector3.zero;
                MiniAster.GetComponent<AsteroidScript>().MinSpeed *= 3;
                MiniAster.GetComponent<AsteroidScript>().MaxSpeed *= 3;
                MiniAster.GetComponent<AsteroidScript>().IsBig = false;
            }
            GameObject exp = Instantiate(Explotion, transform.position, Quaternion.identity) as GameObject;
            exp.transform.localScale *= 4;
        }
        else
            Instantiate(Explotion, transform.position, Quaternion.identity);
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().ScoreUP(10);
        Destroy(gameObject);
    }
}
