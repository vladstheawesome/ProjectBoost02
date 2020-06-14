using ProjectBoost.ImpactSystem;
using ProjectBoost.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using ProjectBoost.FuelSystem;

namespace ProjectBoost.Control
{
    public enum TransitionParameter
    {
        FloatThrust,
        RotateForward,
        Rotatebackwards,
        Landing,
    }

    public class AstronautControl : MonoBehaviour
    {
        [Header("Keyboard Input")]
        public bool FloatThrust;
        public bool RotateForward;
        public bool RotateBackwards;

        [Header("Thrust Info & General")]
        [SerializeField] float rcsThrust = 100f;
        [SerializeField] float mainThrust = 100f;
        [SerializeField] float levelLoadDelay = 4f;

        [Header("Audio Clips")]
        [SerializeField] AudioClip mainEngine;
        [SerializeField] AudioClip success;
        [SerializeField] AudioClip death;
        [SerializeField] AudioClip damageImpact;
        [SerializeField] AudioClip healingPortion;
        [SerializeField] AudioClip refuelPortion;

        [Header("Player Particles")]
        [SerializeField] ParticleSystem jetPackParticles;
        [SerializeField] ParticleSystem successParticles;
        [SerializeField] ParticleSystem deathParticles;
        [SerializeField] ParticleSystem healParticles;
        [SerializeField] ParticleSystem refuelParticles;

        Rigidbody rigidBody;
        AudioSource audioSource;

        enum State { Alive, Dying, Transcending }
        State state = State.Alive;

        DamageOnCollision typeOfDamage;
        PlayerHealthBar player;
        PlayerFuelBar playerFuel;
        GameObject astronaut;

        private float trackHealth;
        private float trackFuel;
        private float fuelThrustValue = 5f;
        private Animator anim;

        void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            player = GetComponent<PlayerHealthBar>();
            playerFuel = GetComponent<PlayerFuelBar>();
            astronaut = GameObject.FindWithTag("Player");
            anim = GetComponentInChildren<Animator>();
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
                case "Refueling":
                    {
                        PlayerRefuel(collision);
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

                    // On collision with any object, make sure player is still facing front (left to right)
                    // and lock the Z-axis, so they are still moving along the gamepath
                    astronaut.transform.eulerAngles = new Vector3(0, 0, 0);
                    astronaut.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
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
            var currentHealth = player.GetCurrentHealth(trackHealth);
            var maxHealth = player.GetMaxHealth(trackHealth);

            if (collision.gameObject.GetComponent<HealingOnCollision>() != null)
            {
                var healingType = collision.gameObject.GetComponent<HealingOnCollision>().healingEffect;
                var collisionHealing = player.GetHealingValues(healingType);

                if (currentHealth > Mathf.Epsilon || currentHealth < maxHealth)
                {
                    player.GetHealing(collisionHealing);

                    Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), astronaut.GetComponent<Collider>(), false);
                    state = State.Alive;
                    audioSource.PlayOneShot(healingPortion);
                    healParticles.Play();
                    Destroy(collision.gameObject);
                }
            }
        }

        private void PlayerRefuel(Collision collision)
        {
            var currentFuel = playerFuel.GetCurrentFuel(trackFuel);
            var maxFuel = playerFuel.GetMaxFuel(trackFuel);

            if (collision.gameObject.GetComponent<RefuelOnCollision>() != null)
            {
                var refuelAmount = collision.gameObject.GetComponent<RefuelOnCollision>().fuelAmount;
                var collisionFueling = playerFuel.GetFuelValues(refuelAmount);

                if (currentFuel > Mathf.Epsilon || currentFuel < maxFuel)
                {
                    playerFuel.ReFuel(collisionFueling);

                    Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), astronaut.GetComponent<Collider>(), false);
                    state = State.Alive;
                    audioSource.PlayOneShot(refuelPortion);
                    refuelParticles.Play();
                    Destroy(collision.gameObject);
                }

            }
        }

        private void LoadNextLevel()
        {
            SceneManager.LoadScene(2);
        }

        private void LoadFirstLevel()
        {
            SceneManager.LoadScene(1);
        }

        private void RespondToThrustInput()
        {
            //if (Input.GetKey(KeyCode.Space)) // can thrust while rotating
            if (this.FloatThrust)
            {
                // Only thrust if we have fuel
                var currentFuel = playerFuel.GetCurrentFuel(trackFuel);
                if (currentFuel > Mathf.Epsilon)
                {
                    ApplyThrust();
                }
            }
            else
            {
                audioSource.Stop();
                jetPackParticles.Stop();
                anim.SetBool(TransitionParameter.FloatThrust.ToString(), false);
            }
        }

        private void ApplyThrust()
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            var playerFuel = GameObject.FindWithTag("Player").GetComponent<PlayerFuelBar>();
            if (!audioSource.isPlaying)
            {
                UseJetPackFuel(playerFuel);
                audioSource.PlayOneShot(mainEngine);
                jetPackParticles.Play();
                // TODO float animation from ANIMATOR
                anim.SetBool(TransitionParameter.FloatThrust.ToString(), true);
            }
        }

        private void UseJetPackFuel(PlayerFuelBar usefuel)
        {

            if (usefuel != null)
            {
                usefuel.UseFuel(fuelThrustValue);
                var currentFuel = usefuel.GetCurrentFuel(trackFuel);
            }
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
}