using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Asteroids;

public class GameManager : MonoBehaviour
{
    private GameObject Player;
    public Slider LaserMagazine;
    public Text ScoreText;
    public GameObject LoseMenu;
    public Text LoseLabel;
    private ManagerStatus status;

    void Start()
    {
        status = new ManagerStatus();
        Player = GameObject.FindGameObjectWithTag("Player");
        LaserMagazine.value = 100;
        ScoreText.text = "Score: 0";
        LoseMenu.SetActive(false);
    }

    void Update()
    {
        LaserMagazine.value = gameObject.GetComponent<GameController>()._Presenter.Model.Player.Gun.LaserMagazine;
        if (Player == null)
        {
            status.CheckDie = true; ;
            LoseLabel.text = "Score: " + status.GetScore() + "\nYou Lose\nTry again?";
            LoseMenu.SetActive(true);
        }
    }

    public void ScoreUP(int CurScore)
    {
        status.ScoreUP(CurScore);
        ScoreText.text = "Score: " + status.GetScore();
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
