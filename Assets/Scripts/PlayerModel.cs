using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

public class PlayerModel : MoveClass
{
    public float AngularSpeed;
    private float Acceleration;
    public static float Width;
    public static float Height;
    public GunModel Gun;
    public PointF Gpos;


    public PlayerModel(float speed, float angularspeed, float WidthScene, float HeigthScene, GunModel gun)
        : base(speed/100)
    {
        AngularSpeed = angularspeed/100;
        Width = WidthScene / 2;
        Height = HeigthScene / 2;
        Gun = gun;

        Acceleration = 0;
        Gpos = PointF.Empty;
    }

    public float RotatePlayer(float Dir)
    {
        angle += -Dir * AngularSpeed;
        if (angle > 360) angle -= 360;
        if (angle < 0) angle += 360;
        return angle;
    }

    public void UpdateAccel()
    {
        Acceleration = 1;
    }
    
    public void Move()
    {
        if(Acceleration > 0)
            Acceleration -= 0.01f;
        if (Pos.X < -Width || Pos.X > Width) Pos.X *= -1;
        if (Pos.Y < -Height || Pos.Y > Height) Pos.Y *= -1;
        Move(Acceleration);
    }

    public bool Shoot()
    {
        setGunPos(false);
        if (Gun.Shoot(angle, Gpos)) return true;
        else return false;
    }

    public bool LaserShoot()
    {
        setGunPos(true);
        if (Gun.LaserShoot(angle, Gpos)) return true;
        else return false;
    }

    public bool MoveLaser()
    {
        setGunPos(true);
        if (Gun.MoveLaser(Gpos, angle))
            return true;
        else return false;
    }

    public void setGunPos(bool isLaser)
    {
        if (!isLaser)
        {
            Gpos.X = Pos.X + (float)(Math.Cos(GetRadians(angle)) * 0.8);
            Gpos.Y = Pos.Y + (float)(Math.Sin(GetRadians(angle)) * 0.8);
        }
        else
        {
            Gpos.X = Pos.X + (float)(Math.Cos(GetRadians(angle)) * 10.3);
            Gpos.Y = Pos.Y + (float)(Math.Sin(GetRadians(angle)) * 10.3);
        }
    }

}


public class GunModel
{
    public float ShootDelay;
    public float LaserDelay;
    public float LaserMagazine;

    private DateTime nextShoot;
    private DateTime nextLaser;

    public LaserModel laser;
    public List<BulletModel> bullets;

    public SizeF SizeBullet;
    public SizeF SizeLaser;

    public GunModel(float shootD, float laserD)
    {
        ShootDelay = shootD;
        LaserDelay = laserD;

        LaserMagazine = 100;
        nextShoot = DateTime.Now;
        nextLaser = DateTime.Now;
        bullets = new List<BulletModel>();

        SizeBullet = new SizeF(0.45f, 0.1f);
        SizeLaser = new SizeF(19, 0.4f);
    }

    public void MoveBullets()
    {
        foreach (BulletModel b in bullets)
        {
                b.Move(1);
        }
    }
    public List<bool> GetCheckBullets()
    {
        List<bool> check = new List<bool>();
        for(int i=0; i<bullets.Count; i++)
        {
            bullets[i].CheckDeath();
            check.Add(bullets[i].IsDead);
            if (bullets[i].IsDead)
            {
                bullets.RemoveAt(i);
                i--;
            }
        }
        return check;
    }

    public List<PointF> GetBulletsPos()
    {
        List<PointF> bPos = new List<PointF>();
        foreach(BulletModel b in bullets)
        {
            bPos.Add(b.Pos);
        }
        return bPos;
    }

    public bool Shoot(float angle, PointF pos)
    {
        if (DateTime.Now > nextShoot)
        {
            bullets.Add(new BulletModel(15, pos, angle, SizeBullet));
            nextShoot = DateTime.Now.AddSeconds(ShootDelay);
            return true;
        }
        else
            return false;
    }

    public bool LaserShoot(float angle, PointF pos)
    {
        if (DateTime.Now > nextLaser && LaserMagazine >= 25)
        {
            laser = new LaserModel(pos, angle, SizeLaser);
            LaserMagazine -= 25;
            nextLaser = DateTime.Now.AddSeconds(LaserDelay);
            return true;
        }
        else return false;
    }
    

    public bool MoveLaser(PointF pos, float angle)
    {
        if (laser == null) return false;
        else
        {
            if (laser.CheckLaserDeath())
            {
                laser = null;
                return false;
            }
            else
            {
                laser.Move(pos, angle);
                return true;
            }
        }
    }

    public float UpdateLaserMagazine()
    {
        if (LaserMagazine >100)
            return LaserMagazine;
        else
            return LaserMagazine += 0.1f;
    }
}

public class BulletModel : MoveClass
{
    public bool IsDead;

    public BulletModel(float speed, PointF pos, float ang, SizeF size)
        : base(speed / 100, pos, ang, size) { IsDead = false; }

    public void CheckDeath()
    {
        if ((Pos.X < -PlayerModel.Width || Pos.X > PlayerModel.Width) || (Pos.Y < -PlayerModel.Height || Pos.Y > PlayerModel.Height))
            IsDead = true;
    }
}

public class LaserModel : MoveClass
{
    public DateTime TimeDeath;

    public LaserModel(PointF pos, float angle, SizeF size)
        :base(1, pos, angle, size)
    {
        TimeDeath = DateTime.Now.AddSeconds(0.5);
    }

    public void Move(PointF pos,  float ang)
    {
        Pos = pos;
        angle = ang;
    }

    public bool CheckLaserDeath()
    {
        if (DateTime.Now < TimeDeath)
            return false;
        else return true;
    }
}

