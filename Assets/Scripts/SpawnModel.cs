using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MoveBase
{
    public float PosX;
    public float PosY;
    public float angle;

    public MoveBase(float[] pos, float ang)
    {
        PosX = pos[0];
        PosY = pos[1];
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
    private System.Random rnd;

    public List<AsteroidModel> asteroids;
    public List<EnemyModel> enemies;


    public SpawnModel(float asterDelay, float enemyDelay, float width, float height, float speedObjects)
    {
        DelayAster = asterDelay;
        DelayEnemy = enemyDelay;
        Width = width;
        Height = height;
        SpeedObjects = speedObjects/100;
        rnd = new System.Random();

        DieWidth = width + 2;
        DieHeight = height + 2;
        nextAster = DateTime.Now;
        nextEnemy = DateTime.Now.AddSeconds(DelayEnemy);
        asteroids = new List<AsteroidModel>();
        enemies = new List<EnemyModel>();
        
    }

    public bool SpawnAsteroids()
    {
        if (DateTime.Now > nextAster)
        {
            int check = rnd.Next(1, 5);
            if(check == 1)
                asteroids.Add(new AsteroidModel(SpeedObjects, GetNewPos(), true));
            else
                asteroids.Add(new AsteroidModel(SpeedObjects, GetNewPos(), false));

            nextAster = DateTime.Now.AddSeconds(DelayAster);
            return true;
        }
        else return false;
    }

    public bool SpawnEnemy()
    {
        if (DateTime.Now > nextEnemy)
        {
            enemies.Add(new EnemyModel(GetNewPos(), SpeedObjects));
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

    public List<float[]> GetAllPos(string AsterOrEnemy)
    {
        List<float[]> pos = new List<float[]>();
        if (AsterOrEnemy == "Aster")
        {
            foreach (AsteroidModel aster in asteroids)
            {
                pos.Add(aster.GetPos());
            }
        }
        else
        {
            foreach (EnemyModel enemy in enemies)
            {
                pos.Add(enemy.GetPos());
            }
        }
        return pos;
    }

    private MoveBase GetNewPos()
    {
        int IndWall = rnd.Next(1, 5); //  1
        float[] pos = new float[2];   //3   4
        float angle = 0;              //  2


        switch (IndWall)
        {
            case 1:
                pos[0] = (rnd.Next(0, (int)Width) - Width / 2) + (float)rnd.NextDouble();
                pos[1] = Height / 2;
                angle = rnd.Next(200, 340);
                break;
            case 2:
                pos[0] = (rnd.Next(0, (int)Width) - Width / 2) + (float)rnd.NextDouble();
                pos[1] = -Height / 2;
                angle = rnd.Next(20, 160);
                break;
            case 3:
                pos[1] = (rnd.Next(0, (int)Height) - Height / 2) + (float)rnd.NextDouble();
                pos[0] = -Width / 2;
                angle = rnd.Next(20, 160) - 90;
                break;
            case 4:
                pos[1] = (rnd.Next(0, (int)Height) - Height / 2) + (float)rnd.NextDouble();
                pos[0] = Width / 2;
                angle = rnd.Next(110, 250);
                break;
        }
        return new MoveBase(pos, angle);
    }

    public float[] GetLastPos(string AsterOrEnemy)
    {
        float[] pos = new float[2];
        if(AsterOrEnemy == "Aster")
        {
            pos = asteroids.Last().GetPos();
        }
        else
        {
            enemies.Last().GetPos();
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
    private bool isDead;

    public AsteroidModel(float speed, MoveBase move, bool big)
        : base(speed, move.PosX, move.PosY, move.angle)
    {
        isBig = big;
        isDead = false;
    }

    public bool CheckAster()
    {
        if ((PosX < -SpawnModel.DieWidth || PosX > SpawnModel.DieWidth) || (PosY < -SpawnModel.DieHeight || PosY > SpawnModel.DieHeight))
            isDead = true;
        return isDead;
    }

}

public class EnemyModel
{
    private float PosX;
    private float PosY;

    private float Speed;

    public EnemyModel(MoveBase m, float speed)
    {
        PosX = m.PosX;
        PosY = m.PosY;
        Speed = speed;
    }

    public float[] GetPos()
    {
        return new float[] { PosX, PosY };
    }

    public void Move(float[] PlayerPos)
    {
        PlayerPos[0] -= PosX;
        PlayerPos[1] -= PosY;
        float dist = (float)Math.Sqrt(PlayerPos[0] * PlayerPos[0] + PlayerPos[1] * PlayerPos[1]);
        PosX += PlayerPos[0] / dist * Speed;
        PosY += PlayerPos[1] / dist * Speed;
        Debug.Log(PosX + " " + PosY);
    }
}