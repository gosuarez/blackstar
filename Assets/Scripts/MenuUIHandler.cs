using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    private string currentPlayer;
    public Text bestScoreText;

    private void Start()
    {
        UpdateScore();
    }

    public void CurrentPlayerName(string text)
    {
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

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else

        Application.Quit();
#endif
    }
}
