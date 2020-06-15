using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AsteroidLib;

public class GameController : MonoBehaviour
{
    public GameObject Player;
    private PlayerModel _PlayerModel;
    private GunModel _Gun;

    public Slider LaserMagazine;
    public Text ScoreText;
    public GameObject LoseMenu;
    public Text LoseLabel;

    public float speed;
    public float angularSpeed;
    public float ShootDelay;
    public float LaserDelay;

    public GameObject Spawner;
    public float DelayAster;
    public float DelayEnemy;
    public float Speed;
    private SpawnModel _Model;

    private Presenter _Presenter; 

    void Start()
    {
        _Gun = new GunModel(ShootDelay, LaserDelay);
        _PlayerModel = new PlayerModel(speed, angularSpeed, 20, 12, _Gun); //20 и 12 ширина и высота игрового пространства
        PlayerView Pview = Player.GetComponent<PlayerView>();

        SpawnView Sview = Spawner.GetComponent<SpawnView>();
        _Model = new SpawnModel(DelayAster, DelayEnemy, 20, 12, Speed);

        GameModel Model = new GameModel(_PlayerModel, _Model);
        GameView View = new GameView(Pview, Sview, LaserMagazine, ScoreText, LoseMenu, LoseLabel);
        _Presenter = new Presenter(View, Model);
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") > 0) _Presenter.UpdateAccel();
        _Presenter.Rotate(Input.GetAxis("Horizontal"));
        _Presenter.PresenterFixedUpdate();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            _Presenter.Shoot();

        if (Input.GetMouseButton(1))
            _Presenter.LaserShoot();
        _Presenter.PresenterUpdate();
    }

    public void NGButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
