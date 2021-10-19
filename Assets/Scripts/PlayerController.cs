﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam; //for camera follow

    public float moveSpeed = 6f;

    //Character roation with camera, comment to disable
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    //Gravity
    private float verticalVelocity;
    public float gravity = 14.0f;

    //Jump
    public float jumpForce;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        Movement();

        //Gravity and Jump
        if (controller.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce; //jumping
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; //falling
        }
        Vector3 moveVector = new Vector3(0, verticalVelocity, 0);
        controller.Move(moveVector * Time.deltaTime);
        
    }


    #region - Movement -

    public void Movement()
    {
        //Movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            //Rotates camera and player smoothly depending on direction faced, comment to disable
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            //Used for regular movement with no camera follow or rotation, comment to disable
            //controller.Move(direction * moveSpeed * Time.deltaTime);

        }
    }

    #endregion

   

    


}
