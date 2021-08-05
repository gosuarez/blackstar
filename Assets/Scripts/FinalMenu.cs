using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FinalMenu : MonoBehaviour
{
    public Canvas finalMenu;
    public bool showMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        finalMenu = gameObject.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowFinalMenu();
        }
    }

    private void ShowFinalMenu()
    {
  
            finalMenu.enabled = !finalMenu.enabled;
            showMenu = !showMenu;
    }
    
    public static void SaveData()
    {
        SaveData data = new SaveData();
        
        data.bestScore = DataManager.Instance.bestScore;
        data.bestPlayer = DataManager.Instance.bestPlayer;
    
        string json = JsonUtility.ToJson(data);
        
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void TryAgain(int index)
    {
        CheckBestScoreBeforeExit();
        DataManager.Instance.ReStartGame(1);
        //DataManager.Instance.currentScore = 0;
        //DataManager.Instance.AssignLevelIndex(1);
        //SceneManager.LoadScene(index);
    }


    public void CheckBestScoreBeforeExit()
    {
        //Checks if the current score is higher than bestscore. If higher, save data.
        if (DataManager.Instance.currentScore > DataManager.Instance.bestScore)
        {
            DataManager.Instance.bestScore = DataManager.Instance.currentScore;
            DataManager.Instance.bestPlayer = DataManager.Instance.currentPlayer;
            SaveData();
        }
        return;
    }
    
    public void Exit()
    {
        CheckBestScoreBeforeExit();
        
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else

        Application.Quit();
#endif
    }
    
}
