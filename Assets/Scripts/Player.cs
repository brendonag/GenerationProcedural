﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Player component. Manages inputs, character states and associated game flow.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

    public static Player Instance = null;

    [System.Serializable]
    public class MovementParameters
    {
        public float speedMax = 2.0f;
        public float acceleration = 12.0f;
        public float friction = 12.0f;
    }

    // Possible orientation for player aiming : 4 direction, 8 direction (for keyboards or D-pads) or free direction (for analogic joysticks)
    public enum ORIENTATION
    {
        FREE,
        DPAD_8,
        DPAD_4
    }

    // Character can only be at one state at a time. For example, he can't attack and be stunned at the same time.
    public enum STATE
    {
        IDLE = 0,
        ATTACKING = 1,
        STUNNED = 2,
        DEAD = 3,
    }

    // Life and hit related attributes
    [Header("Life")]
    public int life = 3;
    public float invincibilityDuration = 1.0f;
    public float invincibilityBlinkPeriod = 0.2f;
    public LayerMask hitLayers;
    public float knockbackSpeed = 3.0f;
    public float knockbackDuration = 0.5f;
    public Color deadColor = Color.gray;

    private float _lastHitTime = float.MinValue;
    private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();
    private Coroutine _blinkCoroutine = null;


    // Movement attributes
    [Header("Movement")]
    public MovementParameters defaultMovement = new MovementParameters();
    public MovementParameters stunnedMovement = new MovementParameters();

    private Rigidbody2D _body = null;
    private Vector2 _direction = Vector2.zero;
    private MovementParameters _currentMovement = null;
    private float _coefInverse = 1.0f;

    private float originSpeedMax = 0;
    private float originFriction = 0;
    private float originAcceleration = 0;

    // Attack attributes
    [Header("Attack")]
    public GameObject attackPrefab = null;
    public GameObject attackSpawnPoint = null;
    public float attackCooldown = 0.3f;
    public ORIENTATION orientation = ORIENTATION.FREE;

    private float lastAttackTime = float.MinValue;

    // Input attributes
    [Header("Input")]
    [Range(0.0f, 1.0f)]
    public float controllerDeadZone = 0.3f;

    // State attributes
    private STATE _state = STATE.IDLE;

    // Collectible attributes
    private int _keyCount;
    public int KeyCount { get { return _keyCount; } set { _keyCount = value; } }

	// Dungeon position
	private Room _room = null;
	public Room Room { get { return _room; } }

    public SpriteRenderer _blackout;


	private void Awake () {
        Instance = this;
        _body = GetComponent<Rigidbody2D>();
        GetComponentsInChildren<SpriteRenderer>(true, _spriteRenderers);
    }

    private void Start()
    {
        SetState(STATE.IDLE);

        originAcceleration = defaultMovement.acceleration;
        originSpeedMax = defaultMovement.speedMax;
        originFriction = defaultMovement.friction;

        _blackout.color = new Vector4(1, 1, 1, 0);
    }

    private void Update () {
        UpdateState();
        UpdateInputs();
        UpdateRoom();
    }

    private void FixedUpdate()
    {
        FixedUpdateMovement();
	}

    /// <summary>
    /// Updates any room related behaviours. By default, move from one room to another when reaching 
    /// </summary>
	private void UpdateRoom()
	{
        
        Bounds roomBounds = _room.GetWorldBounds();
        Room nextRoom = null;

        if(transform.position.x > roomBounds.max.x)
        {
            nextRoom = _room.GetAdjacentRoom(Utils.ORIENTATION.EAST, transform.position);
        } else if(transform.position.x < roomBounds.min.x)
        {
            nextRoom = _room.GetAdjacentRoom(Utils.ORIENTATION.WEST, transform.position);
        } else if (transform.position.y > roomBounds.max.y)
        {
            nextRoom = _room.GetAdjacentRoom(Utils.ORIENTATION.NORTH, transform.position);
        }
        else if (transform.position.y < roomBounds.min.y)
        {
            nextRoom = _room.GetAdjacentRoom(Utils.ORIENTATION.SOUTH, transform.position);
        }

		if(nextRoom != null)
		{
            EnterRoom(nextRoom);
		}
	}

	/// <summary>
    /// Updates inputs
    /// </summary>
	private void UpdateInputs()
    {
        float l_x = Input.GetAxisRaw("Horizontal");
        float l_y = Input.GetAxisRaw("Vertical");
        
        if (CanMove())
        {
            _direction = new Vector2(l_x *= _coefInverse, l_y *= _coefInverse);

            if (_direction.magnitude < controllerDeadZone)
            {
                _direction = Vector2.zero;
            } 
            else 
            {
                _direction.Normalize();
            }

            if(Input.GetButtonDown("Fire1")) 
            {
                Attack();
            }
        }
        else 
        {
            _direction = Vector2.zero;
        }
    }

    /// <summary>
    /// Updates current state
    /// </summary>
    private void UpdateState()
    {
        switch(_state)
        {
			case STATE.ATTACKING:
				SpawnAttackPrefab();
				SetState(STATE.IDLE);
				break;
			default: break;
        }
    }

    /// <summary>
    /// Changes current state to a new given state. Instructions related to exiting and entering a state should be coded in the two "switch(_state){...}" of this method.
    /// </summary>    
    private void SetState(STATE state)
    {
        // Exiting previous state
        // switch (_state)
        //{
        //}

        _state = state;

        // Entering new state
        switch (_state)
        {
            case STATE.STUNNED: _currentMovement = stunnedMovement; break;
            case STATE.DEAD: 
                EndBlink();
                SetColor(deadColor);
                break;
            default: _currentMovement = defaultMovement; break;
        }

        // Reset direction if player cannot move in this state
        if (!CanMove())
        {
            _direction = Vector2.zero;
        }
    }

    //TRAPS EFFECT
    public void StunPlayer(float p_duration)
    {
        StartCoroutine(TimeStunned(p_duration));
    }
    public void ConfusePlayer(float p_duration)
    {
        StartCoroutine(TimeConfused(p_duration));
    }
    public void ChangePlayerSpeed(bool p_isSpeed, float p_duration)
    {
        if (p_isSpeed)
        {
            defaultMovement.speedMax = 10;
            defaultMovement.acceleration = 50;
        }
        else
        {
            defaultMovement.speedMax = 1;
            defaultMovement.acceleration = 5;
        }

        StartCoroutine(TimeSpeedChanged(p_duration));
    }
    public void TpPlayer()
    {
        int rnd = Random.Range(1, Room.allRooms.Count);

        _room = Room.allRooms[rnd];

        Bounds currentBounds = _room.GetWorldBounds();
        Vector3 newPosition = currentBounds.center;

        gameObject.transform.position = newPosition;

        EnterRoom(_room);
    }
    public void ActiveAlarm(List<GameObject> p_list)
    {
        Bounds currentBounds = _room.GetWorldBounds();

        for(int i =0; i < p_list.Count; i++)
        {
            Vector3 newPosition = new Vector3(currentBounds.center.x + i, currentBounds.center.y, currentBounds.center.z);
            p_list[i].transform.position = newPosition;
        }
    }
    public void GoStraightAhead(float p_duration)
    {
        StartCoroutine(TimeStraightAhead(p_duration));
    }
    public void GoBlackout(Sprite sprite, float p_duration)
    {
        _blackout.sprite = sprite;
        StartCoroutine(TimeBlackout(_blackout, p_duration));
    }

    //ENUMERATOR EFFECTS
    private IEnumerator TimeStunned(float p_duration)
    {
        SetState(STATE.STUNNED);

        yield return new WaitForSeconds(p_duration);

        SetState(STATE.IDLE);
    }
    private IEnumerator TimeConfused(float p_duration)
    {
        _coefInverse = -1;

        yield return new WaitForSeconds(p_duration);

        _coefInverse = 1;
    }
    private IEnumerator TimeSpeedChanged(float p_duration)
    {
        yield return new WaitForSeconds(p_duration);

        defaultMovement.friction = originFriction;
        defaultMovement.speedMax = originSpeedMax;
        defaultMovement.acceleration = originAcceleration;
    }
    private IEnumerator TimeStraightAhead(float p_duration)
    {
        _body.AddForce(transform.right * 500);

        SetState(STATE.STUNNED);
        yield return new WaitForSeconds(p_duration);

        SetState(STATE.IDLE);
    }
    IEnumerator TimeBlackout(SpriteRenderer sprite, float p_duration)
    {
        // Fade In
        yield return Fade(sprite, 0f, 1f, 1);

        yield return new WaitForSeconds(p_duration);

        // Fade Out
        yield return Fade(sprite, 1f, 0f, 1);
    }
    IEnumerator Fade(SpriteRenderer sprite, float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = sprite.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (elapsedTime < duration)
        {
            sprite.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        sprite.color = targetColor;
    }

    //FIXED UPDATE
    /// <summary>
    /// Updates velocity and frictions
    /// </summary>
    void FixedUpdateMovement()
    {
        if (_direction.magnitude > Mathf.Epsilon) // magnitude > 0
        {
            // If direction magnitude > 0, Accelerate in direction, then clamp velocity to max speed. Do not apply friction if character is moving toward a direction.
            _body.velocity += _direction * _currentMovement.acceleration * Time.fixedDeltaTime;
            _body.velocity = Vector2.ClampMagnitude(_body.velocity, _currentMovement.speedMax);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, ComputeOrientationAngle(_direction));
        } else {
            // If direction magnitude == 0, Apply friction
            float frictionMagnitude = _currentMovement.friction * Time.fixedDeltaTime;
            if (_body.velocity.magnitude > frictionMagnitude)
            {
                _body.velocity -= _body.velocity.normalized * frictionMagnitude;
            } else {
                _body.velocity = Vector2.zero;
            }
        }
    }

    /// <summary>
	/// Sets player in attack state. Attack prefab will be spawned when calling SpawnAttackPrefab method a little later.
    /// </summary>
	private void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;
        lastAttackTime = Time.time;
        SetState(STATE.ATTACKING);
        
    }

    /// <summary>
    /// Spawns the associated "attack" prefab on attackSpawnPoint.
    /// </summary>
    private void SpawnAttackPrefab()
    {
        if (attackPrefab == null)
            return;

        // transform used for spawn is attackSpawnPoint.transform if attackSpawnPoint is not null. Else it's transform.
        Transform spawnTransform = attackSpawnPoint ? attackSpawnPoint.transform : transform;
        GameObject.Instantiate(attackPrefab, spawnTransform.position, spawnTransform.rotation);
    }

    /// <summary>
    /// Called when player takes a hit (ie from enemy hitbox or spikes).
    /// </summary>
    public void ApplyHit(Attack attack, int p_power)
    {
        if (Time.time - _lastHitTime < invincibilityDuration)
            return;
        _lastHitTime = Time.time;

        life -= (attack != null ? attack.damages : p_power);

        if (life <= 0)
        {
            SetState(STATE.DEAD);
        } else {
            if (attack != null && attack.knockbackDuration > 0.0f)
            {
                StartCoroutine(ApplyKnockBackCoroutine(attack.knockbackDuration, attack.transform.right * attack.knockbackSpeed));
            }
            EndBlink();
            _blinkCoroutine = StartCoroutine(BlinkCoroutine());
            if (attack != null && attack.isSlowTrap)
            {
                StartCoroutine(SlowDownPlayer(attack.slowDuration, attack.slowFactor));
            }
        }
    }

    private IEnumerator SlowDownPlayer(float duration, float factor)
    {
        float originalSpeedMax = defaultMovement.speedMax; 
        defaultMovement.speedMax *= factor;

        yield return new WaitForSeconds(duration);

      
        defaultMovement.speedMax = originalSpeedMax;
    }
    public void ApplySpeedModifier(float factor)
    {
        defaultMovement.speedMax *= factor;
    }
    public void RemoveSpeedModifier(float factor)
    {
        defaultMovement.speedMax /= factor; 
    }

    /// <summary>
    /// Puts player in STUNNED state and sets a velocity to knockback player. It resume to IDLE state after a fixed duration. STUNNED state has his own movement parameters that allow to redefine frictions when character is knocked.
    /// </summary>
    private IEnumerator ApplyKnockBackCoroutine(float duration, Vector3 velocity)
    {
        SetState(STATE.STUNNED);
        _body.velocity = velocity;
        yield return new WaitForSeconds(duration);
        SetState(STATE.IDLE);
    }

    /// <summary>
    /// Makes all sprite renderers in the player hierarchy blink from enabled to disabled with a fixed period over a fixed time.  
    /// </summary>
    private IEnumerator BlinkCoroutine()
    {
        float invincibilityTimer = 0;
        while(invincibilityTimer < invincibilityDuration)
        {
            invincibilityTimer += Time.deltaTime;
            bool isVisible = ((int)(invincibilityTimer / invincibilityBlinkPeriod)) % 2 == 1;
            foreach(SpriteRenderer spriteRenderer in _spriteRenderers)
            {
                spriteRenderer.enabled = isVisible;
            }
            yield return null; // wait next frame
        }
        EndBlink();
    }

    /// <summary>
    /// Stops current blink coroutine if any is started and set all sprite renderers to enabled.
    /// </summary>
    private void EndBlink()
    {
        if (_blinkCoroutine == null)
            return;
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.enabled = true;
        }
        StopCoroutine(_blinkCoroutine);
        _blinkCoroutine = null;

    }

    /// <summary>
    /// Sets the tint color of all SpriteRenderers to a given tint color.
    /// </summary>
    private void SetColor(Color color)
    {
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.color = color;
        }
    }

    /// <summary>
    /// Transforms the orientation vector into a discrete angle.
    /// </summary>
    private float ComputeOrientationAngle(Vector2 direction)
    {
        float angle = Vector2.SignedAngle(Vector2.right, direction);
        switch(orientation)
        {
            case ORIENTATION.DPAD_8: return Utils.DiscreteAngle(angle, 45); // Only 0 45 90 135 180 225 270 315
            case ORIENTATION.DPAD_4: return Utils.DiscreteAngle(angle, 90); // Only 0 90 180 270
            default: return angle;
        }
    }

    /// <summary>
    /// Returns whether or not player can moves and attack
    /// </summary>
    private bool CanMove()
    {
        return _state == STATE.IDLE;
    }

    /// <summary>
    /// Called to enter a room
    /// </summary>
    /// 
	public void EnterRoom(Room room)
	{
        Room previous = _room;
        _room = room;
        room.OnEnterRoom(previous);
    }

    /// <summary>
    /// Checks if player gets hit by any attack hitbox. Applies attack data (damages, knockback, ...) to player.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( ((1 << collision.gameObject.layer) & hitLayers) != 0 )
        {
            // Collided with hitbox
            Attack attack = collision.gameObject.GetComponent<Attack>();
            ApplyHit(attack, 1);
        }
    }
}
