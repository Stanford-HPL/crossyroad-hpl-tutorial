using UnityEngine;
using System.Collections;
using METAVIZ;
using Event = UnityEngine.Event;

public class CarScript : MonoBehaviour {

    // =============================
    // TODO Genericize Car and Trunk
    // =============================

    /// <summary>
    /// The X-speed of car, in units per second.
    /// </summary>
    public float speedX = 1.0f;

    private Rigidbody playerBody;
    
    
    // VPI
    private Transform _player;
    private bool _playerAlive = true;
    private bool _playerWasOneAwayFromObject = false;
    private bool _interactedOnce = false;
    private bool _updatedAlready = false;
    private TargetDistractorStimulus _targetDistractorStimulus;

    private void Start()
    {
        _player = _player = GameObject.FindGameObjectWithTag("Player").transform;
        _targetDistractorStimulus = GetComponent<TargetDistractorStimulus>();
    }

    public void Update()
    {
        transform.position += new Vector3(speedX * Time.deltaTime, 0.0f, 0.0f);
        
        // VPI Implementation
        // Runs update loop only once per object if player is near log or car.
        if (_updatedAlready) return;
        if (!ObjectMinOneAhead()) return;
        if (_playerWasOneAwayFromObject) return;
        _playerWasOneAwayFromObject = true;
        
        
        _targetDistractorStimulus.Observe(interactedWith: false);
    }

    void OnTriggerEnter(Collider other)
    {
        // When collide with player, flatten it!
        if (other.gameObject.tag == "Player")
        {
            Vector3 scale = other.gameObject.transform.localScale;
            other.gameObject.transform.localScale = new Vector3(scale.x, scale.y * 0.1f, scale.z);
            other.gameObject.SendMessage("GameOver");
            _playerAlive = false;
            _targetDistractorStimulus.Observe(interactedWith: true);
        }
    }
    
    
    // ALL VPI
    
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
    
         /// <summary>
         /// Sends a POST request to a Psychometric API when the player is only one jump away from the stimuli and it
         /// gets destroyed from being out of bounds. This will be a "Child" eventType.
         /// </summary>
         /// <returns>Void</returns>
         private void OnDestroy()
         {
             if (!_interactedOnce && _playerWasOneAwayFromObject)
             {
                 _interactedOnce = true;
                 _targetDistractorStimulus.Observe(interactedWith: false);
             }
         }

         /// <summary>
         /// Sends a POST request to a Psychometric API when the playerdies. This will be a "Child" eventType.
         /// </summary>
         /// <returns>Void</returns>
         private void OnPlayerDeath()
         {
             if (!_interactedOnce && _playerWasOneAwayFromObject)
             {
                 _interactedOnce = true;
                 _targetDistractorStimulus.Observe(interactedWith: false);
             }
         }
}
