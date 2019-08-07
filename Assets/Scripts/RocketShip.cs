using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketShip : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 10f;
    [SerializeField] AudioClip thrustSFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip finishSFX;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //todo - make sure sfx stop on death/finish
        if (state == State.Alive)
        {
            ApplyThrustInput();
            ApplyRotationInput();
        } 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }  //if not alive, stop collisions

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                state = State.Transcending;
                audioSource.PlayOneShot(finishSFX);
                Invoke("LoadNextScene", 1f); //giving 1 second delay
                break;

            default:
                state = State.Dying;
                audioSource.PlayOneShot(deathSFX);
                Invoke("RestartFirstScene",  1f);
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1); // todo allow for more then 2 levels
    }

    private void RestartFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void ApplyRotationInput()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics

    }

    private void ApplyThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ThrustInput();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ThrustInput()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustSFX);
        }
    }
}
