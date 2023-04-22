

namespace MetricCollection
{
    public class QuestionnaireMetrics
    {
        public string Question;
        public string Answer;

        /// <summary>
        /// Constructor for creating a Questionnaire Metrics Class 
        /// </summary>
        /// <param name="question"></param>
        /// <param name="answer"></param>
        public QuestionnaireMetrics(string question, string answer)
        {
            Question = question;
            Answer = answer;
        }
    }
}