using UnityEngine;

public class PhysicsRig : MonoBehaviour
{
    [Header("Controller Tracking (Targets)")]
    [Tooltip("Transforms provided by the VR system.")]
    [SerializeField] private Transform leftController;
    [SerializeField] private Transform rightController;

    [Header("Physics Hands")]
    [Tooltip("Rigidbodies that are driven by the joint to simulate physics interactions.")]
    [SerializeField] private Rigidbody leftHandRigidbody;
    [SerializeField] private Rigidbody rightHandRigidbody;

    [Header("Hand Joints")]
    [SerializeField] private ConfigurableJoint leftHandJoint;
    [SerializeField] private ConfigurableJoint rightHandJoint;

    [Header("Visual Hand Models")]
    [Tooltip("These should be the in-game models that will be updated to follow the physics objects.")]
    [SerializeField] private Transform leftHandVisual;
    [SerializeField] private Transform rightHandVisual;

    [Header("Visual Update Smoothing")]
    [Range(0f, 1f)]
    [SerializeField] private float visualSmoothing = 1f;

    private void FixedUpdate()
    {
        // Drive the physics objects toward the tracked controller targets.
        if (leftController != null && leftHandJoint != null)
        {
            leftHandJoint.targetPosition = leftController.localPosition;
            leftHandJoint.targetRotation = leftController.localRotation;
        }

        if (rightController != null && rightHandJoint != null)
        {
            rightHandJoint.targetPosition = rightController.localPosition;
            rightHandJoint.targetRotation = rightController.localRotation;
        }
    }

    private void LateUpdate()
    {
        // Update the visual hand models to follow the physics-simulated hands.
        // Using LateUpdate ensures we capture the final positions after physics simulation.
        if (leftHandVisual != null && leftHandRigidbody != null)
        {
            if (visualSmoothing < 1f)
            {
                leftHandVisual.position = Vector3.Lerp(leftHandVisual.position, leftHandRigidbody.position, visualSmoothing);
                leftHandVisual.rotation = Quaternion.Slerp(leftHandVisual.rotation, leftHandRigidbody.rotation, visualSmoothing);
            }
            else
            {
                leftHandVisual.position = leftHandRigidbody.position;
                leftHandVisual.rotation = leftHandRigidbody.rotation;
            }
        }

        if (rightHandVisual != null && rightHandRigidbody != null)
        {
            if (visualSmoothing < 1f)
            {
                rightHandVisual.position = Vector3.Lerp(rightHandVisual.position, rightHandRigidbody.position, visualSmoothing);
                rightHandVisual.rotation = Quaternion.Slerp(rightHandVisual.rotation, rightHandRigidbody.rotation, visualSmoothing);
            }
            else
            {
                rightHandVisual.position = rightHandRigidbody.position;
                rightHandVisual.rotation = rightHandRigidbody.rotation;
            }
        }
    }
}