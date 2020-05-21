using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Asteroids
{
    public class PlayerGunStatus
    {
        private DateTime nextBullet;
        private DateTime nextLaser;
        public float DelayBullet;
        public float DelayLaser;
        public float LaserMagazine;
        private float MaxMagazine;


        public PlayerGunStatus(float DelayBullet, float DelayLaser, float MaxMagazine)
        {
            LaserMagazine = MaxMagazine;
            this.MaxMagazine = MaxMagazine;
            this.DelayBullet = DelayBullet;
            this.DelayLaser = DelayLaser;
            nextBullet = DateTime.Now;
            nextLaser = DateTime.Now;
        }

        public void LaserMagazineUP(float up)
        {
            if (LaserMagazine < MaxMagazine) LaserMagazine += up;
        }

        public void LaserMagazineDown(float dwn)
        {
            LaserMagazine -= dwn;
        }

        public bool CheckMagazine(float cur)
        {
            if (LaserMagazine >= cur && nextLaser<DateTime.Now) return true;
            else return false;
        }

        public void UpdateShootTime()
        {
            nextBullet = DateTime.Now.AddMilliseconds(DelayBullet);
        }

        public void UpdateLaserTime()
        {
            nextLaser = DateTime.Now.AddSeconds(DelayLaser);
        }

        public bool CheckShootTime()
        {
            if (nextBullet < DateTime.Now) return true;
            else return false;
        }
    }

    public class AsteroidStatus
    {
        public float rotation;
        public float MaxSpeed, MinSpeed;
        public bool isBig;
        private Random rnd;

        public AsteroidStatus(float MinSpeed, float MaxSpeed, float rotation, bool isBig)
        {
            this.MinSpeed = MinSpeed;
            this.MaxSpeed = MaxSpeed;
            this.rotation = rotation;
            this.isBig = isBig;
            rnd = new Random();
        }

        public List<float> GetAngularVelosity()
        {
            List<float> angVel = new List<float>();
            angVel.Add(0);
            angVel.Add((float)(rnd.NextDouble() - rnd.NextDouble()) * rotation);
            angVel.Add(0);
            return angVel;
        }

        public float GetSpeed()
        {
            return (float)rnd.NextDouble() * rnd.Next((int)MinSpeed, (int)MaxSpeed);
        }
    }

    public class EnemyStatus
    {
        public float speed;

        public EnemyStatus(float speed)
        {
            this.speed = speed;
        }
    }

    public class ManagerStatus
    {
        public bool CheckDie;
        private int Score;

        public ManagerStatus()
        {
            CheckDie = false;
            Score = 0;
        }

        public void ScoreUP(int score)
        {
            Score += score;
        }

        public int GetScore()
        {
            return Score;
        }
    }

    public class PlayerControllerStatus
    {
        public float Speed;
        public float rotationSpeed;
        private float BrakingTime;

        public PlayerControllerStatus(float speed, float rotationSpeed)
        {
            this.Speed = speed;
            this.rotationSpeed = rotationSpeed;
            BrakingTime = 0;
        }

        public void CheckBrTime(float delay)
        {
            if(BrakingTime!=1)
                BrakingTime += delay;
        }

        public void UpdateBrTime()
        {
            BrakingTime = 0;
        }

        public float GetBrTime()
        {
            return BrakingTime;
        }
    }

    public class SpawnStatus
    {
        public int minDelayAster;
        public int maxDelayAster;
        private DateTime nextAster;
        public float DelayEnemy;
        private DateTime nextEnemy;
        private Random rnd;

        public SpawnStatus(int minD, int maxD, float DEnemy)
        {
            minDelayAster = minD;
            maxDelayAster = maxD;
            DelayEnemy = DEnemy;

            nextAster = DateTime.Now;
            nextEnemy = DateTime.Now.AddSeconds(DelayEnemy);
            rnd = new Random();
        }

        public int GetSpawnIndex()
        {
            return rnd.Next(0, 4);
        }

        public bool CheckAster()
        {
            if (DateTime.Now > nextAster) return true;
            else return false;
        }

        public bool CheckEnemy()
        {
            if (DateTime.Now > nextEnemy) return true;
            else return false;
        }

        public void UpdateAsterTime()
        {
            nextAster = DateTime.Now.AddSeconds(rnd.Next(minDelayAster, maxDelayAster));
        }

        public void UpdateEnemyTime()
        {
            nextEnemy = DateTime.Now.AddSeconds(DelayEnemy);
        }
    }
}
