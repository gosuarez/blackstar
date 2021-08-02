using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public string currentPlayer;
    public string bestPlayer;
    public int bestScore;
    public List<LevelDefinition> levels;
    public int currentLevelIndex = 0;
    public LevelDefinition currentLevel;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        AssignLevelIndex(currentLevelIndex);
    }
    
    private void AssignLevelIndex(int levelIndex)
    {
        currentLevel = levels[levelIndex];
    }
    
    public void LoadNewScene()
    {
        currentLevelIndex++;
        AssignLevelIndex(currentLevelIndex);
        StartCoroutine(LoadScene(currentLevelIndex));
    }
    
    public void ReStartGame()
    {
        StartCoroutine(LoadScene(currentLevelIndex));
    }
    
    private IEnumerator LoadScene(int index)
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(index);
    }
}
