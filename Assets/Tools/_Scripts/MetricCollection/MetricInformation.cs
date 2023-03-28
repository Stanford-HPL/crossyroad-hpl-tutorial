using UnityEngine;

namespace MetricCollection
{
    /// <summary>
    /// Metric Information is a class containing specific metric classes such as dynamic, static, and questionnaire.
    /// If you need another field variable, add it to the fields, then add it to the constructor, then go to
    /// where the class constructor is called in order to change the constructor there to include the new field added.
    /// </summary>
    public class MetricInformation
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

}
