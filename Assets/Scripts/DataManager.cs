using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public string currentPlayer;
    public string bestPlayer;
    public int currentScore;
    public int bestScore;
    public int shipLives = 3;
    public List<LevelDefinition> levels;
    public int currentLevelIndex = 0;
    public LevelDefinition currentLevel;
    public bool pauseMenuActive;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        AssignLevelIndex(currentLevelIndex);
    }
    
    public void AssignLevelIndex(int levelIndex)
    {
        currentLevel = levels[levelIndex];
    }
    
    public void LoadNewScene()
    {
        currentLevelIndex++;
        AssignLevelIndex(currentLevelIndex);
        StartCoroutine(LoadSceneAfterTime(currentLevelIndex));
    }
    
    public void ReStartGame(int levelIndex)
    {
        AssignLevelIndex(levelIndex);
        currentLevelIndex = levelIndex;
        shipLives = 3;
        currentScore = 0;
        StartCoroutine(LoadSceneAfterTime(currentLevelIndex));
    }

    public void LoadMainMenu(int levelIndex)
    {
        AssignLevelIndex(levelIndex);
        currentLevelIndex = levelIndex;
        shipLives = 3;
        currentScore = 0;
        SceneManager.LoadScene(levelIndex); 
    }

    private IEnumerator LoadSceneAfterTime(int index)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(index);
    }
}
