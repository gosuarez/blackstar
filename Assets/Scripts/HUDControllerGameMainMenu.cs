using System;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HUDControllerGameMainMenu : MonoBehaviour
{
    public string currentPlayer;
    private Gamepad _gamepad;
    public Text bestScoreText;
    public GameObject uIKeyboard;
    private InputField _inputField;
    public Button startButton;
    private string _name;

    private void Start()
    {
        UpdateScore();
        _inputField = GetComponentInChildren<InputField>();
    }
    
    public void GetGamePad()
    {
        if (InputSystem.GetDevice<Gamepad>() != null)
        {
            _gamepad = InputSystem.GetDevice<Gamepad>();
        }
    }

    public void ActivateKeyboard()
    {
        if (_gamepad != null &&  _gamepad.aButton.wasPressedThisFrame)
        {
            _inputField.DeactivateInputField();
            uIKeyboard.SetActive(true);
        }
    }

    public void CancelKeyboard()
    {
        uIKeyboard.SetActive(false);
        startButton.Select();
    }

    public void AddAlphabetToInputField(string alphabet)
    {
        _name += alphabet;
        _inputField.text = _name;
    }
    
    public void RemoveAlphabetToInputField()
    {
        if (_name.Length != 0)
        {
            _name = _name.Substring(0, _name.Length - 1);
            _inputField.text = _name;
        }
    }
    
    public void CurrentPlayerName()
    {
        DataManager.Instance.currentPlayer = _inputField.text;
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
