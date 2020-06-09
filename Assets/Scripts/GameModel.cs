using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;



public interface ICollision
{
    bool Collision(ICollision obj);
    MyRectangle Rect { get; }
}


public class GameModel
{
    public PlayerModel Player;
    public SpawnModel Spawn;

    public GameModel(PlayerModel player, SpawnModel spawn)
    {
        Player = player;
        Spawn = spawn;
    }

    public void MoveEnemy()
    {
        foreach (EnemyModel enemy in Spawn.enemies)
        {
            enemy.Move(Player.Pos);
        }
    }

    public void CheckCollision()
    {
        foreach(BulletModel bullet in Player.Gun.bullets)
        {
            foreach(AsteroidModel asteroid in Spawn.asteroids)
            {
                if (bullet.Collision(asteroid))
                {
                    bullet.IsDead = true;
                    asteroid.isDead = true;
                }
            }
            foreach(EnemyModel enemy in Spawn.enemies)
            {
                if (bullet.Collision(enemy))
                {
                    bullet.IsDead = true;
                    enemy.isDead = true;
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
                }
            }
        }
        foreach (EnemyModel enemy in Spawn.enemies)
        {
            if (Player.Gun.laser != null)
            {
                if (enemy.Collision(Player.Gun.laser))
                {
                    enemy.isDead = true;
                }
            }
        }
    }
}





public abstract class MoveClass : ICollision
{
    public float Speed;
    public float angle;
    public PointF Pos;
    public SizeF Size;

    public MoveClass(float speed, PointF pos, float Angle, SizeF size)
    {
        Speed = speed;
        Pos = pos;
        angle = Angle;
        Size = size;
    }
    public MoveClass(float speed)
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
    
    public MyRectangle Rect
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

    public bool IntersectRects(MyRectangle other)
    {
        for(int i = 0; i<4; i+=3) // 0 and 3 points
        {
            for(int j=1; j<3; j++)//1 and 2 points
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
    private bool areCrossing(PointF p1, PointF p2, PointF p3, PointF p4)//проверка пересечения
    {
        float v1 = vector_mult(p4.X - p3.X, p4.Y - p3.Y, p1.X - p3.X, p1.Y - p3.Y);
        float v2 = vector_mult(p4.X - p3.X, p4.Y - p3.Y, p2.X - p3.X, p2.Y - p3.Y);
        float v3 = vector_mult(p2.X - p1.X, p2.Y - p1.Y, p3.X - p1.X, p3.Y - p1.Y);
        float v4 = vector_mult(p2.X - p1.X, p2.Y - p1.Y, p4.X - p1.X, p4.Y - p1.Y);
        if ((v1 * v2) < 0 && (v3 * v4) < 0)
            return true;
        return false;
    }

    //старая версия определения пересечения двух отрезков
    //private bool CheckIntersectTwoLine(PointF MyP1, PointF MyP2, PointF OthP1, PointF OthP2) //метод для сравнения двух отрезков
    //{
    //    float Xa, Ya, A1, A2, b1, b2; // х и у координаты пересечения прямых, а и б - части уравнений наших прямых

    //    if (MyP2.X < MyP1.X) //начальная точка прямой должна находиться левее чем конечная
    //    {
    //        PointF tmp = MyP1;
    //        MyP1 = MyP2;
    //        MyP2 = tmp;
    //    }
    //    if (OthP2.X < OthP1.X)
    //    {
    //        PointF tmp = OthP1;
    //        OthP1 = OthP2;
    //        OthP2 = tmp;
    //    }

    //    if (MyP2.X < OthP1.X) //если конечная точка первого отрезка левее чем начальная другого, то пересечения не будет
    //        return false;

    //    if (MyP1.X - MyP2.X == 0) // если первый отрезок вертикальный
    //    {
    //        //найдём Xa, Ya - точки пересечения двух прямых
    //        Xa = MyP1.X;
    //        A2 = (OthP1.Y - OthP2.Y) / (OthP1.X - OthP2.X);
    //        b2 = OthP1.Y - A2 * OthP1.X;
    //        Ya = A2 * Xa + b2;

    //        if (OthP1.X <= Xa && OthP2.X >= Xa && Math.Min(MyP1.Y, MyP2.Y) <= Ya && Math.Max(MyP1.Y, MyP2.Y) >= Ya)
    //            return true;
    //        return false;
    //    }
    //    //если второй отрезок вертикальный
    //    if (OthP1.X - OthP2.X == 0)
    //    {
    //        //найдём Xa, Ya - точки пересечения двух прямых
    //        Xa = OthP1.X;
    //        A1 = (MyP1.Y - MyP2.Y) / (MyP1.X - MyP2.X);
    //        b1 = MyP1.Y - A1 * MyP1.X;
    //        Ya = A1 * Xa + b1;

    //        if (MyP1.X <= Xa && MyP2.X >= Xa && Math.Min(OthP1.Y, OthP2.Y) <= Ya && Math.Max(OthP1.Y, OthP2.Y) >= Ya)
    //            return true;
    //        return false;
    //    }

    //    //наконец общий случай когда оба отрезка невертикальные
    //    //находим уравнения прямых по данным нам точкам
    //    A1 = (MyP1.Y - MyP2.Y) / (MyP1.X - MyP2.X);
    //    A2 = (OthP1.Y - OthP2.Y) / (OthP1.X - OthP2.X);
    //    b1 = MyP1.Y - A1 * MyP1.X;
    //    b2 = OthP1.Y - A2 * OthP1.X;

    //    if (A1 == A2)
    //    {
    //        return false; //отрезки параллельны
    //    }

    //    //Xa - абсцисса точки пересечения двух прямых
    //    Xa = (b2 - b1) / (A1 - A2);

    //    if ((Xa < Math.Max(MyP1.X, OthP1.X)) || (Xa > Math.Min(MyP2.X, OthP2.X)))
    //        return false; //точка Xa находится вне пересечения проекций отрезков на ось X 
    //    else
    //        return true;
    //} 
}


