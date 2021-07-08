#define GAMESCENECONTROLLER_DEBUG

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    #region Field Declarations

    [Header("Set in Inspector")]
    [SerializeField] 
    private RocketShipController _rocketShipPrefab;

    [SerializeField] 
    private Transform _rocketShipRoot;

    [SerializeField] 
    private Transform _launchingPad;

    [Header("Set Dinamically")]
    [SerializeField] 
    private int _lives = 3;

    private WaitForSeconds shipSpawnDelay = new WaitForSeconds(2f);
    private Vector3 rocketShipOrigingPoint;
    private float shipOriginOffsetYPosition = 2f;

    private enum GameLevels {Level0, Level1, Level2}
    private GameLevels currentLevel;
    [SerializeField] private int _nextLevel;

    #endregion


    private void Awake()
    {
        LoadLevel();
    }

    // Start is called before the first frame update
    void Start()
    {
        _rocketShipRoot.position = _launchingPad.position;
        Vector3 _rocketShipPosition = _rocketShipRoot.position;
        rocketShipOrigingPoint = new Vector3(_rocketShipPosition.x, _rocketShipPosition.y + shipOriginOffsetYPosition, _rocketShipPosition.z);

        StartCoroutine(SpawnRocketShip(false));
    }

    private void LoadLevel()
    {
        var scene = SceneManager.GetActiveScene();

        if (scene == SceneManager.GetSceneByName("Level 0"))
        {
            currentLevel = GameLevels.Level0;
            _nextLevel = 1;
#if GAMESCENECONTROLLER_DEBUG
            Debug.Log("we are on level 0");
#endif
        }

        if (scene == SceneManager.GetSceneByName("Level 1"))
        {
            currentLevel = GameLevels.Level1;
            _nextLevel = 2;
#if GAMESCENECONTROLLER_DEBUG
            Debug.Log("we are on level 1");
#endif
        }

        if (scene == SceneManager.GetSceneByName("Level 2"))
        {
            currentLevel = GameLevels.Level2;
            _nextLevel = 3;
#if GAMESCENECONTROLLER_DEBUG
            Debug.Log("we are on level 2");
#endif
        }
    }

    private void _rocketShipPrefab_OnLandingPad(int level)
    {
        level = this._nextLevel;
        StartCoroutine(LoadScene(level));
    }

    private IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(index);
    }

    private IEnumerator SpawnRocketShip(bool delayed)
    {
        if (delayed)

        {
            yield return shipSpawnDelay;
        }

        RocketShipController ship = Instantiate(_rocketShipPrefab, rocketShipOrigingPoint, Quaternion.identity);
        ship.transform.SetParent(_rocketShipRoot.transform);
        ship.HitByObtacle += Ship_HitByObstacle;
        ship.OnLandingPad += _rocketShipPrefab_OnLandingPad;

        yield return null;
    }

    private void Ship_HitByObstacle()
    {
        _lives--;

        if (_lives > 0)
        {
            StartCoroutine(SpawnRocketShip(true));
        }
    }
}
