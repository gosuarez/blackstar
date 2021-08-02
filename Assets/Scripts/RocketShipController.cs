#define ROCKETSHIPCONTROLLER_DEBUG

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RocketShipController : MonoBehaviour
{
    #region Variables Declaration

    [Header("Set in Inspector")] [Range(0, 15)] [SerializeField]
    private float movementSpeed;

    [Range(0, 300)] [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject explosionParticleSystem;
    [SerializeField] private GameObject successParticleSystem;
    [SerializeField] private GameObject shipInstantiatedParticleSystem;
    [SerializeField] private GameObject powerUpParticleSystem;

    public enum State
    {
        Alive,
        Dying,
        Transcending
    }

    public State shipState;

    private Rigidbody _rigidbody;
    private float _horizontalInput;
    private float _verticalInput;
    private ParticleSystem _thrusterParticleSystem;
    private HUDControllerGameLevels _hudControllerGameLevels;
    private int _currentLevel = 0;
    private AudioSource _mainAudioSource;
    private AudioSource _rocketShipAudioSource;
    private AudioController _audioController;
    private int _consumedFuel = 1;

    //These two action events are removed as they were replaced by an event broker
    //public event Action HitByObstacle;
    //public event Action<int> OnLandingPad;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        InstantiateShipParticleSystem();
        
        _hudControllerGameLevels = FindObjectOfType<HUDControllerGameLevels>();
        _rigidbody = GetComponent<Rigidbody>();
        _audioController = GameSceneController.Instance.GetComponent<AudioController>();
        _mainAudioSource = _audioController.GetComponent<AudioSource>();
        _rocketShipAudioSource = GetComponent<AudioSource>();
        _thrusterParticleSystem = GetComponentInChildren<ParticleSystem>();
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
            RockectControllerRotation();
        }
    }

    #region RocketShip Movement

    private void RocketControllerMovement()
    {
        _verticalInput = Input.GetAxisRaw("Vertical");

        //moves rocketship according to input and coordinate system
        if (_verticalInput > 0 && _hudControllerGameLevels.currentFuelBar > 0)
        {
            _rigidbody.AddRelativeForce(Vector3.up * (_verticalInput * movementSpeed), ForceMode.Force);
            _hudControllerGameLevels.UpdateFuelBar(_consumedFuel, false);

            if (!_rocketShipAudioSource.isPlaying)
            {
                _rocketShipAudioSource.PlayOneShot(AudioController.DictionaryAudioClips["Rocket_Thruster"]);
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
        _horizontalInput = Input.GetAxis("Horizontal");
        _rigidbody.freezeRotation = true;

        //rotates rocketship according to input
        transform.Rotate(Vector3.back, _horizontalInput * Time.deltaTime * rotationSpeed);
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

#if ROCKETSHIPCONTROLLER_DEBUG
                Debug.Log("Ship is on LaunchingPad");
#endif
                break;

            case "LandingPad":
                _mainAudioSource.Stop();
                AudioController.Main_Play_One_Shot_Audio("Landing_Pad_Reached");
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
        AudioController.Main_Play_One_Shot_Audio("Death_Explosion");
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


