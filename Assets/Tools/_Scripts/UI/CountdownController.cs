using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CountdownController : MonoBehaviour
    {
        [Tooltip("Choose the Game State that will start after countdown")]
        [SerializeField] private GameManager.GameState gameState;
        
        [Header("Countdown")]
        [SerializeField] private int countdownStartTime;
        [SerializeField] private TMP_Text countdownDisplay;
        
        [Space]
        [Header("Game Timer")]
        [SerializeField] private TMP_Text gameTimerDisplay;
        
        private int _amountOfTime = 45;

        /// <summary>
        /// When scene loads, the Countdown starts
        /// </summary>
        private void Start()
        {
            _amountOfTime = GameManager.Instance.GameLength;
            StartCoroutine(CountdownToStartGameCoroutine());
        }

        /// <summary>
        /// Counts down from countdownStartTime till it hits 0, then game starts by updating GameState
        /// </summary>
        private IEnumerator CountdownToStartGameCoroutine()
        {
            countdownDisplay.gameObject.SetActive(true);
            
            while (countdownStartTime > 0)
            {
                countdownDisplay.text = countdownStartTime.ToString();
                yield return new WaitForSeconds(1f);
                countdownStartTime -= 1;
            }

            yield return null;
            countdownDisplay.gameObject.SetActive(false);
            GameManager.Instance.UpdateGameState(gameState);
            StartCoroutine(GameTimerCoroutine());
        }
        
        /// <summary>
        /// This is the in-game timer, that counts down from timeLeft, which starts from _amountOfTime.
        /// Once the timer reaches 0, the game ends, and the GameState is changed to End.
        /// </summary>
        private IEnumerator GameTimerCoroutine()
        {
            gameTimerDisplay.gameObject.SetActive(true);
            var timeLeft = _amountOfTime;
            
            while (timeLeft > 0)
            {
                gameTimerDisplay.text = "Time Left: " + timeLeft;
                yield return new WaitForSeconds(1f);
                timeLeft -= 1;
            }
            
            gameTimerDisplay.gameObject.SetActive(false);
            GameManager.Instance.UpdateGameState(GameManager.GameState.End);
        }
        
    }
}

