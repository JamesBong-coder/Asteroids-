using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject Player;
    public Slider LaserMagazine;
    public Text ScoreText;
    public GameObject LoseMenu;
    public Text LoseLabel;
    private int score;
    private bool CheckDie;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        score = 0;
        ScoreText.text = "Score: " + score;
        LoseMenu.SetActive(false);
    }

    void Update()
    {
        if (Player != null)
            LaserMagazine.value = Player.GetComponent<PlayerGun>().LaserMagazine;
        else if (!CheckDie)
        {
            CheckDie = false;
            LoseLabel.text = "Score: " + score + "\nYou Lose\nTry again?";
            LoseMenu.SetActive(true);
        }
    }

    public void ScoreUP(int CurScore)
    {
        score += CurScore;
        ScoreText.text = "Score: " + score;
    }

    public void NGButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
