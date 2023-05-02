using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Metaviz.HPL.Demo
{
    public class StimuliInformation : MonoBehaviour
    {
        private Transform _player;
        private TargetDistractorStimulus _targetDistractorStimulus;
        private bool _firstObservation;
        private bool _firstInteraction;
        private bool _playerAlive;

        private void Start()
        {
            _targetDistractorStimulus = GetComponent<TargetDistractorStimulus>();
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _playerAlive = true;
        }

        ///<summary>
        /// Update loop for checking if the player is alive or dead, and running the loop only once per object if player is near log or car.
        /// If near log or car, set the _curID, and make a parent event. This makes the log or car a stimulus now.
        /// </summary>
        private void Update()
        {
            if (!_firstObservation && _playerAlive &&
                ObjectMinOneAhead()) // If the player is alive and the object is one ahead of the player and the object has not been observed yet
            {
                _firstObservation = true;
                _targetDistractorStimulus.Observe(interactedWith: false);
            }
        }

        /// <summary>
        /// Called when the player first collides with a log. Logs a child event and sets interactedWith to true.
        /// </summary>
        /// <param name="collision">The collision object that triggered the function</param>
        public void OnCollisionEnter(Collision collision)
        {
            if (!_firstInteraction && _playerAlive &&
                collision.gameObject
                    .CompareTag(
                        "Player")) // If the player has not interacted with the log yet and the player is riding on the log
            {
                _firstInteraction = true;
                _targetDistractorStimulus.Observe(interactedWith: true);
            }
        }

        /// <summary>
        /// Called when the player first collides with a car. Logs a child event and sets interactedWith to true.
        /// </summary>
        /// <param name="other">The collider of the object that collided with the trigger.</param>
        /// <returns>Void</returns>
        private void OnTriggerEnter(Collider other)
        {
            if (!_firstInteraction && _playerAlive &&
                other.gameObject
                    .CompareTag(
                        "Player")) // If the player has not interacted with the car yet and the car hits the player
            {
                _firstInteraction = true;
                _playerAlive = false;
                _targetDistractorStimulus.Observe(interactedWith: true);
            }
        }

        /// <summary>
        /// Called when the object is destroyed. If the player has not interacted with the object yet and the object goes off screen, log the object observe.
        /// </summary>
        private void OnDestroy()
        {
            // If the player has not interacted with the object yet and the object goes off screen, log the object observe.
            if (!_firstInteraction && _playerAlive)
            {
                _targetDistractorStimulus.Observe(interactedWith: false);
            }
        }

        ///<summary>
        /// This is determined by calculating the distance between the z-coordinate of the current stimuli and the player's position,
        /// and returning true if the distance is between 0 and 1.
        ///</summary>
        ///<returns>True if the stimuli is ahead of the current object, false otherwise.</returns>
        private bool ObjectMinOneAhead()
        {
            var distance = (int)transform.position.z - (int)_player.position.z;
            return 0 <= distance && distance <= 1;
        }
    }
}