using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject Player;
    public PlayerModel _PlayerModel;
    public GunModel _Gun;

    public float speed;
    public float angularSpeed;
    public float ShootDelay;
    public float LaserDelay;

    public GameObject Spawner;
    public float DelayAster;
    public float DelayEnemy;
    public float Speed;
    private SpawnModel _Model;

    public Presenter _Presenter; 

    void Start()
    {
        _Gun = new GunModel(ShootDelay, LaserDelay);
        _PlayerModel = new PlayerModel(speed, angularSpeed, 20, 12, _Gun); //20 и 12 ширина и высота игрового пространства
        PlayerView Pview = Player.GetComponent<PlayerView>();

        SpawnView Sview = Spawner.GetComponent<SpawnView>();
        _Model = new SpawnModel(DelayAster, DelayEnemy, 20, 12, Speed);

        GameModel Model = new GameModel(_PlayerModel, _Model);
        GameView View = new GameView(Pview, Sview);
        _Presenter = new Presenter(View, Model);
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") > 0) _Presenter.UpdateAccel();
        _Presenter.Move();

        _Presenter.Rotate(Input.GetAxis("Horizontal"));

        _Presenter.PresenterFixedUpdate();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            _Presenter.Shoot();

        if (Input.GetKey(KeyCode.M))
            _Presenter.LaserShoot();

        _Presenter.PresenterUpdate();
    }
}
