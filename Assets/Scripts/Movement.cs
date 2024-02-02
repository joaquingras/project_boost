using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private AudioSource _audioSource;

    [SerializeField] float _mainThrust = 1000f;
    [SerializeField] float _rotationThrust = 200f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem enginePs;
    [SerializeField] ParticleSystem lBoosterPs;
    [SerializeField] ParticleSystem rBoosterPs;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }


    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //Play Sound
            _rigidbody.AddRelativeForce(Vector3.up * _mainThrust * Time.deltaTime);
            //Particle system
            if (!enginePs.isPlaying)
            {
                enginePs.Play();
            }
            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(mainEngine);
            }
        }
        else
        {
            enginePs.Stop();
            _audioSource.Stop();
        }
    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!rBoosterPs.isPlaying) rBoosterPs.Play();
            ApplyRotation(_rotationThrust);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!lBoosterPs.isPlaying) lBoosterPs.Play();
            ApplyRotation(-1 * _rotationThrust);
        }
        else
        {
            if (lBoosterPs != null) lBoosterPs.Stop();
            if (rBoosterPs != null) rBoosterPs.Stop();
        }
    }

    private void ApplyRotation(float rotationThrust)
    {
        _rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThrust * Time.deltaTime);
        _rigidbody.freezeRotation = false;
    }
}
