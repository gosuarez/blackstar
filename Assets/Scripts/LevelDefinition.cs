using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Level Definition", fileName = "NewLevelDefinition")]
public class LevelDefinition : ScriptableObject
{
    public string levelName;
    public bool mainMenu;
    public bool hasPowerUps;
    public float powerUpMinimumWait;
    public float powerUpMaximumWait;
    public float shipThrusterSpeed;
    public float shipRotationSpeed;
}
