using UnityEngine;

public class RigidBodyFollower : MonoBehaviour
{
    public Rigidbody kinematicRigidbody;
    public GameObject Gotofollower;

    void FixedUpdate()
    {
        if (kinematicRigidbody != null && Gotofollower != null)
        {
            // Set the Rigidbody's position and rotation to match the Gotofollower's position and rotation
            kinematicRigidbody.MovePosition(Gotofollower.transform.position);
            kinematicRigidbody.MoveRotation(Gotofollower.transform.rotation);
        }
    }
}