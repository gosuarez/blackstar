using System;

public class EventBroker
{
    public static event Action<int> LifeLost ;
    public static event Action GameOver;
    public static event Action<int> LandingPadReached;
    public static event Action ShipHitByObstacle;
    public static event Action ShipFullTank;
    public static event Action ActivateRestartButton;
    public static event Action<int, bool> CoinSelected;
    public static event Action<int, bool> FuelSelected;

    //Call when a life is lost
    public static void CallLifeLost(int live)
    {
        LifeLost?.Invoke(live);
    }

    //Call when all life are lost
    public static void CallGameOver()
    {
        GameOver?.Invoke();
    }
    
    //Call when ship reaches the landing pad
    public static void CallLandingPadReached(int loadNextLevel)
    {
        LandingPadReached?.Invoke(loadNextLevel);
    }

    //call when ship is hit by an obstacle
    public static void CallShipHitByObstacle()
    {
        ShipHitByObstacle?.Invoke();
    }

    public static void CallShipFullTank()
    {
        ShipFullTank?.Invoke();
    }

    public static void CallActivateRestartButton()
    {
        ActivateRestartButton?.Invoke();
    }

    public static void CallCoinSelected(int coinPoint, bool powerUp)
    {
        CoinSelected?.Invoke(coinPoint, powerUp);
    }

    public static void CallFuelSelected(int fuelPoint, bool powerUp)
    {
        FuelSelected?.Invoke(fuelPoint, powerUp);
    }
    
}
