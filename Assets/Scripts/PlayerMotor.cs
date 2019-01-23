using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMotor : MonoBehaviour
{
    public bool shouldUpdate;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 tilt = Vector3.zero;
    private Vector3 jumpVector = Vector3.zero;
    private bool isGrounded = true;
    private Camera cam;
    private Rigidbody rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        ApplyGravity();
    }

    public void Move(Vector3 vel)
    {
        velocity = vel;
    }

    public void Rotate(Vector3 rot)
    {
        rotation = rot;
    }

    public void Tilt(Vector3 t)
    {
        tilt = t;
    }
    public void Jump(Vector3 jumpV)
    {
        jumpVector = jumpV;
    }

    public void Grounded(bool grounded)
    {
        isGrounded = grounded;
    }

    public void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.velocity -= 0.981f * Vector3.up;
        }

    }

    public void Animate(float hor,float ver)
    {
        anim.SetFloat("Horizontal", hor);
        anim.SetFloat("Vertical", ver);
    }
    public void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }
        if (jumpVector != Vector3.zero)
        {
            rb.velocity =new Vector3(rb.velocity.x,0,rb.velocity.z);
            rb.AddForce(jumpVector, ForceMode.Impulse);
        }
    }

    public void PerformRotation()
    {
        if (rotation != Vector3.zero)
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
            if (cam != null)
            { 
                cam.transform.Rotate(-tilt);
            }
        }
    }
}
