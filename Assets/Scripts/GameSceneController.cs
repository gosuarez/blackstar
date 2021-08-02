#define GAMESCENECONTROLLER_DEBUG

using System.Collections;
using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    #region Singleton Declaration
    
    private static GameSceneController _instance;

    public static GameSceneController Instance
    {
        get 
        
        {
            if (_instance == null)
            {
                GameObject gO = new GameObject("GameSceneController");
                gO.AddComponent<GameSceneController>();
            }
            return _instance;        
        }  
    }

    #endregion

    //Example of the implementation of an action(event), which was replaced by the event broker (publisher subscriber)
    //public static Action<int> LifeLost;
    
    #region Field Declarations

    [Header("Set in Inspector")]
    [SerializeField] private RocketShipController rocketShipPrefab;
    [SerializeField] private Transform rocketShipRoot;
    [SerializeField] private Transform launchingPad;
    [Space]
    [SerializeField] private PowerUpController[] powerUpPrefabs;
    [SerializeField] private float timeToRespawnPowerUp;
    [SerializeField] private Animator obstacleAnimator;
    
    [Header("Set Dynamically")]
    [SerializeField] private int lives = 3;
    [SerializeField] private int nextLevel;
    private WaitForSeconds _shipSpawnDelay = new WaitForSeconds(3f);
    private Vector3 _rocketShipOrigingPoint;
    private float _shipOriginOffsetYPosition = 2f;
    private enum GameLevels {Level0, Level1, Level2}
    private GameLevels _currentLevel;
    private static readonly int Obstacle = Animator.StringToHash("MoveObstacle");

    #endregion

    //This is an example of the observer pattern implementation. Replaced by the publisher subscriber pattern.
    #region Subject Implementation

    // private List<IEndGameObserver> _endGameObservers;
    //
    // public void AddObserver(IEndGameObserver observer)
    // {
    //     _endGameObservers.Add(observer);
    // }
    //
    // public void RemoveObserver(IEndGameObserver observer)
    // {
    //     _endGameObservers.Remove(observer);
    // }
    //
    // private void NotifyObservers()
    // {
    //     foreach (var observer in _endGameObservers)
    //     {
    //         observer.Notify();
    //     }
    // }
    
    #endregion

    #region Startup

    private void Awake()
    {
        _instance = this;
        MoveObstacle(true);
        
        //Part of the same implementation explained above (Observer Pattern)
        //_endGameObservers = new List<IEndGameObserver>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rocketShipRoot.position = launchingPad.position;
        Vector3 rocketShipPosition = rocketShipRoot.position;
        _rocketShipOrigingPoint = new Vector3(rocketShipPosition.x, (rocketShipPosition.y + _shipOriginOffsetYPosition), rocketShipPosition.z);
        
        EventBroker.LandingPadReached += RocketShipPrefab_OnLandingPad;
        EventBroker.ShipHitByObstacle += Ship_HitByObstacle;

        StartCoroutine(SpawnRocketShip(false));

        if (DataManager.Instance != null)
        {
            if (DataManager.Instance.currentLevel.hasPowerUps)
            {
                StartCoroutine(SpawnPowerUP());
            }
        }
    }
    
    #endregion

    #region Mehtods
    
    private void RocketShipPrefab_OnLandingPad(int level)
    {
        DataManager.Instance.LoadNewScene();
    }
    
    private IEnumerator SpawnRocketShip(bool delayed)
    {
        if (delayed)

        {
            yield return _shipSpawnDelay;
        }

        RocketShipController ship = Instantiate(rocketShipPrefab, _rocketShipOrigingPoint, Quaternion.identity);
        AudioController.Main_Play_One_Shot_Audio("Ship_Instantiated");
        EventBroker.CallShipFullTank();
        ship.transform.SetParent(rocketShipRoot.transform);
        
        //These two implementation where removed and replaced by the event broker
        //ship.HitByObstacle += Ship_HitByObstacle;
        //ship.OnLandingPad += _rocketShipPrefab_OnLandingPad;

        yield return null;
    }

    private void Ship_HitByObstacle()
    {
        lives--;
        
#if GAMESCENECONTROLLER_DEBUG
        print("Ship hit by obstacle");
#endif
        
        //Action event implementation replaced by event broker
        //LifeLost?.Invoke(lives);
        
        EventBroker.CallLifeLost(lives);
        
        if (lives > 0)
        {
            StartCoroutine(SpawnRocketShip(true));
        }

        else
        {
            
#if GAMESCENECONTROLLER_DEBUG
            print("All lives lost, Game Over Call");
#endif
            EventBroker.CallGameOver();
            StopAllCoroutines();
            obstacleAnimator.speed = 0;
            //Notify observers that the ship does not have more lives (Observer Pattern).
            //NotifyObservers();
        }
    }

    private void MoveObstacle(bool moveObstacle)
    {
        //Todo: this action will have to be improved as more animations need to be triggered.
        obstacleAnimator.SetBool(Obstacle, moveObstacle);
    }

    private IEnumerator SpawnPowerUP()
    {
        while (true)
        {
            int index = UnityEngine.Random.Range(0, powerUpPrefabs.Length);
            Vector2 spawnPosition = ScreenBounds.RandomTopPosition();
            PowerUpController powerUp = Instantiate(powerUpPrefabs[index], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(timeToRespawnPowerUp);
        }
    }
    
    private void OnDisable()
    {
        EventBroker.LandingPadReached -= RocketShipPrefab_OnLandingPad;
        EventBroker.ShipHitByObstacle -= Ship_HitByObstacle;
    }

    #endregion
}
