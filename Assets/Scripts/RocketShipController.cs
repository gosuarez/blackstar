using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketShipController : MonoBehaviour
{
    #region Variables Declaration
    [Header("Movement and Rotation Speeds")]
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 20f;

    [SerializeField] private AudioClip _audioClip;

    private AudioSource _audioSource;
    private Rigidbody _rigidbody;
    private float _horizontalInput;
    private float _verticalInput;

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
        PlayAudio(_audioClip);
    }

    #region RocketShip Movement

    private void RocketControllerMovement()
    {
        //gets inputs for both axises
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        //moves rocketship according to input and coordinate system
        _rigidbody.AddRelativeForce(Vector3.up * _verticalInput * _movementSpeed, ForceMode.Force);

        //rotates rocketship according to input
        transform.Rotate(Vector3.back, _horizontalInput * Time.deltaTime * _rotationSpeed);
    }

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
