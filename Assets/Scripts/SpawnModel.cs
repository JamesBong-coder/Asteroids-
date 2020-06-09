using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

public struct MoveBase
{
    public PointF Pos;
    public float angle;

    public MoveBase(PointF pos, float ang)
    {
        Pos = pos;
        angle = ang;
    }
}

public class SpawnModel
{
    public float DelayAster, DelayEnemy;
    public float SpeedObjects;
    private DateTime nextAster, nextEnemy;
    public float Width, Height;
    public static float DieWidth, DieHeight;
    private Random rnd;

    public List<AsteroidModel> asteroids;
    public List<EnemyModel> enemies;

    SizeF SizeAster;
    SizeF SizeEnemy;

    public SpawnModel(float asterDelay, float enemyDelay, float width, float height, float speedObjects)
    {
        DelayAster = asterDelay;
        DelayEnemy = enemyDelay;
        Width = width;
        Height = height;
        SpeedObjects = speedObjects/100;
        rnd = new Random();

        DieWidth = width + 2;
        DieHeight = height + 2;
        nextAster = DateTime.Now;
        nextEnemy = DateTime.Now.AddSeconds(DelayEnemy);
        asteroids = new List<AsteroidModel>();
        enemies = new List<EnemyModel>();

        SizeAster = new SizeF(0.7f, 0.7f);
        SizeEnemy = new SizeF(1.5f, 1.5f);
    }

    public bool SpawnAsteroids()
    {
        if (DateTime.Now > nextAster)
        {
            int check = rnd.Next(1, 5);
            if(check == 1)
                asteroids.Add(new AsteroidModel(SpeedObjects, GetNewPos(), true, new SizeF(SizeAster.Width *3, SizeAster.Height *3)));
            else
                asteroids.Add(new AsteroidModel(SpeedObjects, GetNewPos(), false, SizeAster));

            nextAster = DateTime.Now.AddSeconds(DelayAster);
            return true;
        }
        else return false;
    }

    public bool SpawnEnemy()
    {
        if (DateTime.Now > nextEnemy)
        {
            enemies.Add(new EnemyModel(GetNewPos(), SpeedObjects, SizeEnemy));
            nextEnemy = DateTime.Now.AddSeconds(DelayEnemy);
            return true;
        }
        else return false;
    }

    public void MoveAsteroids()
    {
        foreach(AsteroidModel aster in asteroids)
        {
            aster.Move(1);
        }
    }

    public List<bool> CheckAster()
    {
        List<bool> check = new List<bool>();
        for(int i=0; i < asteroids.Count; i++)
        {
            if (asteroids[i].CheckAster())
            {
                check.Add(asteroids[i].CheckAster());
                asteroids.RemoveAt(i);
                i--;
            }
            else
                check.Add(asteroids[i].CheckAster());
        }
        return check;
    } 

    public List<bool> CheckEnemy()
    {
        List<bool> check = new List<bool>();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isDead)
            {
                check.Add(enemies[i].isDead);
                enemies.RemoveAt(i);
                i--;
            }
            else
                check.Add(enemies[i].isDead);
        }
        return check;
    }

    public List<PointF> GetAllPos(string AsterOrEnemy)
    {
        List<PointF> pos = new List<PointF>();
        if (AsterOrEnemy == "Aster")
        {
            foreach (AsteroidModel aster in asteroids)
            {
                pos.Add(aster.Pos);
            }
        }
        else
        {
            foreach (EnemyModel enemy in enemies)
            {
                pos.Add(enemy.Pos);
            }
        }
        return pos;
    }

    private MoveBase GetNewPos()
    {
        int IndWall = rnd.Next(1, 5); //  1
        PointF pos = new PointF();    //3   4
        float angle = 0;              //  2


        switch (IndWall)
        {
            case 1:
                pos.X = (rnd.Next(0, (int)Width) - Width / 2) + (float)rnd.NextDouble();
                pos.Y = Height / 2;
                angle = rnd.Next(200, 340);
                break;
            case 2:
                pos.X = (rnd.Next(0, (int)Width) - Width / 2) + (float)rnd.NextDouble();
                pos.Y = -Height / 2;
                angle = rnd.Next(20, 160);
                break;
            case 3:
                pos.Y = (rnd.Next(0, (int)Height) - Height / 2) + (float)rnd.NextDouble();
                pos.X = -Width / 2;
                angle = rnd.Next(20, 160) - 90;
                break;
            case 4:
                pos.Y = (rnd.Next(0, (int)Height) - Height / 2) + (float)rnd.NextDouble();
                pos.X = Width / 2;
                angle = rnd.Next(110, 250);
                break;
        }
        return new MoveBase(pos, angle);
    }

    public PointF GetLastPos(string AsterOrEnemy)
    {
        PointF pos = new PointF();
        if(AsterOrEnemy == "Aster")
        {
            pos = asteroids.Last().Pos;
        }
        else
        {
            pos = enemies.Last().Pos;
        }
        return pos;
    }

    public bool GetLastAsterScale()
    {
        return asteroids.Last().isBig;
    }
}

public class AsteroidModel : MoveClass
{
    public bool isBig;
    public bool isDead;

    public AsteroidModel(float speed, MoveBase move, bool big, SizeF size)
        : base(speed, move.Pos, move.angle, size)
    {
        isBig = big;
        isDead = false;
    }

    public bool CheckAster()
    {
        if ((Pos.X < -SpawnModel.DieWidth || Pos.X > SpawnModel.DieWidth) || (Pos.Y < -SpawnModel.DieHeight || Pos.Y > SpawnModel.DieHeight))
            isDead = true;
        return isDead;
    }



}

public class EnemyModel : MoveClass
{
    public bool isDead;

    public EnemyModel(MoveBase m, float speed, SizeF size)
        : base(speed, m.Pos, m.angle, size) { isDead = false; }

    public void Move(PointF PlayerPos)
    {
        PlayerPos.X -= Pos.X;
        PlayerPos.Y -= Pos.Y;
        float dist = (float)Math.Sqrt(PlayerPos.X * PlayerPos.X + PlayerPos.Y * PlayerPos.Y);
        Pos.X += PlayerPos.X / dist * Speed;
        Pos.Y += PlayerPos.Y / dist * Speed;
    }
}