using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class QuestionnaireHandler : MonoBehaviour
    {
        [SerializeField] private IntroQuestionnaire introQuestionnaire;
        private QuestionOptions[] _questionOptions = {
            new()
            {
                Question = "Hours of game play per day",
                AnswerChoices = new List<string>{"0-2 hours", "2-5 hours", "5-9 hours", "12+ hours"}
            },
            new()
            {
                Question = "Hours of total screen time per day",
                AnswerChoices = new List<string>{"0-2 hours", "2-5 hours", "5-9 hours", "12+ hours"}
            },
            new()
            {
                Question = "Hours of sleep per night",
                AnswerChoices = new List<string>{"0-1 hours", "1-2 hours", "2-3 hours", "3-4 hours", "4-5 hours", "5-6 hours", "6-7 hours", "7-8 hours", "8-9 hours", "9-10 hours", "10-11 hours", "11-12 hours", "12+ hours"}
            },
            new()
            {
                Question = "Physical Activity per week",
                AnswerChoices = new List<string>{"0-1 hours", "1-2 hours", "2-3 hours", "3-4 hours", "4+ hours"}
            },
            new()
            {
                Question = "How much water do you drink per day?",
                AnswerChoices = new List<string>{"1 cup", "2 cups", "3 cups", "4 cups", "5 cups", "6 cups", "7 cups", "8+ cups"}
            },
        };

        /// <summary>
        /// Loads the questions and answers from the JSON 
        /// </summary>
        private void Start()
        {
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
        
    }
}

