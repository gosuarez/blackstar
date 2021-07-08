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
    [SerializeField] 
    private float _movementSpeed;

    [Range(200, 300)]
    [SerializeField] 
    private float _rotationSpeed;

    [SerializeField] 
    private AudioClip _audioClip;

    private AudioSource _audioSource;
    private Rigidbody _rigidbody;
    private float _horizontalInput;
    private float _verticalInput;

    public event Action HitByObtacle;
    public event Action<int> OnLandingPad;
    private int level = 0;

    [SerializeField] 
    private enum State { Alive, Dying, Transcending }
    State state;

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
        if (state == State.Alive)
        {
            RocketControllerMovement();
            RockectControllerRotation();
            PlayAudio(_audioClip);
        }
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
        if(state != State.Alive)
        {
            return;
        }

            switch (collision.gameObject.tag)
            {
                case "LaunchingPad":
                state = State.Alive;
#if DEBUG_RocketShipController
                    Debug.Log("Ship is on LaunchingPad");
#endif
                    break;
                case "LandingPad":
                state = State.Transcending;
#if DEBUG_RocketShipController
                Debug.Log("Ship is on LandingPad");
#endif
                LandingPadReached(level);
                    break;

                case "Obstacle":
                state = State.Dying;
#if DEBUG_RocketShipController
                Debug.Log("Death");
#endif
                    TakeHit();
                    break;
            }
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

    private void TakeHit()
    {
        if (HitByObtacle != null)
        {
            HitByObtacle();
        }

        Destroy(gameObject);
    }

    private void LandingPadReached(int level)
    {
        if (OnLandingPad != null)
        {
            OnLandingPad(level);
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
