#define DEBUG_RocketShipController

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class RocketShipController : MonoBehaviour
{
    #region Variables Declaration
    [Header("Set in Inspector")]
    [Range(5,15)]
    [SerializeField] private float _movementSpeed;
    [Range(200, 300)]
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private AudioClip _audioClip;

    private AudioSource _audioSource;
    private Rigidbody _rigidbody;
    private float _horizontalInput;
    private float _verticalInput;

    public event Action HitByObtacle;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RocketControllerMovement();
        RockectControllerRotation();
        PlayAudio(_audioClip);
    }

    #region RocketShip Movement

    private void RocketControllerMovement()
    {
        _verticalInput = Input.GetAxisRaw("Vertical");

        //moves rocketship according to input and coordinate system
        _rigidbody.AddRelativeForce(Vector3.up * _verticalInput * _movementSpeed, ForceMode.Force);

    }
    #endregion

    #region RocketShip Rotation
    private void RockectControllerRotation()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        _rigidbody.freezeRotation = true;

        //rotates rocketship according to input
        transform.Rotate(Vector3.back, _horizontalInput * Time.deltaTime * _rotationSpeed);

        _rigidbody.freezeRotation = false;
    }

    #endregion

    #region Collision Detection

    private void OnCollisionEnter(Collision collision)
    {

        switch (collision.gameObject.tag)
        {
            case "LaunchingPad":
#if DEBUG_RocketShipController
                Debug.Log("Ship is on LaunchingPad");
#endif
                break;
            case "LandingPad":
#if DEBUG_RocketShipController
                Debug.Log("Ship is on LandingPad");
#endif
                break;

            case "Obstacle":
#if DEBUG_RocketShipController
                Debug.Log("Death");
#endif
                TakeHit();

                break;
        }
    }

    private void TakeHit()
    {
        if (HitByObtacle != null)
        {
            HitByObtacle();
        }

        Destroy(gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "LaunchingPad":
#if DEBUG_RocketShipController
                Debug.Log("Ship is airborne");
#endif
                break;
        }
    }

    #endregion

    #region Audio

    private void PlayAudio(AudioClip index)
    {
        if (_verticalInput > 0 && !_audioSource.isPlaying)
        {
            _audioSource.clip = index;
            _audioSource.Play();
        }

        else if (_verticalInput <= 0 && _audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    #endregion

}
