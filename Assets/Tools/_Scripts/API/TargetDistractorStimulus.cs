using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Metaviz.HPL.Demo
{
    public class TargetDistractorStimulus : MonoBehaviour
    {
        // The type of stimulus (target or distractor)
        [SerializeField] private StimuliType stimuliType; 
        
        // The ID of the task that the stimulus is associated with
        [SerializeField] private string taskID; 
        
        // The material that is applied to the stimulus when it is in the foreground
        [SerializeField] private Material foregroundMaterial; 
        
        // The material that is applied to the stimulus when it is in the background
        [SerializeField] private Material backgroundMaterial; 
        
        // Created when the user first sees the stimulus
        private Event _parentEvent;
        
        // Created each time the user interacts with the stimulus
        private List<Event> _interactions = new(); 
    
        // Reference to the TargetDistractorManager singleton
        private TargetDistractorManager _targetDistractorManager; 
        
        // Reference to the main camera
        private Camera _mainCamera; 
        
        private void Start()
        {
            _targetDistractorManager = TargetDistractorManager.Instance; // Get the TargetDistractorManager singleton
            _mainCamera = Camera.main; // Get the main camera 
        }

        // observe method: called whenever a user first sees a stimulus
        // if it's the first time the user sees the stimulus, create a parent event
        // otherwise, create a child event and add it to the list of interactions.
        
        /// <summary>
        /// This should be called whenever a stimulus is first seen by the user, and whenever the stimulus changes or is interacted with.
        /// For example, if the stimulus is a target, this method should be called when the user first sees the target, when the target changes, and when the user interacts with the target.
        /// 
        /// This method creates an event and adds it to the list of events in the TargetDistractorManager.
        /// </summary>
        /// <param name="interactedWith">Optional. Set to true if the user has interacted with the stimulus. Default is false.</param>
        /// <param name="time">Optional. The time the event occurred. Default is the current time.</param>
        /// <param name="foreground">Optional. The foreground color of the stimulus. Default is the color of the foregroundMaterial.</param>
        /// <param name="background">Optional. The background color of the stimulus. Default is the color of the backgroundMaterial.</param>
        /// <param name="position">Optional. The position of the stimulus on the screen. Default is the screen position of the object's transform in screen pixel units.</param>
        /// <param name="dimensions">Optional. The dimensions of the stimulus. Default is the local scale of the object's transform in screen pixel units.</param>
        /// 
        public void Observe(bool interactedWith = false, DateTime? time = null, Color foreground = null, Color background = null, Position position = null, Dimensions dimensions = null)
        {
            try
            {
                if (_parentEvent == null)
                {
                    _parentEvent = new Event(
                        eventType: Event.EventTypeEnum.Parent,
                        parentId: GetInstanceID().ToString(),
                        taskId: taskID,
                        userInput: false, // Since the parent event is created when the user first sees the stimulus, they have not interacted with it yet.
                        shouldRespond: stimuliType == StimuliType.Target,
                        timeOccurred: time ?? DateTime.Now,
                        foreground: foreground ?? new Color(foregroundMaterial.color),
                        background: background ?? new Color(backgroundMaterial.color),
                        position: position ?? new Position(GetCameraPosition()),
                        dimensions: dimensions ??
                                    new Dimensions(transform
                                        .localScale) // TODO: This is not the correct way to get the dimensions of the stimulus. It should be the size on the screen, not the size in world units.
                    );
                    print("Observing Parent: " + _parentEvent.ToJson());
                }
                else
                {
                    _interactions.Add(new Event(
                        eventType: Event.EventTypeEnum.Child,
                        parentId: _parentEvent.ParentId,
                        taskId: taskID,
                        userInput: interactedWith,
                        shouldRespond: stimuliType == StimuliType.Target,
                        timeOccurred: time ?? DateTime.Now,
                        foreground: foreground ?? new Color(foregroundMaterial.color),
                        background: background ?? new Color(backgroundMaterial.color),
                        position: position ?? new Position(GetCameraPosition()),
                        dimensions: dimensions ??
                                    new Dimensions(transform
                                        .localScale) // TODO: This is not the correct way to get the dimensions of the stimulus. It should be the size on the screen, not the size in world units.
                    ));
                    print("Observing Child: " + _interactions[_interactions.Count - 1].ToJson());
                }
            }
            catch (InvalidOperationException ex)
            {
                print("Attempted to observe a stimulus that is missing the main camera: " + ex.Message);
            }
        }
        
        /// <summary>
        /// Returns the screen position of the stimulus relative to the main camera.
        /// </summary>
        /// <returns>The screen position of the stimulus relative to the main camera.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the main camera is missing.</exception>
        private Vector3 GetCameraPosition()
        {
            if (_mainCamera == null)
            {
                throw new InvalidOperationException("Main camera is missing!");
            }

            return _mainCamera.WorldToScreenPoint(transform.position);
        }

        // OnDestroy method: called when the stimulus is destroyed. Adds the parent event and all interactions to the TargetDistractorManager's list of events.
        private void OnDestroy()
        {
            if (_parentEvent != null)
            {
                _targetDistractorManager.AddEvent(_parentEvent);
                foreach (var interaction in _interactions)
                {
                    _targetDistractorManager.AddEvent(interaction);
                }
            }
        }

        private enum StimuliType
        {
            Target = 0,
            Distractor = 1
        }
    }
}