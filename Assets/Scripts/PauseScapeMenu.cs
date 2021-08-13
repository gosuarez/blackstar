using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseScapeMenu : MonoBehaviour
{
    private bool _showMenu;
    private PlayerController _playerController;

    [SerializeField] private GameObject pauseScapeMenu;
    [SerializeField] private Button continueButton;

    private void Awake()
    {
        _playerController = new PlayerController();
        _playerController.Gameplay.PauseMenu.canceled += context => ShowPauseMenu();
    }
    
    
    private void OnEnable()
    {
        _playerController.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _playerController.Gameplay.Disable();
    }
    
    private void ShowPauseMenu()
    {
        if (!_showMenu)
        {
            pauseScapeMenu.SetActive(true);
            continueButton.Select();
            Time.timeScale = 0f;
            _showMenu = true;
            DataManager.Instance.pauseMenuActive = _showMenu;
        }
        
        else if (_showMenu)
        {
            Continue();
        }
    }

    public void Continue()
    {
        pauseScapeMenu.SetActive(false);
        Time.timeScale = 1f;
        _showMenu = false;   
        DataManager.Instance.pauseMenuActive = _showMenu;
    }

    public void MainMenu()
    {
        pauseScapeMenu.SetActive(false);
        Time.timeScale = 1;
        if (GameSceneController.Instance.ship != null)
        {
            GameSceneController.Instance.ship.GetComponent<Rigidbody>().useGravity = false;
            GameSceneController.Instance.ship.GetComponent<Rigidbody>().isKinematic = true;
        }
        _showMenu = false;
        DataManager.Instance.pauseMenuActive = _showMenu;
        DataManager.Instance.ReStartGame(0);
    }


    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else

        Application.Quit();
#endif
    }
    
}
