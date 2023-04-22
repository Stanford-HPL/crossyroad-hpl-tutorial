using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Metaviz.HPL.Demo
{    public class StimuliInformation : MonoBehaviour
    {
        private Transform _player;
        private BoxCollider _playerBoxCollider;
        private static int _targetId = 0;
        private static int _distractorId = 0;
        private int _curId;
        private bool _interactedOnce = false;
        private bool _updatedAlready = false;
        private bool _playerWasOneAwayFromObject = false;
        private bool _playerAlive = true;
        
        private GameObject _targetBackgroundObject;
        private GameObject _distractorBackgroundObject;

        private Event _parentEvent;
        private List<Event> _childrenEvent;
        
        private string taskID;
        private bool userInput = false;
        private bool shouldRespond;
        private Color foregroundColor;
        private Color backgroundColor;
        private Position position;
        private Dimensions dimensions;
        

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            taskID = "Crossy Road";
            _playerBoxCollider = _player.GetComponent<BoxCollider>();
            _childrenEvent = new List<Event>();
            
            _targetBackgroundObject = GameObject.Find("BackgroundReference/WaterLine/Water");
            _distractorBackgroundObject = GameObject.Find("BackgroundReference/RoadLine/Road");
        }

        ///<summary>
        /// Update loop for checking if the player is alive or dead, and running the loop only once per object if player is near log or car.
        /// If near log or car, set the _curID, and make a parent event. This makes the log or car a stimulus now.
        /// </summary>
        private void Update()
        {
            // Keeps checking to see if player is alive or dead
            if (_playerBoxCollider.enabled && !_playerAlive)
            {
                _playerAlive = true;
            }
            else if (!_playerBoxCollider.enabled && _playerAlive)
            {
                _playerAlive = false;
                OnPlayerDeath();
            }
            
            // Runs update loop only once per object if player is near log or car.
            if (_updatedAlready) return;
            if (!ObjectMinOneAhead()) return;
            
            _playerWasOneAwayFromObject = true;
            if (gameObject.CompareTag("Target"))
            {
                _curId = _targetId;
                _targetId += 1;
            }
            else if (gameObject.CompareTag("Distractor"))
            {
                _curId = _distractorId;
                _distractorId += 1;
            }
            else
            {
                Debug.LogError("Error: TargetInformation.cs: Object is not tagged as Target or Distractor.");
                return;
            }
            
            if (CollectAllMetrics()) 
            {
                _updatedAlready = true;
                _parentEvent = MakeEventWithoutParentID(Event.EventTypeEnum.Parent);
            }
        }

        /// <summary>
        /// Sends a POST request to a Psychometric API when the player collides with a log.
        /// </summary>
        /// <param name="collision">The collision object that triggered the function</param>
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (!_interactedOnce && CollectAllMetrics())
                {
                    SetUserInput(true);
                    _childrenEvent.Add(MakeEventWithoutParentID(Event.EventTypeEnum.Child));
                    TargetDistractorManager.Instance.AddEvent(_parentEvent, _childrenEvent, _curId);
                }
            }
        }
        
        /// <summary>
        /// Sends a POST request to a Psychometric API when the player crashes with the car. This will be a "Parent" eventType.
        /// </summary>
        /// <param name="other">The collider of the object that collided with the trigger.</param>
        /// <returns>Void</returns>
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!_interactedOnce && CollectAllMetrics())
                {
                    SetUserInput(true);
                    _childrenEvent.Add(MakeEventWithoutParentID(Event.EventTypeEnum.Child));
                    TargetDistractorManager.Instance.AddEvent(_parentEvent, _childrenEvent, _curId);
                }
            }
        }

        /// <summary>
        /// Sends a POST request to a Psychometric API when the player is only one jump away from the stimuli and it
        /// gets destroyed from being out of bounds. This will be a "Child" eventType.
        /// </summary>
        /// <returns>Void</returns>
        private void OnDestroy()
        {
            if (!_interactedOnce && _playerWasOneAwayFromObject && CollectAllMetrics())
            {
                _interactedOnce = true;
                _childrenEvent.Add(MakeEventWithoutParentID(Event.EventTypeEnum.Child));
                TargetDistractorManager.Instance.AddEvent(_parentEvent, _childrenEvent, _curId);
            }
        }

        /// <summary>
        /// Sends a POST request to a Psychometric API when the playerdies. This will be a "Child" eventType.
        /// </summary>
        /// <returns>Void</returns>
        private void OnPlayerDeath()
        {
            if (!_interactedOnce && _playerWasOneAwayFromObject && CollectAllMetrics())
            {
                _interactedOnce = true;
                _childrenEvent.Add(MakeEventWithoutParentID(Event.EventTypeEnum.Child));
                TargetDistractorManager.Instance.AddEvent(_parentEvent, _childrenEvent, _curId);
            }
        }

        ///<summary>
        /// This is determined by calculating the distance between the z-coordinate of the current stimuli and the player's position,
        /// and returning true if the distance is between 0 and 1.
        ///</summary>
        ///<returns>True if the stimuli is ahead of the current object, false otherwise.</returns>
        private bool ObjectMinOneAhead()
        {
            if (!_playerAlive) return false;
            var distance = (int)transform.position.z - (int)_player.position.z;
            return 0 <= distance && distance <= 1;
        }

        ///<summary>
        /// This function collects all the metrics related to the foreground and background objects in the scene.
        /// It calls CompareTargetOrDistractor function to check if both foreground and background objects exist,
        /// and then sets the user input, foreground and background colors, position and dimensions of the objects.
        /// If all metrics are collected successfully, it returns true; otherwise, it returns false.
        /// Purpose of this function is to gather every metric needed to create an API Psychometric event.
        ///</summary>
        ///<returns>
        /// Returns a boolean value indicating whether all metrics were successfully collected.
        ///</returns>
        private bool CollectAllMetrics()
        {
            if (!CompareTargetOrDistractor(out GameObject foregroundObject, out GameObject backgroundObject)) return false;
            SetForegroundColor(foregroundObject);
            SetBackgroundColor(backgroundObject);
            SetPosition();
            SetDimensions();
            
            return true;
        }
        
        ///<summary>
        /// This function creates a new Psychometric event using the values of various parameters such as event type,
        /// task ID, user input, response requirement, time of occurrence, foreground and background colors, object position and dimensions.
        /// This does NOT collect parent_id, so you will need to make a new function to put same parent event for all children.
        /// It then sends this event to the MetavizAPIManager to POST at the Metaviz API Psychometric endpoint.
        ///</summary>
        ///<returns>
        /// Void
        ///</returns>
        private Event MakeEventWithoutParentID(Event.EventTypeEnum eventType)
        {
            Event newEvent = new Event(eventType: eventType,
                taskId: taskID,
                userInput: userInput,
                shouldRespond: shouldRespond,
                timeOccurred: DateTime.Now,
                foreground: foregroundColor,
                background: backgroundColor,
                position: position,
                dimensions: dimensions);

            return newEvent;
        }

        #region PsychometricCollection
        
        ///<summary>
        /// This function checks whether the current game object is tagged as a "Target" or "Distractor" object.
        /// Target = Log, Distractor = Car.
        /// If it is a "Target" object, it sets various parameters related to logging and task identification, and
        /// sets the foreground and background objects to specific game objects. If it is a "Distractor" object, it sets
        /// different parameters related to logging and task identification, and sets the foreground and background objects
        /// to different game objects. If it is neither a "Target" nor a "Distractor" object, it sets both foreground and background
        /// objects to null and returns false.
        ///</summary>
        ///<param name="foregroundObject">The game object representing the foreground object in the scene.</param>
        ///<param name="backgroundObject">The game object representing the background object in the scene.</param>
        ///<returns>
        /// Returns a boolean value indicating whether the current game object is tagged as a "Target" or "Distractor" object.
        ///</returns>
        private bool CompareTargetOrDistractor(out GameObject foregroundObject, out GameObject backgroundObject)
        {
            if (gameObject.CompareTag("Target"))  // Target = Log
            {
                shouldRespond = true;
                foregroundObject = transform.GetChild(0).gameObject;  // obtain correct foreground object (roof)
                backgroundObject = _targetBackgroundObject;
            }
            else if (gameObject.CompareTag("Distractor"))  // Distractor = Car
            {
                shouldRespond = false;
                foregroundObject = transform.GetChild(5).gameObject;  // obtain correct foreground object (cylinder)
                backgroundObject = _distractorBackgroundObject;
            }
            else
            {
                foregroundObject = null;
                backgroundObject = null;
                return false;
            }

            return true;
        }

        ///<summary>
        /// This function sets the user input to a specified boolean value and also sets the interactedOnce flag to true.
        /// The user input flag is used to indicate whether the player has interacted with the current object, while the
        /// interactedOnce flag is used to ensure that metrics are only collected once per interaction.
        ///</summary>
        ///<param name="userInput">The boolean value indicating whether the player has interacted with the current object.</param>
        ///<returns>
        /// Void
        ///</returns>
        private void SetUserInput(bool userInput)
        {
            this.userInput = userInput;
            _interactedOnce = true;
        }

        ///<summary>
        /// This function sets the foreground color of the current object to a specified game object's color. It creates a
        /// new Color object using RGB values obtained from the specified game object, and sets alpha value to the alpha
        /// value of the game object's color. The function also creates a new ImageTransformations object using the alpha
        /// value of the game object's color, and sets the foregroundColor variable to the new Color object.
        ///</summary>
        ///<param name="foregroundObject">The game object whose color is used as the foreground color.</param>
        ///<returns>
        /// Void
        ///</returns>
        private void SetForegroundColor(GameObject foregroundObject)
        {
            var foregroundImageTransformations = new ImageTransformations(alpha: foregroundObject.GetComponent<MeshRenderer>().material.color.a);
            foregroundColor = new Color(encoding: "RGBAFloat", 
                r: foregroundObject.GetComponent<MeshRenderer>().material.color.r,
                g: foregroundObject.GetComponent<MeshRenderer>().material.color.g,
                b: foregroundObject.GetComponent<MeshRenderer>().material.color.b,
                transforms: foregroundImageTransformations);
        }
        
        ///<summary>
        /// This function sets the background color of the current object to a specified game object's color. It creates a
        /// new Color object using RGB values obtained from the specified game object, and sets alpha value to the alpha
        /// value of the game object's color. The function also creates a new ImageTransformations object using the alpha
        /// value of the game object's color, and sets the backgroundColor variable to the new Color object.
        ///</summary>
        ///<param name="backgroundObject">The game object whose color is used as the background color.</param>
        ///<returns>
        /// Void
        ///</returns>
        private void SetBackgroundColor(GameObject backgroundObject)
        {
            var backgroundImageTransformations = new ImageTransformations(alpha: backgroundObject.GetComponent<MeshRenderer>().material.color.a);
            backgroundColor = new Color(encoding: "RGBAFloat",
                r: backgroundObject.GetComponent<MeshRenderer>().material.color.r,
                g: backgroundObject.GetComponent<MeshRenderer>().material.color.g,
                b: backgroundObject.GetComponent<MeshRenderer>().material.color.b,
                transforms: backgroundImageTransformations);
        }
        
        ///<summary>
        /// This function sets the position of the current object to a new Position object. The Position object has three
        /// fields: UnitTypeEnum, x, y, and z. The function obtains the position of the current object, and sets the x, y,
        /// and z fields of the Position object to the corresponding values of the current object's position. The unit type
        /// of the position is set to meters.
        ///</summary>
        ///<returns>
        /// Void
        ///</returns>
        private void SetPosition()
        {
            var worldToScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
            position = new Position(unitType: UnitTypeEnum.Pixels, x: worldToScreenPoint.x, y: worldToScreenPoint.y, z: worldToScreenPoint.z);
        }
        
        /// <summary>
        /// This function sets the dimensions of the current object to a new Dimensions object. The Dimensions object has
        /// three fields: UnitTypeEnum, width, height, and depth. The function obtains the local scale of the current object,
        /// and sets the width, height, and depth fields of the Dimensions object to the corresponding values of the current
        /// object's local scale. The unit type of the dimensions is set to meters.
        ///</summary>
        ///<returns>
        /// Void
        ///</returns>
        private void SetDimensions()
        {
            var transformScale = transform.localScale;
            dimensions = new Dimensions(unitType: UnitTypeEnum.Pixels, width: transformScale.x,
                height: transformScale.y, depth: transformScale.z);   
        }
        
        #endregion
    }
}