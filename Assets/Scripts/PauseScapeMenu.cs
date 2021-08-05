using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseScapeMenu : MonoBehaviour
{
    private bool _showMenu;
    public GameObject pauseScapeMenu;
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu();
            print("showing menu");
        }
    }

    private void ShowPauseMenu()
    {
        if (!_showMenu)
        {
            pauseScapeMenu.SetActive(true);
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
    
    public void Exit()
    {

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else

        Application.Quit();
#endif
    }
    
}
