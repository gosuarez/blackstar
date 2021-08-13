using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
            bestScoreText.text = $"Best Score: {DataManager.Instance.bestPlayer}: {DataManager.Instance.bestScore}";
        }
        else
        {
            bestScoreText.text = "Best Score: name: 0";
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
    
    public void LoadNewScene()
    {
        DataManager.Instance.currentLevelIndex++;
        DataManager.Instance.AssignLevelIndex(DataManager.Instance.currentLevelIndex);
        StartCoroutine(LoadScene(DataManager.Instance.currentLevelIndex));
    }
    
    public void ReStartGame(int levelIndex)
    {
        DataManager.Instance.AssignLevelIndex(levelIndex);
        DataManager.Instance.currentLevelIndex = levelIndex;
        DataManager.Instance.shipLives = 3;
        DataManager.Instance.currentScore = 0;
        StartCoroutine(LoadScene(DataManager.Instance.currentLevelIndex));
    }
    
    private IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(index);
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
