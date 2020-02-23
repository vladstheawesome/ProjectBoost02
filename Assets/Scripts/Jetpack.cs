using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource jetpackSound;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        jetpackSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    print("safe");
                }
                break;
            default:
                {
                    print("dead");
                }
                break;
        }
    }

    private void Thrust()
    {
        //float thisThrust = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!jetpackSound.isPlaying)
            {
                jetpackSound.Play();
            }
        }
        else
        {
            jetpackSound.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics
    }    
}
