using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ComputerKey : MonoBehaviour
{
    public string keyCode;

    public float lastKeyPress = 0f;

    public float keyPressDebounce = 0.2f;

    public Material pressedMaterial;

    public Material defaultMaterial;

    public bool testPress = false;

    public string triggerTag;

    public Computer computer;

    public AudioSource keyPressSound;

    public Renderer buttonRenderer;

    private void Start()
    {
        buttonRenderer = GetComponent<Renderer>();
        computer = Computer.instance;
    }
    private void Update()
    {


        if (testPress)
        {
            testPress = false;
            PressKey();
            lastKeyPress = Time.time;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            if (Time.time > lastKeyPress + keyPressDebounce)
            {
                PressKey();
                lastKeyPress = Time.time;
                keyPressSound.PlayOneShot(keyPressSound.clip);
            }
        }
    }
    private async void PressKey()
    {
        computer.KeyFunction(this);
        buttonRenderer.material = pressedMaterial;
        await Task.Delay(150);
        buttonRenderer.material = defaultMaterial;
    }
}
