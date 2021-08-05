using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class HUDControllerGameMainMenu : MonoBehaviour
{
    public string currentPlayer;
    public Text bestScoreText;

    private void Start()
    {
        UpdateScore();
    }

    public void CurrentPlayerName(string text)
    {
        currentPlayer = text;
        DataManager.Instance.currentPlayer = text;
    }
    
    public void UpdateScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            DataManager.Instance.bestPlayer = data.bestPlayer;
            DataManager.Instance.bestScore = data.bestScore;
        }

        // Updates score if loaded or reseted it
        if (DataManager.Instance != null)
        {
            bestScoreText.text = $"Best Score: {DataManager.Instance.bestPlayer} : {DataManager.Instance.bestScore}";
        }
    }

    public void ResetBestScore()
    {
        SaveData data = new SaveData();

        data.bestScore = 0;
        data.bestPlayer = "Name";

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        UpdateScore();
    }
    
    // public static void SaveData()
    // {
    //     SaveData data = new SaveData();
    //     
    //     data.bestScore = DataManager.Instance.bestScore;
    //     data.bestPlayer = DataManager.Instance.bestPlayer;
    //
    //     string json = JsonUtility.ToJson(data);
    //     
    //     File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    // }
    
    // public void CheckBestScoreBeforeExit()
    // {
    //     //Checks if the current score is higher than bestscore. If higher, save data.
    //     if (DataManager.Instance.currentScore > DataManager.Instance.bestScore)
    //     {
    //         DataManager.Instance.bestScore = DataManager.Instance.currentScore;
    //         DataManager.Instance.bestPlayer = currentPlayer;
    //         //bestScoreText.text = $"Best Score: {DataManager.Instance.bestPlayer}: { DataManager.Instance.bestScore}";
    //         SaveData();
    //     }
    //     return;
    // }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else

        Application.Quit();
#endif
    }
}
