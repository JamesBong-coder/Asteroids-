using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerModel : MoveClass
{
    public float AngularSpeed;
    private float Acceleration;
    public static float Width;
    public static float Height;
    public GunModel Gun;
    public float[] Gpos;

    public PlayerModel(float speed, float angularspeed, float WidthScene, float HeigthScene, GunModel gun)
        : base(speed/100)
    {
        AngularSpeed = angularspeed/100;
        Width = WidthScene / 2;
        Height = HeigthScene / 2;
        Gun = gun;

        Acceleration = 0;
        Gpos = new float[] { 0, 0 };
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
        if (PosX < -Width || PosX > Width) PosX *= -1;
        if (PosY < -Height || PosY > Height) PosY *= -1;
        Move(Acceleration);
    }

    public bool Shoot()
    {
        setGunPos();
        if (Gun.Shoot(angle, Gpos)) return true;
        else return false;
    }

    public bool LaserShoot()
    {
        setGunPos();
        if (Gun.LaserShoot(angle, Gpos)) return true;
        else return false;
    }

    public bool MoveLaser()
    {
        setGunPos();
        if (Gun.MoveLaser(Gpos, angle))
            return true;
        else return false;
    }

    public void setGunPos()
    {
        Gpos[0] = PosX + (float)(Math.Cos(GetRadians(angle)) * 0.8);
        Gpos[1] = PosY + (float)(Math.Sin(GetRadians(angle)) * 0.8);
    }
}

public abstract class MoveClass 
{
    public float Speed;
    public float PosX;
    public float PosY;
    public float angle;

    public MoveClass(float speed, float posX, float posY, float Angle)
    {
        Speed = speed;
        PosX = posX;
        PosY = posY;
        angle = Angle;
    }
    public MoveClass(float speed)
    {
        Speed = speed;
        PosX = 0;
        PosY = 0;
        angle = 0;
    }

    public float GetRadians(float angle)
    {
        return (angle * (float)Math.PI) / 180;
    }

    public float[] GetPos()
    {
        return new float[] { PosX, PosY };
    }

    public void Move(double accel)
    {
        PosX += (float)(Math.Cos(GetRadians(angle)) * Speed * accel);
        PosY += (float)(Math.Sin(GetRadians(angle)) * Speed * accel);
    }

}

public class GunModel
{
    public float ShootDelay;
    public float LaserDelay;
    public float LaserMagazine;

    private DateTime nextShoot;
    private DateTime nextLaser;

    private LaserModel laser;
    public List<BulletModel> bullets;

    private BulletModel bullet;

    public GunModel(float shootD, float laserD)
    {
        ShootDelay = shootD;
        LaserDelay = laserD;

        LaserMagazine = 100;
        nextShoot = DateTime.Now;
        nextLaser = DateTime.Now;
        bullets = new List<BulletModel>();
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

    public List<float[]> GetBulletsPos()
    {
        List<float[]> bPos = new List<float[]>();
        foreach(BulletModel b in bullets)
        {
            bPos.Add(b.GetPos());
        }
        return bPos;
    }

    public bool Shoot(float angle, float[] pos)
    {
        if (DateTime.Now > nextShoot)
        {
            bullets.Add(new BulletModel(15, pos, angle));
            nextShoot = DateTime.Now.AddSeconds(ShootDelay);
            return true;
        }
        else
            return false;
    }

    public bool LaserShoot(float angle, float[] pos)
    {
        if (DateTime.Now > nextLaser && LaserMagazine >= 25)
        {
            laser = new LaserModel(pos[0], pos[1], angle);
            LaserMagazine -= 25;
            nextLaser = DateTime.Now.AddSeconds(LaserDelay);
            return true;
        }
        else return false;
    }
    

    public bool MoveLaser(float[] pos, float angle)
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

    public BulletModel(float speed, float[] pos, float ang)
        : base(speed / 100, pos[0], pos[1], ang) { IsDead = false; }

    public void CheckDeath()
    {
        if ((PosX < -PlayerModel.Width || PosX > PlayerModel.Width) || (PosY < -PlayerModel.Height || PosY > PlayerModel.Height))
            IsDead = true;
    }
}

public class LaserModel
{
    public DateTime TimeDeath;
    float PosX, PosY, Angle;

    public LaserModel(float posX, float posY, float angle)
    {
        PosX = posX;
        PosY = posY;
        Angle = angle;
        TimeDeath = DateTime.Now.AddSeconds(0.5);
    }

    public void Move(float[] pos,  float angle)
    {
        PosX = pos[0];
        PosY = pos[1];
        Angle = angle;
    }

    public bool CheckLaserDeath()
    {
        if (DateTime.Now < TimeDeath)
            return false;
        else return true;
    }
}

