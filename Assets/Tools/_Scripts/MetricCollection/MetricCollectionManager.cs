using System.Collections.Generic;
using UnityEngine;

namespace MetricCollection
{
    public class MetricCollectionManager : MonoBehaviour
    {
        public static MetricCollectionManager Instance;
        
        public List<MetricInformation.QuestionnaireMetrics> QuestionnaireMetricsList;

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
        /// At the start of the game, create a new List for both Questionnaire and Dynamic, then GetStaticMetrics
        /// </summary>
        private void Start()
        {
            QuestionnaireMetricsList = new List<MetricInformation.QuestionnaireMetrics>();
        }

        /// <summary>
        /// Takes questionMetrics and adds to the QuestionMetricsList
        /// </summary>
        /// <param name="questionnaireMetrics"></param>
        public void AppendToQuestionnaireMetricsList(MetricInformation.QuestionnaireMetrics questionnaireMetrics)
        {
            QuestionnaireMetricsList.Add(questionnaireMetrics);
        }
    }

}
