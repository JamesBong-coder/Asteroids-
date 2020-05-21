using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids;

public class PlayerGun : MonoBehaviour
{
    public GameObject Gun;
    public GameObject BulletPrefab;
    public GameObject LaserPrefab;
    public GameObject FireSpaceShip;

    public AudioClip[] clips;
    private AudioSource aud;

    public float DelayBullet;
    public float DelayLaser;
    public float LaserMagazine = 100;

    public PlayerGunStatus status;

    void Start()
    {
        status = new PlayerGunStatus(DelayBullet, DelayLaser, LaserMagazine);
        aud = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (Input.GetAxis("Vertical") > 0) //огонь из турбин космического корабля
            FireSpaceShip.SetActive(true);
        else FireSpaceShip.SetActive(false);

        if (Input.GetButton("Fire1") && status.CheckShootTime())//выстрел из пушки
        {
            Shoot();
        }
        if (Input.GetButton("Fire2") && status.CheckMagazine(25))
        {
            LaserShoot(); //выстрел Лазером
        }
        status.LaserMagazineUP(0.1f);
    }

    public void Shoot()
    {
        GameObject Bullet = Instantiate(BulletPrefab, Gun.transform.position, Gun.transform.rotation) as GameObject;
        Rigidbody run = Bullet.GetComponent<Rigidbody>();
        run.AddForce(Bullet.transform.forward * 10, ForceMode.Impulse);
        status.UpdateShootTime();
        aud.clip = clips[0];
        aud.Play();
    }

    public void LaserShoot()
    {
        GameObject Laser = Instantiate(LaserPrefab, Gun.transform.position, Gun.transform.rotation) as GameObject;
        Laser.transform.parent = Gun.transform;
        Destroy(Laser, 0.2f);
        status.LaserMagazineDown(25);
        status.UpdateLaserTime();
        aud.clip = clips[1];
        aud.Play();
    }
}
