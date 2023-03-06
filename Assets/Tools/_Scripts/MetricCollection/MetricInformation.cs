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
        public class DynamicMetrics
        {
            public float Time;
            public bool IsTarget;
            public bool IsInteracted;
            // public EnemyAttributes EnemyAttributes;
            public Vector2 EnemyPosition;
            public float EnemyMoveSpeed;

            /// <summary>
            /// Constructor for creating a Dynamic Metrics Class 
            /// </summary>
            /// <param name="time"></param>
            /// <param name="isTarget"></param>
            /// <param name="isInteracted"></param>
            /// <param name="enemyAttributes"></param>
            /// <param name="enemyPosition"></param>
            /// <param name="enemyMoveSpeed"></param>
            public DynamicMetrics(float time, bool isTarget, bool isInteracted, Vector2 enemyPosition,
                float enemyMoveSpeed)
            {
                Time = time;
                IsTarget = isTarget;
                IsInteracted = isInteracted;
                EnemyPosition = enemyPosition;
                EnemyMoveSpeed = enemyMoveSpeed;
            }
        }
        
        public class StaticMetrics
        {
            public Resolution ScreenResolution;
            public float ScreenDPI;

            /// <summary>
            /// Constructor for creating a Static Metrics Class 
            /// </summary>
            /// <param name="screenResolution"></param>
            /// <param name="screenDPI"></param>
            public StaticMetrics(Resolution screenResolution, float screenDPI)
            {
                ScreenResolution = screenResolution;
                ScreenDPI = screenDPI;
            }
        }

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
