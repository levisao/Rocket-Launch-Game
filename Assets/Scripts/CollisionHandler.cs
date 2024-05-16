using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]private float delayLoadScene = 1f;
    [SerializeField] private AudioClip playerCrash;
    [SerializeField] private AudioClip playerFinish;

    [SerializeField] private ParticleSystem particleCrash;
    [SerializeField] private ParticleSystem particleFinish;


    private AudioSource audioS;

    bool isTransiotioningScene = false;
    bool collisionDisabled = false;

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys() // botar dentro do if q danilo ensinou?
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; //mudar valor do boolean (desabilitar ou habilitar collision)
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransiotioningScene || collisionDisabled) { return; }  // return vai sair do metodo quando istransitioninScene for true, q fica true nos metodos dentro

        switch (collision.gameObject.tag) //switch is bad because it's hard to change if variable is string      
        {
            case "Friendly":
                Debug.Log("HI");
                break;
            case "Finish":
                //ReloadNextLevel();
                StartSuccessSequence();
                break;
            default:  //anywhere else
                StartCrashSequence();
                break;
        }
            
    }

    private void StartSuccessSequence()
    {
        isTransiotioningScene = true;
        audioS.Stop();
        audioS.PlayOneShot(playerFinish);
        particleFinish.Play();
        GetComponent<Movement>().enabled = false; //por estar no mesmo objeto que está o script movimento, é simples assim
        Invoke("LoadNextLevel", delayLoadScene); //not good performer, and bad because it's string. Use Coroutines instead

        
    }

    private void StartCrashSequence()
    {
        isTransiotioningScene = true;
        audioS.Stop();
        audioS.PlayOneShot(playerCrash);
        particleCrash.Play();
        GetComponent<Movement>().enabled = false; //por estar no mesmo objeto que está o script movimento, é simples assim
        Invoke("ReloadLevel", delayLoadScene);
        
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex); //every scene has a int number
        isTransiotioningScene = false;
    }
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) // checando quantas cenas tem no build settings para ver se foi a ultima
        {
            nextSceneIndex = 0; //volta para a primeira cena
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
