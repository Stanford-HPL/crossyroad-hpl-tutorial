using UnityEngine;
using System.Collections;
using METAVIZ;

public class TrunkFloatingScript : MonoBehaviour {

    // ==================================================================
    // TODO Make generic Sinerp() -- or use iTween or Animator instead :v
    // ==================================================================

    /// <summary>
    /// The X-speed of floating trunk, in units per second.
    /// </summary>
    public float speedX = 0.0f;

    /// <summary>
    /// Time for sinking animation, in seconds.
    /// </summary>
    public float animationTime = 0.1f;

    /// <summary>
    /// Distance of the trunk sinking, in units.
    /// </summary>
    public float animationDistance = 0.1f;

    /// <summary>
    /// The water splash prefab to be instantiated.
    /// </summary>
    public GameObject splashPrefab;

    private float originalY;
    private bool sinking;
    private float elapsedTime;
    private Rigidbody playerBody;
    
    // VPI
    private Transform _player;
    private bool _playerAlive = true;
    private bool _playerWasOneAwayFromObject = false;
    private bool _interactedOnce = false;
    private bool _updatedAlready = false;
    private TargetDistractorStimulus _targetDistractorStimulus;

    public void Start()
    {
        originalY = transform.position.y;
        
        // VPI
        _player = _player = GameObject.FindGameObjectWithTag("Player").transform;
        _targetDistractorStimulus = GetComponent<TargetDistractorStimulus>();
    }

    public void Update()
    {
        transform.position += new Vector3(speedX * Time.deltaTime, 0.0f, 0.0f);

        elapsedTime += Time.deltaTime;
        if (elapsedTime > animationTime)
        {
            sinking = false;
            transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
        }

        if (sinking)
        {
            float y = Sinerp(originalY, originalY - animationDistance, (elapsedTime < animationTime) ? (elapsedTime / animationTime) : 1);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }
        
        // VPI Implementation
        // Runs update loop only once per object if player is near log or car.
        if (_updatedAlready) return;
        if (!ObjectMinOneAhead()) return;
        if (_playerWasOneAwayFromObject) return;
        _playerWasOneAwayFromObject = true;
        
        
        _targetDistractorStimulus.Observe(interactedWith: false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _targetDistractorStimulus.Observe(interactedWith: true);
            playerBody = collision.gameObject.GetComponent<Rigidbody>();

            if (!sinking)
            {
                var o = (GameObject)Instantiate(splashPrefab, transform.position, Quaternion.Euler(-90, 0, 0));
                o.transform.localScale = transform.localScale;

                sinking = true;
                elapsedTime = 0.0f;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerBody.position += new Vector3(speedX * Time.deltaTime, 0.0f, 0.0f);
        }
    }

    private float Sinerp(float min, float max, float weight)
    {
        return min + (max - min) * Mathf.Sin(weight * Mathf.PI);
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
