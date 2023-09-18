using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float loadDelay = 1f;
    [SerializeField] float crashAudioVolume = 1f;
    [SerializeField] float successAudioVolume = 1f;
    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip successSFX;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource playerAudioSource;
    BoxCollider playerBoxCollider;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        playerAudioSource = GetComponent<AudioSource>();
        playerBoxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        CheatLoadNextLevel();
        CheatDisableCollision();
    }

    void OnCollisionEnter(Collision other) 
    {

        if (isTransitioning || collisionDisabled) {return;} // Prevents collision handling if player in a transition state
        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This object is friendly.");
                break;

            case "Finish":
                    NextLevelSequence();
                break;

            default:
                    CrashSequence();
                break;
        }
    }

    void CrashSequence()
    {
        isTransitioning = true;

        playerAudioSource.Stop();
        playerAudioSource.PlayOneShot(crashSFX, crashAudioVolume);

        crashParticles.Play();

        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", loadDelay);
    }

    void NextLevelSequence()
    {
        isTransitioning = true;

        playerAudioSource.Stop();
        playerAudioSource.PlayOneShot(successSFX, successAudioVolume);

        successParticles.Play();

        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", loadDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    void CheatLoadNextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
            Debug.Log("Loaded Next Level.");
        }
    }

    void CheatDisableCollision()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled;
            Debug.Log("Toggled Collision");
        }
    }

}
