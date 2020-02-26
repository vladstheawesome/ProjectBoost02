using ProjectBoost.ImpactSystem;
using ProjectBoost.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Jetpack : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip damageImpact;

    [SerializeField] ParticleSystem jetPackParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    DamageOnCollision typeOfDamage;
    PlayerHealthBar player;
    private float trackHealth;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<PlayerHealthBar>();
    }

    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotate();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return; // ignore collisions when dead
        }

         switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    print("safe");
                }
                break;
            case "Finish":
                {
                    PlayerSuccess();
                }
                break;
            case "Healing":
                {
                    PlayerHeal(collision);
                }
                break;
            default:
                {
                    PlayerTakeDamage(collision);
                }
                break;
        }
    }

    

    private void PlayerSuccess()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void PlayerTakeDamage(Collision collision)
    {
        if (collision.gameObject.GetComponent<DamageOnCollision>() != null)
        {  
            var collisionType = collision.gameObject.GetComponent<DamageOnCollision>().damageImpact;
            var collisionDamage = player.GetDamageValues(collisionType);

            player.TakeDamage(collisionDamage);
            var currentHealth = player.GetCurrentHealth(trackHealth);

            if (currentHealth > Mathf.Epsilon) 
            {
                state = State.Alive;
                audioSource.PlayOneShot(damageImpact);
                // TODO: realign player along the Y-Axis
            }
            else
            {
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                deathParticles.Play();
                Invoke("LoadFirstLevel", levelLoadDelay);
            }
        }        
    }

    private void PlayerHeal(Collision collision)
    {
        if (collision.gameObject.GetComponent<HealingOnCollision>() != null)
        {
            var healingType = collision.gameObject.GetComponent<HealingOnCollision>().healingEffect;
            var collisionHealing = player.GetHealingValues(healingType);

            player.GetHealing(collisionHealing);
            var currentHealth = player.GetCurrentHealth(trackHealth);

            if (currentHealth > Mathf.Epsilon || currentHealth < 100f)
            {
                state = State.Alive;
                // TODO: play healing sound
                // TODO: destroy gameobject
            }
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            jetPackParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
            jetPackParticles.Play();
        }
        //jetPackParticles.Play();
    }

    private void RespondToRotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics
    }    
}
