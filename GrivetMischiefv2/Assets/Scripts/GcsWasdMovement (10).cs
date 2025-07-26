using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

[RequireComponent(typeof(Rigidbody))]
public class GcsWasdMovement : MonoBehaviour
{
    [Header("Made by Glitched Cat Studios\nPlease give credits!")]
    [Space]

    public bool movementEnabled = false;
    [Space(20)]
    public float speedMultiplier = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 1.0f;

    private Rigidbody rb;
    private bool isGrounded;
    private bool isRotating;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (movementEnabled)
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                moveDirection += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection += Vector3.back;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += Vector3.right;
            }

            moveDirection = transform.TransformDirection(moveDirection) * speedMultiplier;

            Vector3 newVelocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
            rb.velocity = newVelocity;

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                isRotating = true;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Cursor.lockState = CursorLockMode.None;
                isRotating = false;
            }

            if (isRotating)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

                transform.Rotate(Vector3.up, mouseX);

                Camera mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    mainCamera.transform.Rotate(Vector3.right, -mouseY);
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
#endif

//This Script was made by Glitched Cat Studios!
//You may distribute this script but LEAVE THIS WATERMARK IN THE SCRIPT!!!
//Thanks, The Glitched Cat Studios Team.