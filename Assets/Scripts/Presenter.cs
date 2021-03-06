﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsteroidLib;

public class Presenter
{
    private GameView View;
    private GameModel Model;
    private bool GameOver;

    public Presenter(GameView v, GameModel m)
    {
        View = v;
        Model = m;
        GameOver = false;
    }
    
    public void Move()
    {
        Model.Player.Move();
        View.Player.Move(Model.Player.Pos);
    }

    public void Rotate(float dir)
    {
            Model.Player.RotatePlayer(dir);
            View.Player.Rotate(Model.Player.angle);
    }

    public void UpdateAccel()
    {
           Model.Player.UpdateAccel();
    }

    public void Shoot()
    {
        if (!GameOver)
            if (Model.Player.Shoot())
        {
            View.Player.Shoot(Model.Player.Gpos, Model.Player.angle);
        }
    }

    private void MoveBullets()
    {
        Model.Player.Gun.MoveBullets();
        View.Player.MoveBullets(Model.Player.Gun.GetBulletsPos());
    }

    private void CheckBullets()
    {
        View.Player.CheckBullets(Model.Player.Gun.GetCheckBullets());
    }

    public void LaserShoot()
    {
        if (!GameOver)
            if (Model.Player.LaserShoot())
        {
            View.Player.LaserShoot(Model.Player.Gpos, Model.Player.angle);
        }
    }

    private void MoveLaser()
    {
        if (Model.Player.MoveLaser())
            View.Player.laser.GetComponent<LaserView>().Move(Model.Player.Gpos, Model.Player.angle);
        else
            View.Player.DestroyLaser();
    }

    private float UpdateLaserMagazine()
    {
        return Model.Player.Gun.UpdateLaserMagazine();
    }


    //Spawn
    private void SpawnAster()
    {
        if(Model.Spawn.SpawnAsteroids())
            View.Spawn.SpawnAster(Model.Spawn.GetLastPos("Aster"), Model.Spawn.GetLastAsterScale());
    }

    private void SpawnEnemy()
    {
        if (Model.Spawn.SpawnEnemy())
            View.Spawn.SpawnEnemy(Model.Spawn.GetLastPos("Enemy"));
    }

    private void MoveAster()
    {
        Model.Spawn.MoveAsteroids();
        View.Spawn.MoveAster(Model.Spawn.GetAllPos("Aster"));
    }

    private void MoveEnemy()
    {
        Model.MoveEnemy();
        View.Spawn.MoveEnemy(Model.Spawn.GetAllPos("Enemy"));
    }

    private void CheckAster()
    {
        View.Spawn.CheckAster(Model.Spawn.CheckAster());
    }

    private void CheckEnemies()
    {
        View.Spawn.CheckEnemy(Model.Spawn.CheckEnemy());
    }

    //All
    public void PresenterFixedUpdate()
    {
        Move();
        MoveBullets();
        MoveLaser();
        View.SetLaserMagazine(UpdateLaserMagazine());
        CheckAster();
        CheckEnemies();
        CheckBullets();
        MoveAster();
        MoveEnemy();
        Model.CheckCollision();
        if(Model.GameOver && !GameOver)
        {
            GameOver = true;
            View.GameOver(Model.GetScore());
        }
    }

    public void PresenterUpdate()
    {
        SpawnAster();
        SpawnEnemy();
        View.SetScore(Model.GetScore());

    }
}

