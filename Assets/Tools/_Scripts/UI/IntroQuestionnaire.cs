using System.Collections.Generic;
using MetricCollection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class IntroQuestionnaire : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI questionText;
        [SerializeField] private TMP_Dropdown  dropDown;
        [SerializeField] private GameObject gameSelector;
        
        private Queue<QuestionOptions> _questionOptionsQueue;
        private QuestionOptions _curQuestionOption;
        private bool _loginScreen = true;

        private void Awake()
        {
            _questionOptionsQueue = new Queue<QuestionOptions>();
        }

        /// <summary>
        /// Enqueue every questionOption from the list of questionOptions.
        /// </summary>
        /// <param name="questionOptions"></param>
        public void SetQuestionOptionsQueue(IEnumerable<QuestionOptions> questionOptions)
        {
            _questionOptionsQueue.Clear();
            foreach (var questionOption in questionOptions)
            {
                _questionOptionsQueue.Enqueue(questionOption);
            }
        }

        /// <summary>
        /// Fills every dropbox with the answer choices from the questionOptions.AnswerChoices.
        /// </summary>
        /// <param name="questionOptions"></param>
        public void SetAnswerChoices(QuestionOptions questionOptions)
        {
            dropDown.ClearOptions();
            dropDown.options.Add(new TMP_Dropdown.OptionData("Select an option"));
            // Fill dropdown with choices
            foreach (var choice in questionOptions.AnswerChoices)
            {
                dropDown.options.Add(new TMP_Dropdown.OptionData() { text = choice });
            }
        }

        /// <summary>
        /// Continue onto the next Question and AnswerChoices until there are no more Questions in the Queue, in which
        /// the GameSelector is displayed.
        /// </summary>
        public void ContinueQuestions()
        {
            if (dropDown.value == 0 && !_loginScreen) return;

            if (GetNextQuestionOption())
            {
                if (_loginScreen)
                {
                    dropDown.GameObject().SetActive(true);
                    _loginScreen = false;
                }
                else
                {
                    var metric = new MetricInformation.QuestionnaireMetrics(questionText.text,   dropDown.options[dropDown.value].text); 
                    MetricCollectionManager.Instance.AppendToQuestionnaireMetricsList(metric);
                }
                SetAnswerChoices(_curQuestionOption);
            }
            else
            {
                ShowGameSelector();
                EndQuestions();
            }
            
        }

        /// <summary>
        /// Turns off the Canvas for the Questionnaire
        /// </summary>
        private void EndQuestions()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Displays the GameSelector menu and changes the GameState
        /// </summary>
        private void ShowGameSelector()
        {
            gameSelector.SetActive(true);
            GameManager.Instance.UpdateGameState(GameManager.GameState.GameSelector);
        }

        /// <summary>
        /// Dequeues the queue so that the next set of QuestionOptions are displayed.
        /// </summary>
        /// <returns></returns>
        private bool GetNextQuestionOption()
        {
            if (_questionOptionsQueue.Count == 0)
            {
                return false;
            }
            
            _curQuestionOption = _questionOptionsQueue.Dequeue();
            DisplayQuestion(_curQuestionOption);
            return _questionOptionsQueue.Count >= 0;
        }
        
        /// <summary>
        /// Displays the question being asked in the Canvas Text.
        /// </summary>
        /// <param name="questionOptions"></param>
        private void DisplayQuestion(QuestionOptions questionOptions)
        {
            questionText.text = questionOptions.Question;
        }
    }
}
