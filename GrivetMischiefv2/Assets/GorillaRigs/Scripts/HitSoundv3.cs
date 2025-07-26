using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class HitSoundv3 : MonoBehaviour
{
    [System.Serializable]
    public class TagSound
    {
        public string tag;
        public AudioClip[] sounds;
    }

    [Header("Experimental")]
    public bool velocityBasedHitsound = true;
    public float maxVelocity = 10f;
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;
    public float minVolume = 0.5f;
    public float maxVolume = 1f;

    public TagSound[] tagSoundMappings;
    public AudioSource audioSource;
    public float velocityThreshold = 1f;
    public float vibrationAmplitude = 0.15f;
    public float vibrationDuration = 0.05f;
    public bool haptics;
    public bool left;

    private Vector3 prevPosition;

    private void Start()
    {
        prevPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (velocityBasedHitsound)
        {
            Vector3 velocity = (transform.position - prevPosition) / Time.deltaTime;
            float collisionVelocity = velocity.magnitude;

            if (collisionVelocity <= velocityThreshold)
            {
                PlayHitSoundAndVibration(other.tag, collisionVelocity);
            }
            else
            {
                PlayHitSoundAndVibration(other.tag, 5f);
            }
        }
        else
        {
            PlayHitSoundAndVibration(other.tag, 5f);
        }

        prevPosition = transform.position;
    }

    private void PlayHitSoundAndVibration(string tag, float collisionVelocity)
    {
        AudioClip selectedSound = GetRandomSoundForTag(tag);

        if (selectedSound == null) return;

        float pitch, volume;

        if (collisionVelocity <= maxVelocity)
        {
            pitch = Random.Range(minPitch, maxPitch);
            volume = Random.Range(minVolume, maxVolume);
        }
        else
        {
            pitch = Mathf.Lerp(minPitch, maxPitch, collisionVelocity / maxVelocity);
            volume = Mathf.Lerp(minVolume, maxVolume, collisionVelocity / maxVelocity);
        }

        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.clip = selectedSound;
        audioSource.Play();

        if (haptics)
        {
            StartHapticPulses(left);
        }
    }


    private AudioClip GetRandomSoundForTag(string tag)
    {
        foreach (TagSound tagSound in tagSoundMappings)
        {
            if (tagSound.tag == tag && tagSound.sounds.Length > 0)
            {
                return tagSound.sounds[Random.Range(0, tagSound.sounds.Length)];
            }
        }
        return null;
    }

    private void StartHapticPulses(bool isLeftHand)
    {
        StartCoroutine(HapticPulses(isLeftHand ? XRNode.LeftHand : XRNode.RightHand));
    }

    private IEnumerator HapticPulses(XRNode handNode)
    {
        float startTime = Time.time;
        InputDevice device = InputDevices.GetDeviceAtXRNode(handNode);

        while (Time.time < startTime + vibrationDuration)
        {
            device.SendHapticImpulse(0, vibrationAmplitude, vibrationDuration);
            yield return new WaitForSeconds(vibrationDuration);
        }
    }
}
