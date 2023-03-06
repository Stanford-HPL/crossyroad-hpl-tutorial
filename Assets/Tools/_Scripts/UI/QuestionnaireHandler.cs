using _Scripts.DataPersistence;
using UnityEngine;

namespace UI
{
    public class QuestionnaireHandler : MonoBehaviour
    {
        [SerializeField] private IntroQuestionnaire introQuestionnaire;
        private QuestionOptions[] _questionOptions;

        /// <summary>
        /// Loads the questions and answers from the JSON 
        /// </summary>
        private void Start()
        {
            LoadData(DataPersistenceManager.GetPreferences());
            HandleQuestionnaire(_questionOptions);
        }

        /// <summary>
        /// Fills in the Questionnaire QuestionOptionsQueue with all the questionOptions asked.
        /// </summary>
        /// <param name="questionOptions"></param>
        private void HandleQuestionnaire(QuestionOptions[] questionOptions)
        {
            introQuestionnaire.SetQuestionOptionsQueue(questionOptions);
        }
        
        public void LoadData(GamePreferences gamePreferences)
        {
            _questionOptions = gamePreferences.questionaire;
        }
    }
}

