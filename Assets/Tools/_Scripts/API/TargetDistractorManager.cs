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
        /// Enables the MetavizAPIManager to persist across scenes as a singleton.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else Destroy(gameObject);
        }
        
        
        public void AddEvent(Event parentEvent, List<Event> childrenEvent, int curAlienID)
        {
            Event lastChildEvent;
            
            if (childrenEvent.Count != 0)
            {
                lastChildEvent = childrenEvent[childrenEvent.Count - 1];
            }
            else
            {
                lastChildEvent = parentEvent;
            }
            
            string parentID = GetParentID(lastChildEvent, curAlienID);
            parentEvent.ParentId = parentID;
            _eventList.Add(parentEvent);
            foreach (var childEvent in childrenEvent)
            {
                childEvent.ParentId = parentID;
                _eventList.Add(childEvent);
            }
        }
        
        
        private string GetParentID(Event lastEvent, int curObjectID)
        {
            var objectName = lastEvent.ShouldRespond ? "log" : "car";
            
            if (lastEvent.UserInput && lastEvent.ShouldRespond) // if the user responded and it is a target (CORRECT)
            {
                return objectName + curObjectID + "_target_with_correct_user_response";
            }

            if (!lastEvent.UserInput && !lastEvent.ShouldRespond) // if the user did not respond and it is a distractor (CORRECT)
            {
                return objectName + curObjectID + "_distractor_with_no_user_response";
            }

            if (!lastEvent.UserInput && lastEvent.ShouldRespond) // if the user did not respond and it is a target (INCORRECT)
            {
                return objectName + curObjectID + "_target_with_no_user_response";
            }

            if (lastEvent.UserInput &&
                !lastEvent.ShouldRespond) // if the user responded and it is a distractor (INCORRECT)
            {
                return objectName + curObjectID + "_distractor_with_user_incorrect_response";
            }
            // this shouldn't ever happen though
            Debug.LogError("No parent ID given");
            return null;
        }
        
        public void GetVpiScore(Action<String> vpiScoreCallback = null)
        {
            MetavizAPIManager.Instance.BeginPostPsychometrics(new EventArray(_eventList), callback: (result) =>
            {
                MetavizAPIManager.Instance.BeginGetPerformanceModel(callback: (getResult) =>
                {
                    vpiScoreCallback?.Invoke(getResult);
                });
            });
        }
    }
}