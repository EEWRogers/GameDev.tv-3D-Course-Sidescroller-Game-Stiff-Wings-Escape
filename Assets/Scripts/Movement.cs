using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float thrustStrength = 1f;
    [SerializeField] float rotationStrength = 1f;
    [SerializeField] float audioVolume = 1f;
    [SerializeField] AudioClip thrustSound;
    [SerializeField] ParticleSystem mainThrusterParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    Rigidbody playerRigidbody;
    AudioSource playerAudioSource;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GetThrust();
        GetRotation();
    }

    void GetThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void GetRotation()
    {
       if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }

        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }

        else
        {
            StopRotating();
        }
    }

    void StartThrusting()
    {
        playerRigidbody.AddRelativeForce(Vector3.up * thrustStrength * Time.deltaTime);
        mainThrusterParticles.Play();

        if (!playerAudioSource.isPlaying) // This prevents multiple audio tracks from playing simultaneously
        {
            playerAudioSource.PlayOneShot(thrustSound, audioVolume);
        }
    }

    void StopThrusting()
    {
        playerAudioSource.Stop();
        mainThrusterParticles.Stop();
    }
    
    private void RotateLeft()
    {
        RotatePlayer(rotationStrength);
        rightThrusterParticles.Play();
    }

    private void RotateRight()
    {
        RotatePlayer(-rotationStrength);
        leftThrusterParticles.Play();
    }

    private void StopRotating()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    void RotatePlayer(float rotationThisFrame)
    {
        playerRigidbody.freezeRotation = true; //freezing physics-based rotation to allow for manual rotation
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        playerRigidbody.freezeRotation = false; //unfreezing physics-based rotation to allow the physics system to take over
    }

}