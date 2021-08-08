#define ROCKETSHIPCONTROLLER_DEBUG

using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class RocketShipController : MonoBehaviour
{
    #region Variables Declaration

    [Header("Set in Inspector")]
    [SerializeField] private GameObject explosionParticleSystem;
    [SerializeField] private GameObject successParticleSystem;
    [SerializeField] private GameObject shipInstantiatedParticleSystem;
    [SerializeField] private GameObject powerUpParticleSystem;

    private PlayerController _playerController;
    private float _movementSpeed;
    private float _rotationSpeed;

    public enum State
    {
        Alive,
        Dying,
        Transcending
    }

    public enum Action
    {
        Flying,
        LaunchingPad,
        LandignPad,
    }

    [Header("Set Dynamically")]
    public State shipState;
    public Action shipAction;

    private Rigidbody _rigidbody;
    private float _horizontalInput;
    private float _verticalInput;
    private ParticleSystem _thrusterParticleSystem;
    private HUDControllerGameLevels _hudControllerGameLevels;
    private int _currentLevel = 0;
    private AudioSource _mainAudioSource;
    private AudioSource _rocketShipAudioSource;
    private AudioSFXController _audioSfxController;
    private int _consumedFuel = 1;
    private float _verticalMovement;
    private Vector2 _horizontalMovement;

    //These two action events are removed as they were replaced by an event broker
    //public event Action HitByObstacle;
    //public event Action<int> OnLandingPad;

    #endregion

    private void Awake()
    {
        _playerController = new PlayerController();
        //_playerController.Gameplay.Thruster += context => ActivateThruster();
        _playerController.Gameplay.Thruster.performed += context => _verticalMovement = context.ReadValue<float>();
        _playerController.Gameplay.Thruster.canceled += context => _verticalMovement = 0;
        
        _playerController.Gameplay.Rotation.performed += context => _horizontalMovement = context.ReadValue<Vector2>();
        _playerController.Gameplay.Rotation.canceled += context => _horizontalMovement = Vector2.zero;
    }

    private void OnEnable()
    {
        _playerController.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _playerController.Gameplay.Disable();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InstantiateShipParticleSystem();
        
        _hudControllerGameLevels = FindObjectOfType<HUDControllerGameLevels>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSfxController = GameSceneController.Instance.GetComponent<AudioSFXController>();
        _mainAudioSource = _audioSfxController.GetComponent<AudioSource>();
        _rocketShipAudioSource = GetComponent<AudioSource>();
        _thrusterParticleSystem = GetComponentInChildren<ParticleSystem>();
        _movementSpeed = DataManager.Instance.currentLevel.shipThrusterSpeed;
        _rotationSpeed = DataManager.Instance.currentLevel.shipRotationSpeed;
    }
    
    private void InstantiateShipParticleSystem()
    {
        GameObject ShipInstantiatedSFX =
            Instantiate(shipInstantiatedParticleSystem, transform.position, Quaternion.identity);
        ShipInstantiatedSFX.GetComponent<ParticleSystem>().Play();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (shipState == State.Alive)
        {
            RocketControllerMovement();
            if (shipAction == Action.Flying)
            {
                RockectControllerRotation();
            }
        }
    }

    #region RocketShip Movement

    private void RocketControllerMovement()
    {
        //_verticalInput = Input.GetAxisRaw("Vertical");

        //moves rocketship according to input and coordinate system
        if (_verticalMovement > 0 && _hudControllerGameLevels.currentFuelBar > 0 && !DataManager.Instance.pauseMenuActive)
        {
            _rigidbody.AddRelativeForce(Vector3.up * (_verticalMovement * _movementSpeed), ForceMode.Force);
            _hudControllerGameLevels.UpdateFuelBar(_consumedFuel, false);

            if (!_rocketShipAudioSource.isPlaying)
            {
                _rocketShipAudioSource.PlayOneShot(AudioSFXController.DictionaryAudioClips["Rocket_Thruster"]);
                _thrusterParticleSystem.Play();

                //This is another way to play an array of audio clips. Replaced by a dictionary implementation.
                //_audioSource.PlayOneShot(_audioController.audioClips[0]);

                // foreach (var clip in _audioController.audioClips)
                // {
                //     if(clip.name == "Rocket_thruster_SFX")
                //             _audioSource.PlayOneShot(clip);
                // }
            }
        }

        else
        {
            _rocketShipAudioSource.Stop();
            _thrusterParticleSystem.Stop();
        }
    }
    
    private void RockectControllerRotation()
    {
        //_horizontalInput = Input.GetAxis("Horizontal");
        _rigidbody.freezeRotation = true;

        //rotates rocketship according to input
        transform.Rotate(Vector3.back, _horizontalMovement.x * Time.deltaTime * _rotationSpeed);
        _rigidbody.freezeRotation = false;
    }
    
    // private void RocketControllerMovement()
    // {
    //     _verticalInput = Input.GetAxisRaw("Vertical");
    //
    //     //moves rocketship according to input and coordinate system
    //     if (_verticalInput > 0 && _hudControllerGameLevels.currentFuelBar > 0 && !DataManager.Instance.pauseMenuActive)
    //     {
    //         _rigidbody.AddRelativeForce(Vector3.up * (_verticalInput * _movementSpeed), ForceMode.Force);
    //         _hudControllerGameLevels.UpdateFuelBar(_consumedFuel, false);
    //
    //         if (!_rocketShipAudioSource.isPlaying)
    //         {
    //             _rocketShipAudioSource.PlayOneShot(AudioSFXController.DictionaryAudioClips["Rocket_Thruster"]);
    //             _thrusterParticleSystem.Play();
    //
    //             //This is another way to play an array of audio clips. Replaced by a dictionary implementation.
    //             //_audioSource.PlayOneShot(_audioController.audioClips[0]);
    //
    //             // foreach (var clip in _audioController.audioClips)
    //             // {
    //             //     if(clip.name == "Rocket_thruster_SFX")
    //             //             _audioSource.PlayOneShot(clip);
    //             // }
    //         }
    //     }
    //
    //     else
    //     {
    //         _rocketShipAudioSource.Stop();
    //         _thrusterParticleSystem.Stop();
    //     }
    // }

    #endregion

    #region RocketShip Rotation
    
    #endregion

    #region Collision Detection

    private void OnCollisionEnter(Collision collision)
    {
        if (shipState != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "LaunchingPad":
                shipState = State.Alive;
                shipAction = Action.LandignPad;
#if ROCKETSHIPCONTROLLER_DEBUG
                Debug.Log("Ship is on LaunchingPad");
#endif
                break;

            case "LandingPad":
                shipAction = Action.LaunchingPad;
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                //_mainAudioSource.Stop();
                _rocketShipAudioSource.Stop();
                _thrusterParticleSystem.Stop();
                AudioSFXController.Main_Play_One_Shot_Audio("Landing_Pad_Reached");
                GameObject success = Instantiate(successParticleSystem, transform.position, Quaternion.identity);
                success.GetComponent<ParticleSystem>().Play();
                shipState = State.Transcending;

#if ROCKETSHIPCONTROLLER_DEBUG
                Debug.Log("Ship is on LandingPad");
#endif
                LandingPadReached(_currentLevel);
                break;

            case "Obstacle":

#if ROCKETSHIPCONTROLLER_DEBUG
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
                shipAction = Action.Flying;
#if ROCKETSHIPCONTROLLER_DEBUG
                Debug.Log("Ship is airborne");
#endif
                break;
        }
    }

    #endregion

    private void TakeHit()
    {
        _mainAudioSource.Stop();
        AudioSFXController.Main_Play_One_Shot_Audio("Death_Explosion");
        shipState = State.Dying;

        GameObject xp = Instantiate(explosionParticleSystem, transform.position, Quaternion.identity);
        xp.GetComponent<ParticleSystem>().Play();

        EventBroker.CallShipHitByObstacle();

        Destroy(gameObject);

        //Action event removed and replaced by event broker
        // if (HitByObstacle != null)
        // {
        //     HitByObstacle();
        // }
    }

    private void LandingPadReached(int loadNextLevel)
    {
        EventBroker.CallLandingPadReached(loadNextLevel);

        //Action event removed and replaced by event broker
        // if (OnLandingPad != null)
        // {
        //     OnLandingPad(level);
        // }
    }
}


