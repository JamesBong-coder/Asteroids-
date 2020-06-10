using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AsteroidLib
{


    public interface ICollision
    {
        bool Collision(ICollision obj);
        MyRectangle Rect { get; }
    }


    public class GameModel
    {
        public PlayerModel Player;
        public SpawnModel Spawn;
        private int Score;
        public bool GameOver;

        public GameModel(PlayerModel player, SpawnModel spawn)
        {
            Player = player;
            Spawn = spawn;
            Score = 0;
            GameOver = false;
        }

        public GameModel()
        {
            Player = new PlayerModel();
            Spawn = new SpawnModel();
            Score = 0;
            GameOver = false;
        }

        public void MoveEnemy()
        {
            foreach (EnemyModel enemy in Spawn.enemies)
            {
                if (Player != null)
                    enemy.Move(Player.Pos);
            }
        }

        public void ScoreUp(int t)
        {
            Score += t;
        }
        public int GetScore()
        {
            return Score;
        }

        public void CheckCollision() //проверяет столкновения объектов
        {
            foreach (BulletModel bullet in Player.Gun.bullets)
            {
                foreach (AsteroidModel asteroid in Spawn.asteroids)
                {
                    if (bullet.Collision(asteroid))
                    {
                        bullet.IsDead = true;
                        asteroid.isDead = true;
                        ScoreUp(10);
                    }
                }
                foreach (EnemyModel enemy in Spawn.enemies)
                {
                    if (bullet.Collision(enemy))
                    {
                        bullet.IsDead = true;
                        enemy.isDead = true;
                        ScoreUp(50);
                    }
                }
            }
            foreach (AsteroidModel asteroid in Spawn.asteroids)
            {
                if (Player.Gun.laser != null)
                {
                    if (asteroid.Collision(Player.Gun.laser))
                    {
                        asteroid.isDead = true;
                        ScoreUp(10);
                    }
                }
                if (asteroid.Collision(Player))
                {
                    asteroid.isDead = true;
                    Player = null;
                    GameOver = true;
                    return;
                }
            }
            foreach (EnemyModel enemy in Spawn.enemies)
            {
                if (Player.Gun.laser != null)
                {
                    if (enemy.Collision(Player.Gun.laser))
                    {
                        enemy.isDead = true;
                        ScoreUp(50);
                    }
                }
                if (enemy.Collision(Player))
                {
                    enemy.isDead = true;
                    Player = null;
                    GameOver = true;
                    return;
                }
            }
        }
    }





    public abstract class MoveClass : ICollision
    {
        public readonly float Speed;
        public float angle;
        public PointF Pos;
        private SizeF Size;

        public MoveClass(float speed, PointF pos, float Angle, SizeF size)
        {
            Speed = speed;
            Pos = pos;
            angle = Angle;
            Size = size;
        }
        public MoveClass(float speed) //используется для инициализации игрока, можно использовать вместо него предыдущий
        {
            Speed = speed;
            Pos = new PointF(0, 0);
            angle = 0;
            Size = new SizeF(1.7f, 1.3f);
        }

        public static float GetRadians(float angle)
        {
            return (angle * (float)Math.PI) / 180;
        }

        public void Move(double accel)
        {
            Pos.X += (float)(Math.Cos(GetRadians(angle)) * Speed * accel);
            Pos.Y += (float)(Math.Sin(GetRadians(angle)) * Speed * accel);
        }

        //For Collision
        public bool Collision(ICollision o)
        {
            if (o.Rect.IntersectRects(this.Rect))
                return true;
            else return false;
        }

        public MyRectangle Rect //содержит 4 точки отвечающие за габариты объекта
        {
            get
            {
                return new MyRectangle(Pos, Size, angle);
            }
        }
    }



    public struct MyRectangle
    {

        PointF[] Points; //0 1
                         //2 3   

        public MyRectangle(PointF pos, SizeF size, float angle)
        {
            Points = new PointF[]
            {
            new PointF(pos.X - size.Width/2, pos.Y + size.Height/2),
            new PointF(pos.X + size.Width/2, pos.Y + size.Height/2),
            new PointF(pos.X - size.Width/2, pos.Y - size.Height/2),
            new PointF(pos.X + size.Width/2, pos.Y - size.Height/2)
            };
            for (int i = 0; i < 4; i++)
            {
                Points[i] = RotatePoint(Points[i], pos, angle);
            }
        }

        public static PointF RotatePoint(PointF point, PointF center, float angle)
        {
            PointF p = new PointF();
            p.X = center.X + (point.X - center.X) * (float)Math.Cos(MoveClass.GetRadians(angle)) - (point.Y - center.Y) * (float)Math.Sin(MoveClass.GetRadians(angle));
            p.Y = center.Y + (point.X - center.X) * (float)Math.Sin(MoveClass.GetRadians(angle)) + (point.Y - center.Y) * (float)Math.Cos(MoveClass.GetRadians(angle));
            return p;
        }

        public bool IntersectRects(MyRectangle other) //проверям стороны прямоугольников на пересечение
        {
            for (int i = 0; i < 4; i += 3) // 0 and 3 points
            {
                for (int j = 1; j < 3; j++)//1 and 2 points
                {
                    for (int a = 0; a < 4; a += 3) // 0 and 3 points
                    {
                        for (int b = 1; b < 3; b++)//1 and 2 points
                        {
                            if (areCrossing(Points[i], Points[j], other.Points[a], other.Points[b]))
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        private float vector_mult(float ax, float ay, float bx, float by) //векторное произведение
        {
            return ax * by - bx * ay;
        }
        private bool areCrossing(PointF p1, PointF p2, PointF p3, PointF p4)//проверка пересечения двух отрезков
        {
            float v1 = vector_mult(p4.X - p3.X, p4.Y - p3.Y, p1.X - p3.X, p1.Y - p3.Y);
            float v2 = vector_mult(p4.X - p3.X, p4.Y - p3.Y, p2.X - p3.X, p2.Y - p3.Y);
            float v3 = vector_mult(p2.X - p1.X, p2.Y - p1.Y, p3.X - p1.X, p3.Y - p1.Y);
            float v4 = vector_mult(p2.X - p1.X, p2.Y - p1.Y, p4.X - p1.X, p4.Y - p1.Y);
            if ((v1 * v2) < 0 && (v3 * v4) < 0)
                return true;
            return false;
        }
    }

}
