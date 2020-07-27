using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private const float MAX_SPEED = 80.0f;

	private const string FENCE_TAG = "Wall";

	private const string LEFT_WALL_TAG = "Wall Left";
	private const string RIGHT_WALL_TAG = "Wall Right";

	private bool enable_move_left;
	private bool enable_move_right;

	// This is a reference to the Rigidbody component called "rigidBody"
	public Rigidbody rigidBody;

	public float forwardForce = 2000f;  // Variable that determines the forward force
	public float sidewaysForce = 500f;  // Variable that determines the sideways force

	private float speed;

	public Gameplay gameplay;

	public void SetSpeed(float speed)
    {
		if (speed <= 30)
			return;

		if (speed >= 80)
			return;

		this.speed = speed;
    }

	public void IncrementSpeed()
    {
		SetSpeed(speed + 1);
    }

	public PlayerMovement()
    {
		enable_move_left = enable_move_right = true;
		speed = 30;
	}

	// Use FixedUpdate to ensure the physics uphold
	void FixedUpdate()
	{
		// Add a forward force if the velocity is less than maximum
		if (rigidBody.velocity.magnitude < speed)
			rigidBody.AddForce(0, 0, forwardForce * 2 * Time.deltaTime);

		rigidBody.AddForce(0, -forwardForce * Time.deltaTime, 0);

		// If the player is pressing the right arrow key
		if (Input.GetKey(KeyCode.RightArrow))  
		{
			if (enable_move_right)
				// Add a force to the right
				rigidBody.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		// If the player is pressing the left arrow key
		if (Input.GetKey(KeyCode.LeftArrow))  
		{
			if (enable_move_left)
				// Add a force to the left
				rigidBody.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		// Add force automatically when colliding with the fence
		if (collision.gameObject.tag == FENCE_TAG)
		{
			rigidBody.AddForce(sidewaysForce * 2 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
			enable_move_left = enable_move_right = false;
		}

		// Check if colliding with the walls
		if (collision.gameObject.tag == LEFT_WALL_TAG)
		{
			Debug.Log(LEFT_WALL_TAG);
			enable_move_left = false;
			rigidBody.AddForce(sidewaysForce * 2 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		if (collision.gameObject.tag == RIGHT_WALL_TAG)
		{
			Debug.Log(RIGHT_WALL_TAG);
			enable_move_right = false;
			rigidBody.AddForce(-sidewaysForce * 2 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		// Callback when collided with the answer
		if (collision.gameObject.tag == "Answer")
			gameplay.AnswerCallback(collision.gameObject);
	}

	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == FENCE_TAG)
			enable_move_left = enable_move_right = true;

		// Check if colliding with the walls
		if (collision.gameObject.tag == LEFT_WALL_TAG)
			enable_move_left = true;

		if (collision.gameObject.tag == RIGHT_WALL_TAG)
			enable_move_right = true;
	}

    private void OnTriggerEnter(Collider collider)
    {
		// Add a life to the player
		if (collider.gameObject.tag == "Life")
			StartCoroutine(gameplay.AddLifeCallback(collider.gameObject));
	}
}