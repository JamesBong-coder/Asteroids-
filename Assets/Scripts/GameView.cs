using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public PlayerView Player;
    public SpawnView Spawn;

    private Slider LaserMagazine;
    private Text ScoreText;
    private GameObject LoseMenu;
    private Text LoseLabel;


    public GameView(PlayerView player, SpawnView spawn, Slider lMag, Text scoreT, GameObject loseMen, Text loseL)
    {
        Player = player;
        Spawn = spawn;
        LaserMagazine = lMag;
        ScoreText = scoreT;
        LoseMenu = loseMen;
        LoseLabel = loseL;

        LaserMagazine.value = 100;
        ScoreText.text = "Score: 0";
        LoseMenu.SetActive(false);
    }

    public void GameOver(int Score)
    {
        Time.timeScale = 0;
        Player.DestroyPlayer();
        LoseLabel.text = "Score: " + Score + "\nYou Lose\nTry again?";
        LoseMenu.SetActive(true);
    }

    public void SetScore(int Score)
    {
        ScoreText.text = "Score: " + Score;
    }

    public void SetLaserMagazine(float value)
    {
        LaserMagazine.value = value;
    }
}
