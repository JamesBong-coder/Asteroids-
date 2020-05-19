using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float _speed = 10;
    public float _rotationSpeed = 180;
    private float t;
    public float DelayLerp;

    private Vector3 rotation;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = 0;
    }

    public void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Horizontal") * _rotationSpeed * Time.deltaTime, 0)); //поворот корабля
        if (Input.GetAxis("Vertical") > 0)
        {
            rb.AddForce( transform.forward * _speed * Input.GetAxis("Vertical"));//Добавляем силу по направлению корабля, корабль двигается только вперед
            rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -_speed, _speed), rb.velocity.y, Mathf.Clamp(rb.velocity.z, -_speed, _speed));
            t = 0;
        }
        else
        {
            rb.velocity = new Vector3(Mathf.Lerp(rb.velocity.x, 0, t), 0, Mathf.Lerp(rb.velocity.z, 0, t)); //постепенное снижение скорости
            if (t != 1)
                t += DelayLerp;
        }
        if (transform.position.z > 6 || transform.position.z < -6) transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z * -1);
        if (transform.position.x > 9.5f || transform.position.x < -9.5f) transform.position = new Vector3(transform.position.x * -1, transform.position.y, transform.position.z);
    }
}
