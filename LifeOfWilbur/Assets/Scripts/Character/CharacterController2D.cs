using UnityEngine;
using UnityEngine.Events;
// Adapted from https://github.com/Brackeys/2D-Character-Controller
public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float jumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float movementSmoothing = 0.05f;	// How much to smooth out the movement
	[SerializeField] private bool airControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask whatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform groundCheck;							    // A position marking where to check if the player is grounded.
	[SerializeField] private Transform ceilingCheck;							// A position marking where to check for ceilings
	
	const float groundedRadius = 0.2f; // Radius of the overlap circle to determine if grounded
	private bool grounded = false;            // Whether or not the player is grounded.
	const float ceilingRadius = 0.2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D rigidBody2D;
	public Animator animator;
	private bool facingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent onLandEvent;

    public float runSpeed = 25f;
	private float horizontalMove = 0f;
    private float jumpRemember = 0f;
    private float groundedRemember = 0f;
    const float jumpBuffer = 0.1f;
    const float groundedBuffer = 0.1f;

	private void Awake()
	{
		rigidBody2D = GetComponent<Rigidbody2D>();

		if (onLandEvent == null)
			onLandEvent = new UnityEvent();
	}

	void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("horizontalSpeed", Mathf.Abs(horizontalMove)); // Inform animator whether or not wilbur is moving
		animator.SetFloat("verticalSpeed", rigidBody2D.velocity.y); // Inform animator of wilburs y-velocity
		animator.SetBool("grounded",grounded); // Informing animator whether or not wilbur is on the ground
        
        if (Input.GetButtonDown("Jump"))
        {
            jumpRemember = jumpBuffer;
        }
	
    }

	private void FixedUpdate()
	{
		jumpRemember = jumpRemember - Time.deltaTime;
		groundedRemember = groundedRemember - Time.deltaTime;

		bool wasGrounded = grounded;
		grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
                groundedRemember = groundedBuffer;
                grounded = true;
				if (!wasGrounded)
					onLandEvent.Invoke();
			}
		}

		Move(horizontalMove * Time.fixedDeltaTime);
	}


	public void Move(float move)
	{
        
		//only control the player if grounded or airControl is turned on
		if (grounded || airControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, rigidBody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			rigidBody2D.velocity = Vector3.SmoothDamp(rigidBody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !facingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && facingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		// If the player should jump...
		if (grounded && jumpRemember > 0)
		{
			jumpRemember = 0f;
            groundedRemember = 0f;
			// Add a vertical force to the player.
			grounded = false;
			rigidBody2D.AddForce(new Vector2(0f, jumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}