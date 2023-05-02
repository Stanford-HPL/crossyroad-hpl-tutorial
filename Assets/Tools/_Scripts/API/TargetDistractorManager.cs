using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Metaviz.HPL.Demo
{
    public class TargetDistractorManager : MonoBehaviour
    {
        public static TargetDistractorManager Instance;
        private List<Event> _eventList = new();
        
        /// <summary>
        /// Enables the TargetDistractorManager to persist across scenes as a singleton.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);
        }
        
        public void AddEvent(Event newEvent)
        {
            _eventList.Add(newEvent);
        }

        public void GetVpiScore(Action<String> vpiScoreCallback = null)
        {
            StartCoroutine(MetavizAPIManager.Instance.PostPsychometrics(new EventArray(_eventList),
                callback: (result) =>
                {
                    StartCoroutine(MetavizAPIManager.Instance.GetPerformanceModel(callback: (getResult) =>
                    {
                        vpiScoreCallback?.Invoke(getResult);
                    }));
                }));
            
        }
    }
}