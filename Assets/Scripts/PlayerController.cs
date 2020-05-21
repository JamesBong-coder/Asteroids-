using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10;
    public float rotationSpeed = 180;
    public float DelayLerp;

    private Vector3 rotation;
    private PlayerControllerStatus status;

    public void Start()
    {
        status = new PlayerControllerStatus( speed, rotationSpeed);
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Horizontal") * status.rotationSpeed * Time.deltaTime, 0)); //поворот корабля
        if (Input.GetAxis("Vertical") > 0)
        {
            rb.AddForce( transform.forward * status.Speed * Input.GetAxis("Vertical"));//Добавляем силу по направлению корабля, корабль двигается только вперед
            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -status.Speed, status.Speed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -status.Speed, status.Speed));
            status.UpdateBrTime();
        }
        else
        {
            rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, status.GetBrTime()), 0, Mathf.Lerp(rb.velocity.z, 0, status.GetBrTime())); //постепенное снижение скорости
            status.CheckBrTime(DelayLerp); //0.001
        }
        //перенос корабля при выходе за пределы игрового пространства
        if (transform.position.z > 6 || transform.position.z < -6) transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z * -1); //перенос корабля при выходе за пределы игрового пространства
        if (transform.position.x > 9.5f || transform.position.x < -9.5f) transform.position = new Vector3(transform.position.x * -1, transform.position.y, transform.position.z);
    }
}
