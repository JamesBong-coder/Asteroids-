using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresenter
{
    public PlayerView View;
    public PlayerModel Model;

    public PlayerPresenter(PlayerView v, PlayerModel m)
    {
        View = v;
        Model = m;
    }
    
    public void Move()
    {
        Model.Move();
        View.Move(Model.GetPos());
    }

    public void Rotate(float dir)
    {
        Model.RotatePlayer(dir);
        View.Rotate(Model.angle);
    }

    public void UpdateAccel()
    {
        Model.UpdateAccel();
    }

    public void Shoot()
    {
        if (Model.Shoot())
        {
            View.Shoot(Model.Gpos, Model.angle);
        }
    }

    public void MoveBullets()
    {
        Model.Gun.MoveBullets();
        View.MoveBullets(Model.Gun.GetBulletsPos());
    }

    public void CheckBullets()
    {
        View.CheckBullets(Model.Gun.GetCheckBullets());
    }

    public void LaserShoot()
    {
        if (Model.LaserShoot())
        {
            View.LaserShoot(Model.Gpos, Model.angle);
        }
    }

    public void MoveLaser()
    {
        if (Model.MoveLaser())
            View.laser.GetComponent<LaserView>().Move(Model.Gpos, Model.angle);
        else
            View.DestroyLaser();
    }

    public float UpdateLaserMagazine()
    {
        return Model.Gun.UpdateLaserMagazine();
    }
}

