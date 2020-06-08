using System.Collections;
using System.Drawing;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject LaserPrefab;
    [HideInInspector]
    public List<BulletView> BView;
    [HideInInspector]
    public GameObject laser;

    public GameObject Fire;
    public AudioClip[] clips;
    private AudioSource aud;


    public void Start()
    {
        aud = GetComponent<AudioSource>();
        BView = new List<BulletView>();
        Fire.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetAxis("Vertical") > 0)
            Fire.SetActive(true);
        else
            Fire.SetActive(false);
    }

    public void Move(PointF Pos)
    {
        transform.position = new Vector3(Pos.X, 1, Pos.Y);
    }

    public void Rotate(float angle)
    {
        transform.rotation = Quaternion.Euler(0, -angle, 0); //поворот корабля
    }

    public void Shoot(PointF Pos, float angle)
    {
        GameObject bullet = Instantiate(BulletPrefab, new Vector3(Pos.X, 1, Pos.Y), Quaternion.Euler(0, -angle, 0));
        BView.Add(bullet.GetComponent<BulletView>());
        aud.clip = clips[0];
        aud.Play();
    }     
   
    public void MoveBullets(List<PointF> Pos)
    {
        for (int i = 0; i < BView.Count; i++)
        {
            PointF p = Pos[i];
            BView[i].Move(p);
        }
    }

    public void CheckBullets(List<bool> isDead)
    {
        for (int i = 0; i < BView.Count; i++)
        {
            if (isDead[i])
            {
                BView[i].DestroyBullet();
                BView.RemoveAt(i);
                isDead.RemoveAt(i);
                i--;
            }
        }
    }

    public void LaserShoot(PointF pos, float angle)
    {
        laser = Instantiate(LaserPrefab, new Vector3(pos.X, 1, pos.Y), Quaternion.Euler(0, -angle, 0)) as GameObject;
        aud.clip = clips[1];
        aud.Play();
    }

    public void DestroyLaser()
    {
        Destroy(laser);
    }
}
