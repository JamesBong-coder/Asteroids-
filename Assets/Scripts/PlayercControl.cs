using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayercControl : MonoBehaviour
{
    public GameObject Player;
    public PlayerPresenter _PlayerPresenter;
    public PlayerModel _PlayerModel;
    public GunModel _Gun;

    public float speed;
    public float angularSpeed;
    public float ShootDelay;
    public float LaserDelay;

    void Start()
    {
        _Gun = new GunModel(ShootDelay, LaserDelay);
        _PlayerModel = new PlayerModel(speed, angularSpeed, 20, 12, _Gun); //20 и 12 ширина и высота игрового пространства
        PlayerView view = Player.GetComponent<PlayerView>();
        _PlayerPresenter = new PlayerPresenter(view, _PlayerModel);
    }


    void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") > 0) _PlayerPresenter.UpdateAccel();
        _PlayerPresenter.Move();

        _PlayerPresenter.Rotate(Input.GetAxis("Horizontal"));

        _PlayerPresenter.MoveBullets();
        _PlayerPresenter.CheckBullets();

        _PlayerPresenter.MoveLaser();
        _PlayerPresenter.UpdateLaserMagazine();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            _PlayerPresenter.Shoot();

        if (Input.GetKey(KeyCode.M))
            _PlayerPresenter.LaserShoot();
    }
}
