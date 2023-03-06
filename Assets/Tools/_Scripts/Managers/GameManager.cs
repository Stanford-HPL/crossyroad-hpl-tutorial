using System;
using MetricCollection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State { get; private set; }
    public static event Action<GameState> OnGameStateChanged;

    public int GameLength = 45;
    
    private MetricInformation.StaticMetrics _staticMetrics;
    
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
        LevelManager.Instance.LoadNewScene("GameSelector");
    } 
    
    /// <summary>
    /// Functions/events in this function will trigger when HandleGameSelector is called when
    /// GameState is changed to GameSelector
    /// </summary>
    private void HandleGameSelector()
     {
         print("GM: Game Selector");
         PrintQuestionnaireMetrics();
         PrintStaticMetrics();
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

        PrintAllMetrics();
    }

    /// <summary>
    /// This function will print every Metric in the console
    /// </summary>
    private void PrintAllMetrics()
    {
        PrintQuestionnaireMetrics();
        PrintStaticMetrics();
        PrintDynamicMetrics();
    }
    
    /// <summary>
    /// This function will print every Questionnaire Metric in the console
    /// </summary>
    private void PrintQuestionnaireMetrics()
    {
        print("");
        print("The Questionnaire Metrics Are:");
        foreach (var questionMetric in MetricCollectionManager.Instance.QuestionnaireMetricsList)
        {
            print("Question:" + questionMetric.Question);
            print("Selected Answer:" + questionMetric.Answer);
            print("");
        }
        print("");
    }

    /// <summary>
    /// This function will print every Static Metric in the console
    /// </summary>
    private void PrintStaticMetrics()
    {
        print("");
        print("The Static Metrics Are:");
        print("Screen Resolution: " + MetricCollectionManager.Instance.StaticMetrics.ScreenResolution);
        print("Screen DPI: " + MetricCollectionManager.Instance.StaticMetrics.ScreenDPI);
        print("");
    }

    /// <summary>
    /// This function will print every Dynamic Metric in the console
    /// </summary>
    private void PrintDynamicMetrics()
    {
        print("");
        print("The Dynamic Metrics Are:");
        foreach (var dynamicMetric in MetricCollectionManager.Instance.DynamicMetricsList)
        {
            print("Time:" + dynamicMetric.Time);
            print("Is Target:" + dynamicMetric.IsTarget);
            print("Is Clicked: " + dynamicMetric.IsInteracted);
            //INPUT PSYCHOMETRICS HERE
            print("Enemy Position: " + dynamicMetric.EnemyPosition);
            print("Enemy Move Speed: " + dynamicMetric.EnemyMoveSpeed);
            print("");
        }
        print("");
    }
}
