using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketController : MonoBehaviour
{
    #region Variables Declaration
    [Header("Movement and Rotation Speeds")]
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 20f;

    private Rigidbody _rigidbody;
    private float _horizontalInput;
    private float _verticalInput;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RocketControllerMovement();
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

    #endregion
}
