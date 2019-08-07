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

    //[SerializeField] ParticleSystem thrustPS;
    [SerializeField] ParticleSystem deathPS;
    [SerializeField] ParticleSystem finishPS;

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
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotationInput();
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
                StartSuccessSequence();
                break;

            default:
                StartDeathSequence();
                break;
        }
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ThrustInput();
        }
        else
        {
            audioSource.Stop();
            //thrustPS.Stop();
        }
    }

    private void ThrustInput()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustSFX);
        }
        //thrustPS.Play();
    }

    private void RespondToRotationInput()
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

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSFX);
        //thrustPS.Stop();
        finishPS.Play();
        Invoke("LoadNextScene", 1f); //giving 1 second delay
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1); // todo allow for more then 2 levels
        finishPS.Stop();
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSFX);
        //thrustPS.Stop();
        deathPS.Play();
        Invoke("RestartFirstScene", 1f);
    }

    private void RestartFirstScene()
    {
        SceneManager.LoadScene(0);
        deathPS.Stop();
    }

}
