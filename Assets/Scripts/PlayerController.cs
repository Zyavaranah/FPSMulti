using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;

    [SerializeField]
    private float lookSensitivity = 10f;

    [SerializeField]
    float jumpForce = 5;

    private PlayerMotor motor;
    private bool isGrounded=true;

    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        float xmov = Input.GetAxisRaw("Horizontal");
        float zmov = Input.GetAxisRaw("Vertical");
        motor.Animate(xmov, zmov);

        Vector3 hor = transform.right * xmov;
        Vector3 ver = transform.forward * zmov;

        Vector3 velocity = (hor + ver).normalized * speed;

        motor.Move(velocity);

        float yrot = Input.GetAxis("Mouse X");
        Vector3 rotation = new Vector3(0, yrot, 0) * lookSensitivity;
        motor.Rotate(rotation);

        float xrot = Input.GetAxis("Mouse Y");
        Vector3 tilt = new Vector3(xrot, 0, 0) * lookSensitivity;
        motor.Tilt(tilt);

        Vector3 jumpVector = Vector3.zero;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.05f);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpVector = Vector3.up * jumpForce;
        }
        motor.Jump(jumpVector);
        motor.Grounded(isGrounded);
    }

}
