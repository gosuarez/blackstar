//#define ROCKETSHIPCONTROLLER_DEBUG

using TMPro;
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
    private float _maxSpeed;
    private float _rotationSpeed;
    private TMP_Text _text;

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
    private ParticleSystem _thrusterParticleSystem;
    private HUDControllerGameLevels _hudControllerGameLevels;
    private int _currentLevel = 0;
    private AudioSource _mainAudioSource;
    private AudioSource _rocketShipAudioSource;
    private AudioSFXController _audioSfxController;
    private int _consumedFuel = 1;
    private float _verticalMovement;
    private Vector2 _horizontalMovement;
    private UnityEngine.InputSystem.Gyroscope _gyroscope;

    //These two action events are removed as they were replaced by an event broker
    //public event Action HitByObstacle;
    //public event Action<int> OnLandingPad;

    #endregion

    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        _playerController = new PlayerController();
        _hudControllerGameLevels = FindObjectOfType<HUDControllerGameLevels>();
 
        _playerController.Gameplay.Thruster.performed += context => _verticalMovement = context.ReadValue<float>();
        _playerController.Gameplay.Thruster.canceled += context => _verticalMovement = 0;
        
        //ACTIVATE FOR PC INPUT (GAMEPAD/MOUSE AND KEYBOARD)
        _playerController.Gameplay.Rotation.performed += context => _horizontalMovement = context.ReadValue<Vector2>();
        _playerController.Gameplay.Rotation.canceled += context => _horizontalMovement = Vector2.zero;
        //ACTIVATE FOR PC INPUT (GAMEPAD/MOUSE AND KEYBOARD)
    }

    private void OnEnable()
    {
        _playerController.Gameplay.Enable();
        
        //ACTIVATE FOR MOBILE INPUT
        if (UnityEngine.InputSystem.Gyroscope.current != null)
        {
            _gyroscope = UnityEngine.InputSystem.Gyroscope.current;
            InputSystem.EnableDevice(_gyroscope);
        }
        //ACTIVATE FOR MOBILE INPUT
    }

    private void OnDisable()
    {
        _playerController.Gameplay.Disable();
        
        //ACTIVATE FOR MOBILE INPUT
        if (UnityEngine.InputSystem.Gyroscope.current != null)
        {
            _gyroscope = UnityEngine.InputSystem.Gyroscope.current;
            InputSystem.DisableDevice(_gyroscope);
        }
        //ACTIVATE FOR MOBILE INPUT
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InstantiateShipParticleSystem();
        _rigidbody = GetComponent<Rigidbody>();
        _audioSfxController = GameSceneController.Instance.GetComponent<AudioSFXController>();
        _mainAudioSource = _audioSfxController.GetComponent<AudioSource>();
        _rocketShipAudioSource = GetComponent<AudioSource>();
        _thrusterParticleSystem = GetComponentInChildren<ParticleSystem>();
        _movementSpeed = DataManager.Instance.currentLevel.shipThrusterSpeed;
        _maxSpeed = DataManager.Instance.currentLevel.shipThrusterMaxSpeed;
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
        //ACTIVATE FOR MOBILE INPUT
        if (_gyroscope != null)
        {
            _horizontalMovement = _gyroscope.angularVelocity.ReadValue();
        }
        //ACTIVATE FOR MOBILE INPUT
        
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
        if (_verticalMovement > 0 && _hudControllerGameLevels.currentFuelBar > 0 && !DataManager.Instance.pauseMenuActive)
         
        {
             _rigidbody.AddRelativeForce(Vector3.up * (_verticalMovement * _movementSpeed), ForceMode.Acceleration);

             _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);
             
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
    
    #endregion

    #region RocketShip Rotation
    
    private void RockectControllerRotation()
    {
        _rigidbody.freezeRotation = true;

        // //ACTIVATE FOR MOBILE INPUT
        if (_gyroscope != null)
        {
            transform.Rotate(Vector3.back, _horizontalMovement.y * Time.deltaTime * _rotationSpeed);
        }
        // //ACTIVATE FOR MOBILE INPUT
        // //ACTIVATE FOR MOBILE INPUT

        else
        {
            transform.Rotate(Vector3.back, _horizontalMovement.x * Time.deltaTime * _rotationSpeed);
        }
        
        _rigidbody.freezeRotation = false;
    }
    
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
                if (_hudControllerGameLevels.currentFuelBar == 0)
                {
                    TakeHit();
                }
                
#if ROCKETSHIPCONTROLLER_DEBUG
                Debug.Log("Ship is on LaunchingPad");
#endif
                break;

            case "LandingPad":
                shipAction = Action.LaunchingPad;
                _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
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


