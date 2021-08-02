using UnityEngine;

//This is a delegate example. I'll be using action events instead.
//public delegate void PowerUpPointHandler(int pointValue, bool powerUp);
public class PowerUpController : MonoBehaviour //Interface remove and replace by the publisher pattern IEndGameObserver
{
    //These are only examples using the delegate type
    // public static event PowerUpPointHandler CoinSelected;
    // public static event PowerUpPointHandler FuelSelected;
    
    //These are examples of action types replaced by an event broker
    //public static Action<int, bool> CoinSelected;
    //public static Action<int, bool> FuelSelected;
    
    #region Field Declaration

    public enum PowerType
    {
        Coin,
        Fuel
    };

    [SerializeField] private PowerType powerType;
    [SerializeField] private Vector3 rotationRate;
    [SerializeField] private GameObject powerUpParticleSystem;
    
    public int coinPoint;
    public int fuelPoint;
    
    private bool _powerUp;
    
    #endregion

    private void Start()
    {
        EventBroker.GameOver += RemoveAndDestroy;
        //Observer pattern implementation replaced by publisher pattern 
        //GameSceneController.Instance.AddObserver(this);
    }

    private void OnDisable()
    {
        EventBroker.GameOver -= RemoveAndDestroy;
    }

    private void RemoveAndDestroy()
    {
        Destroy(gameObject);
    }

    #region Movement
    
    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    private void Move()
    {
        if (powerType == PowerType.Fuel)
        {
            transform.Translate(Vector2.down * (Time.deltaTime * 3), Space.World);

            if (ScreenBounds.OutOfBounds(transform.position))
            {
                //Observer pattern implementation replaced by publisher pattern
                //RemoveAndDestroy();
                
                RemoveAndDestroy();
                
            }
        }

        if (powerType == PowerType.Coin)
        {
            transform.Rotate(rotationRate * Time.deltaTime);
        }
    }

    #endregion

    #region Collisions

    private void OnTriggerEnter(Collider other)
    {
        if (powerType == PowerType.Coin)
        {
            EventBroker.CallCoinSelected(coinPoint, true);
            AudioController.Main_Play_One_Shot_Audio("Coin_Selected");
            
            //Observer pattern implementation replaced by publisher pattern
            //RemoveAndDestroy();
            
        }

        if (powerType == PowerType.Fuel)
        {
            EventBroker.CallFuelSelected(fuelPoint, true);
            AudioController.Main_Play_One_Shot_Audio("Fuel_Selected");
            
            //Observer pattern implementation replaced by publisher pattern
            //RemoveAndDestroy();
        }
        
        GameObject powerUpSFX =
            Instantiate(powerUpParticleSystem, transform.position, Quaternion.identity);
        powerUpSFX.GetComponent<ParticleSystem>().Play();
        
        RemoveAndDestroy();
    }
    
    // private void RemoveAndDestroy()
    // {
    //     //Observer pattern implementation replaced by publisher pattern 
    //     //GameSceneController.Instance.RemoveObserver(this);
    // }

    #endregion

    //Observer pattern implementation replaced by publisher pattern. This is call when the user has lost all lives
    // public void Notify()
    // {
    //     Destroy(gameObject);
    // }
}
