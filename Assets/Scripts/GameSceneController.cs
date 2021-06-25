using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneController : MonoBehaviour
{
    #region Field Declarations

    [Header("Set in Inspector")]
    [SerializeField] private RocketShipController _rocketShipPrefab;
    [SerializeField] private GameObject _origin;

    [Header("Set Dinamically")]
    [SerializeField] private int _lives = 3;

    private WaitForSeconds shipSpawnDelay = new WaitForSeconds(2f);

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRocketShip(false));
    }

    private IEnumerator SpawnRocketShip(bool delayed)
    {
        if (delayed)

        {
            yield return shipSpawnDelay;
        }
              
        RocketShipController ship = Instantiate(_rocketShipPrefab, _origin.transform.position, Quaternion.identity);
        ship.transform.SetParent(_origin.transform);
        ship.HitByObtacle += Ship_HitByObstacle;

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
