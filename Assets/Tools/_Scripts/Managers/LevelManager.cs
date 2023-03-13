using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    /// <summary>
    /// Makes LevelManager a singleton
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    
    /// <summary>
    /// Call this function to load a new scene.
    /// Insert the parameter, "sceneName," exactly as the scene's name is in the Scene's folder
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadNewScene(string sceneName)
    {
        if (sceneName == "INPUT YOUR SCENE NAME")
        {
            throw new ArgumentException("PLEASE FILL IN YOUR SCENE NAME ON THIS GAME OBJECT SCRIPT " + gameObject.name);
        }
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// This function is used to change to the GameSelectorScene in the Unity Inspector.
    /// Feel free to make other functions like this if utilizing buttons or similar
    /// that do not allow updating of GameState
    /// </summary>
    public void SwapToGameSelectorScene()
    {
        GameManager.Instance.UpdateGameState(GameManager.GameState.Questionnaire);
    }
}
