using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Level Definition", fileName = "NewLevelDefinition")]
public class LevelDefinition : ScriptableObject
{
    public string levelName;
    public bool mainMenu;
    public bool endGame;
    public bool hasPowerUps;
    public float powerUpMinimumWait;
    public float powerUpMaximumWait;
    [Range(0, 30)] public float shipThrusterSpeed;
    [Range(0, 30)] public float shipThrusterMaxSpeed;
    [Range(0, 300)] public float shipRotationSpeed;
    public float obstacleSpeed;
}
