using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDControllerGameLevels : MonoBehaviour //IEndGameObserver interface removed and replaced by publisher pattern
{
    #region Field Declaration

    [Header("Set in Inspector")] [Space]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private StatusText statusText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private Image[] shipImages;
    [SerializeField] private FuelBar fuelBar;

    private int _totalPoints;

    public int maxFuel;
    public int currentFuelBar;
    
    #endregion

    #region Startup

    private void Awake()
    {
        statusText.gameObject.SetActive(false);
        ShipFullTank();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        EventBroker.ShipFullTank += ShipFullTank;
        EventBroker.CoinSelected += UpdateScore;
        EventBroker.FuelSelected += UpdateFuelBar;
        EventBroker.LifeLost += HideShip;
        EventBroker.GameOver += EventBrokerOnGameOver;
        EventBroker.ActivateRestartButton += EventBrokerOnActivateRestartButton;

        if (DataManager.Instance != null)
        {
            ShowStatus(DataManager.Instance.currentLevel.levelName, false);
        }
        
        //Event action replaced by event broker
        //GameSceneController.LifeLost += HideShip;

        //Implemented to notify that a live is lost (Observer Pattern)
        //_gameSceneController.AddObserver(this);
    }

    public void ReStartGame()
    {
        DataManager.Instance.ReStartGame();
    }

    private void EventBrokerOnActivateRestartButton()
    {
        restartButton.SetActive(true);
    }

    #endregion
    
    #region Methods

    private void ShipFullTank()
    {
        currentFuelBar = maxFuel;
        fuelBar.SetMaxFuel(maxFuel);
    }
    
    public void UpdateScore(int pointValue, bool powerUp)
    {
        if (powerUp)
        {
            _totalPoints += pointValue;
            scoreText.text = _totalPoints.ToString("D5");
        }
    }

    public void UpdateFuelBar(int fuelValue, bool powerUp)
    {
        if (!powerUp)
        {
            currentFuelBar -= fuelValue;
            fuelBar.SetFuelValue(currentFuelBar);
        }

        if (powerUp && currentFuelBar < maxFuel)
        {
            currentFuelBar += fuelValue;
            if (currentFuelBar > maxFuel)
            {
                currentFuelBar = maxFuel;
            }
            fuelBar.SetFuelValue(currentFuelBar);
        }
    }
    
    public void ShowStatus(string newStatus, bool gameOver)
    {
        statusText.gameObject.SetActive(true);
        StartCoroutine(statusText.ChangeStatus(newStatus, gameOver));
    }

    public void HideShip(int imageIndex)
    {
        shipImages[imageIndex].gameObject.SetActive(false);
    }

    public void ResetShip()
    {
        foreach (var shipImage in shipImages)
        {
            shipImage.gameObject.SetActive(true);
        }
    }
    
    private void EventBrokerOnGameOver()
    {
        ShowStatus("Game Over", true);
    }
    
    private void OnDisable()
    {
        EventBroker.CoinSelected -= UpdateScore;
        EventBroker.FuelSelected -= UpdateFuelBar;
        EventBroker.LifeLost -= HideShip;
        EventBroker.GameOver -= EventBrokerOnGameOver;
        EventBroker.ShipFullTank -= ShipFullTank;
        EventBroker.ActivateRestartButton -= EventBrokerOnActivateRestartButton;
    }

    // public void ReStartGame()
    // {
    //     //GameSceneController.Instance.ReStartGame(_indexFirstScene);
    // }

    #endregion

    // Observer pattern implementation replaced by event broker
    // public void Notify()
    // {
    //     ShowStatus("Game Over");
    // }
}
