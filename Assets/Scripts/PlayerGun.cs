using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public GameObject Gun;
    public GameObject BulletPrefab;
    public GameObject LaserPrefab;
    public GameObject FireSpaceShip;

    public AudioClip[] clips;
    private AudioSource aud;

    private float nextBullet;
    private float nextLaser;
    public float DelayBullet;
    public float DelayLaser;
    [HideInInspector]
    public float LaserMagazine;


    void Start()
    {
        aud = GetComponent<AudioSource>();
        nextBullet = Time.time;
        nextLaser = Time.time;
        LaserMagazine = 100;
    }


    void Update()
    {
        if (Input.GetAxis("Vertical") > 0) //огонь из турбин космического корабля
            FireSpaceShip.SetActive(true);
        else FireSpaceShip.SetActive(false);

        if (Input.GetButton("Fire1") && nextBullet < Time.time)//выстрел из пушки
        {
            Shoot();
        }
        if (Input.GetButton("Fire2") && nextLaser < Time.time && LaserMagazine >= 25)
        {
            LaserShoot(); //выстрел Лазером
        }
        if(LaserMagazine<100)
            LaserMagazine += 0.1f;
    }

    public void Shoot()
    {
        GameObject Bullet = Instantiate(BulletPrefab, Gun.transform.position, Gun.transform.rotation) as GameObject;
        Rigidbody run = Bullet.GetComponent<Rigidbody>();
        run.AddForce(Bullet.transform.forward * 10, ForceMode.Impulse);
        nextBullet = Time.time + DelayBullet;
        aud.clip = clips[0];
        aud.Play();
    }

    public void LaserShoot()
    {
        GameObject Laser = Instantiate(LaserPrefab, Gun.transform.position, Gun.transform.rotation) as GameObject;
        Laser.transform.parent = Gun.transform;
        Destroy(Laser, 0.2f);
        nextLaser = Time.time + DelayLaser;
        LaserMagazine -= 25;
        Debug.Log(LaserMagazine);
        aud.clip = clips[1];
        aud.Play();
    }
}
