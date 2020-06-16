using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private const string FENCE_TAG = "Wall";

	private const string LEFT_WALL_TAG = "Wall Left";
	private const string RIGHT_WALL_TAG = "Wall Right";

	private bool enable_move_left;
	private bool enable_move_right;

	// This is a reference to the Rigidbody component called "rigidBody"
	public Rigidbody rigidBody;

	public float forwardForce = 2000f;  // Variable that determines the forward force
	public float sidewaysForce = 500f;  // Variable that determines the sideways force

	public PlayerMovement()
    {
		enable_move_left = enable_move_right = true;
    }

	// We marked this as "Fixed"Update because we
	// are using it to mess with physics.
	void FixedUpdate()
	{
		// Add a forward force if the velocity is less than maximum
		if (rigidBody.velocity.magnitude < 70.0)
			rigidBody.AddForce(0, 0, forwardForce * 2 * Time.deltaTime);

		rigidBody.AddForce(0, -forwardForce * Time.deltaTime, 0);

		if (Input.GetKey(KeyCode.RightArrow))  // If the player is pressing the "d" key
		{
			if (enable_move_right)
				// Add a force to the right
				rigidBody.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		if (Input.GetKey(KeyCode.LeftArrow))  // If the player is pressing the "a" key
		{
			if (enable_move_left)
				// Add a force to the left
				rigidBody.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		//if (Input.GetKey("s"))  // If the player is pressing the "s" key
		//{
		//	// Add a force to the left
		//	rigidBody.AddForce(0, 0, -forwardForce * Time.deltaTime);
		//}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == FENCE_TAG)
			enable_move_left = enable_move_right = false;

		// Check if colliding with the walls
		if (collision.gameObject.tag == LEFT_WALL_TAG)
		{
			Debug.Log(LEFT_WALL_TAG);
			enable_move_left = false;
			rigidBody.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		if (collision.gameObject.tag == RIGHT_WALL_TAG)
		{
			Debug.Log(RIGHT_WALL_TAG);
			enable_move_right = false;
			rigidBody.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		//if (collision.gameObject.tag == "Answer")
		//	Debug.Log(collision.transform.localPosition.z);
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
}