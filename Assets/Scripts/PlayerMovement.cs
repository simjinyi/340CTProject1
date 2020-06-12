using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	// This is a reference to the Rigidbody component called "rigidBody"
	public Rigidbody rigidBody;

	public float forwardForce = 2000f;  // Variable that determines the forward force
	public float sidewaysForce = 500f;  // Variable that determines the sideways force

	// We marked this as "Fixed"Update because we
	// are using it to mess with physics.
	void FixedUpdate()
	{
		// Add a forward force if the velocity is less than maximum
		if (rigidBody.velocity.magnitude < 30.0)
			rigidBody.AddForce(0, 0, forwardForce * 2 * Time.deltaTime);

		rigidBody.AddForce(0, -forwardForce * Time.deltaTime, 0);

		if (Input.GetKey(KeyCode.RightArrow))  // If the player is pressing the "d" key
		{
			// Add a force to the right
			rigidBody.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		if (Input.GetKey(KeyCode.LeftArrow))  // If the player is pressing the "a" key
		{
			// Add a force to the left
			rigidBody.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
		}

		//if (Input.GetKey("s"))  // If the player is pressing the "s" key
		//{
		//	// Add a force to the left
		//	rigidBody.AddForce(0, 0, -forwardForce * Time.deltaTime);
		//}
	}
}