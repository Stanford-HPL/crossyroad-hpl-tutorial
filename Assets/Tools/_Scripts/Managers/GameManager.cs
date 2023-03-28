using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI.Metaviz.HPL.Demo;
using MetricCollection;
using UnityEngine;
using Object = System.Object;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State { get; private set; }
    public static event Action<GameState> OnGameStateChanged;

    public TargetDistractorTask GameTargetDistractorTask = new();
    
    /// <summary>
    /// The GameStates that the game is currently in. Make sure to update GameState each time a new GameState is reached.
    /// Use FILL_IN_GAME_STATE when in Unity Editor and making a prefab, and if this GameState is ever launched,
    /// then the player knows they need to fill in a game state.
    /// </summary>
    public enum GameState {
        Questionnaire,
        GameSelector,
        Game,
        End,
        Scores,
        
        FILL_IN_GAME_STATE,
    }

    /// <summary>
    /// Makes GameManager a singleton
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
    /// Set GameState to where the game should start
    /// </summary>
    private void Start()
    {
        UpdateGameState(GameState.Questionnaire);
    }
    
    /// <summary>
    /// Call this function everytime the GameState is changed in order to change the GameState.
    /// Once this function is called on a new state, then the Handle specific GameState function will trigger. 
    /// </summary>
    /// <param name="newState"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>

    public void UpdateGameState(GameState newState)
    {
        if (newState == State) return;

        State = newState;

        switch (newState)
        {
            case GameState.Questionnaire:
                HandleQuestionnaire();
                break;
            case GameState.GameSelector:
                HandleGameSelector();
                break;
            case GameState.Game:
                HandleGame();
                break;
            case GameState.End:
                HandleEnd();
                break;
            case GameState.Scores:
                HandleScores();
                break;
            case GameState.FILL_IN_GAME_STATE:
                throw new ArgumentException("PLEASE FILL IN GAME STATE IN ASSET");
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        
        OnGameStateChanged?.Invoke(newState);
    }

    /// <summary>
    /// Functions/events in this function will trigger when HandleQuestionnaire is called when
    /// GameState is changed to Questionnaire
    /// </summary>
    private void HandleQuestionnaire()
    {
        print("GM: Questionnaire");
        LevelManager.Instance.LoadNewScene("Questionnaire");
    } 
    
    /// <summary>
    /// Functions/events in this function will trigger when HandleGameSelector is called when
    /// GameState is changed to GameSelector
    /// </summary>
    private void HandleGameSelector()
     {
         print("GM: Game Selector");
         StartCoroutine(PostAndGetBatchMetadata());
     }

    private void HandleGame()
    {
        print("GM: Game");
    }

    /// <summary>
    /// Functions/events in this function will trigger when HandleEnd is called when
    /// GameState is changed to End
    /// </summary>
    private void HandleEnd()
    {
        print("GM: End");
        LevelManager.Instance.LoadNewScene("Ending");
        
        GameTargetDistractorTask.GetVpiScore((vpiScore) =>
        {
            VPIManager.Instance.ParseVPIScore(vpiScore);
            print("VPI Score: " + vpiScore);
        });
    }
    
    /// <summary>
    /// Functions/events in this function will trigger when HandleScore is called when
    /// GameState is changed to End
    /// </summary>
    private void HandleScores()
    {
        print("GM: Scores");
        LevelManager.Instance.LoadNewScene("Scores");
    }
    
    /// <summary>
    /// This function will grab the questionnaire metrics from the MetricCollectionManager, turning it into
    /// batch metadata. Then it POST and GET the batch metadata from the server
    /// </summary>
    /// <param name="callback">Optional Callback function</param>
    /// <returns></returns>
    private IEnumerator PostAndGetBatchMetadata(Action<string> callback = null)
    {
        // Fills the metaData dictionary with the questionnaire metrics
        Dictionary<string, Object> metaData = new Dictionary<string, Object>();
        for (var questionNumber = 1; questionNumber < MetricCollectionManager.Instance.QuestionnaireMetricsList.Count() + 1; questionNumber++)
        {
            metaData["Question " + questionNumber + ":"] = MetricCollectionManager.Instance.QuestionnaireMetricsList[questionNumber - 1];
        }
         
        BatchMetadata batchMetadata = new BatchMetadata(metadata: metaData, updateTime: DateTime.Now);
        yield return StartCoroutine(MetavizAPIManager.Instance.PostBatchMetadata(batchMetadata));
        MetavizAPIManager.Instance.BeginGetBatchMetadata(callback);
    }
}
