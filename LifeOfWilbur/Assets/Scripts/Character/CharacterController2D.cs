using UnityEngine;
using UnityEngine.Events;

// Adapted from https://github.com/Brackeys/2D-Character-Controller
/// <summary>
/// Controller for Wilburs movement.
/// There are many fields which allow the movement to be fine tuned and customised.
/// </summary>
public class CharacterController2D : MonoBehaviour
{
	/// <summary>
    /// Amount of force added when the player jumps.
    /// </summary>
	[SerializeField] private float _jumpForce = 400f;
	/// <summary>
    /// How much to smooth out the movement
    /// </summary>							
	[Range(0, .3f)] [SerializeField] private float _movementSmoothing = 0.05f;
    /// <summary>
    /// Whether or not a player can steer while jumping;
    /// </summary>	
    [SerializeField] private bool _airControl = false;
#pragma warning disable 649 // disable 'field not set' warning as these values are set by Unity.
    /// <summary>
    /// A mask determining what is ground to the character
    /// </summary>
    [SerializeField] private LayerMask _whatIsGround;
    /// <summary>
    /// A position marking where to check if the player is grounded.
    /// </summary>
    [SerializeField] private Transform _groundCheck;
#pragma warning restore 649
    /// <summary>
    /// A position marking where to check for ceilings
    /// </summary>								    
    [SerializeField] private Transform _ceilingCheck;

    /// <summary>
    /// Radius of the overlap circle to determine if grounded
    /// </summary>	
	const float GroundedRadius = 0.05f; 
	/// <summary>
    /// Whether or not the player is grounded.
    /// </summary>	
	private bool _grounded = false;
	/// <summary>
    /// Radius of the overlap circle to determine if the player can stand up
    /// </summary>
	const float CeilingRadius = 0.2f;
	private Rigidbody2D _rigidBody2D;
	public Animator _animator;
	/// <summary>
    /// For determining which way the player is currently facing.
    /// </summary>
	private bool _facingRight = true;  
	private Vector3 _velocity = Vector3.zero;
    public float _runSpeed = 25f;
	private float _horizontalMove = 0f;
    private float _jumpRemember = 0f;
    private float _groundedRemember = 0f;
    const float JumpBuffer = 0.1f;
    const float GroundedBuffer = 0.1f;
	public static bool MovementDisabled { get; set; } = false;

	private void Awake()
	{
		_rigidBody2D = GetComponent<Rigidbody2D>();
	}

	void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed;

		_animator.SetFloat("horizontalSpeed", Mathf.Abs(_horizontalMove)); // Inform animator whether or not wilbur is moving
		_animator.SetFloat("verticalSpeed", _rigidBody2D.velocity.y); // Inform animator of wilburs y-velocity
		_animator.SetBool("grounded",_grounded); // Informing animator whether or not wilbur is on the ground
        
        if (Input.GetButtonDown("Jump"))
        {
            _jumpRemember = JumpBuffer;
        }

        //Debug.Log("Velocity is: " + _rigidBody2D.velocity.x);
        if(_rigidBody2D.velocity.x > 2 || _rigidBody2D.velocity.x < -2)
        {
            //Play the 'SnowWalk' audio whenever Wilbur jumps
            FindObjectOfType<AudioManager>().PlayWalking("SnowWalkTrimmed");
        }
	
    }

	private void FixedUpdate()
	{
		_jumpRemember = _jumpRemember - Time.deltaTime;
		_groundedRemember = _groundedRemember - Time.deltaTime;

		bool wasGrounded = _grounded;
		_grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, GroundedRadius, _whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
                _groundedRemember = GroundedBuffer;
                _grounded = true;
			}
		}

		if (!MovementDisabled)
		{
			Move(_horizontalMove * Time.fixedDeltaTime);
		}
	}


	public void Move(float move)
	{
        
		// Only control the player if grounded or airControl is turned on
		if (_grounded || _airControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, _rigidBody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			_rigidBody2D.velocity = Vector3.SmoothDamp(_rigidBody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !_facingRight)
			{
				// Flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && _facingRight)
			{
				// Flip the player.
				Flip();
			}
		}

		// If the player should jump...
		if (_grounded && _jumpRemember > 0)
		{
			_jumpRemember = 0f;
            _groundedRemember = 0f;
			// Add a vertical force to the player.
			_grounded = false;
			_rigidBody2D.AddForce(new Vector2(0f, _jumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		_facingRight = !_facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
