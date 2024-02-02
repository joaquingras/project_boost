using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
  private AudioSource _audioSrc;
  private bool _isTransitioning = false;

  [SerializeField] float levelLoadDelay = 2f;
  [SerializeField] AudioClip crash;
  [SerializeField] AudioClip success;
  [SerializeField] ParticleSystem successPs;
  [SerializeField] ParticleSystem crashPs;

  void Start()
  {
    _audioSrc = GetComponent<AudioSource>();
  }

  void Update()
  {
    ProcessReloadLevel();
  }

  private void ProcessReloadLevel()
  {
    if (Input.GetKey(KeyCode.L))
    {
      Debug.Log("L presed");
    }
  }

  void OnCollisionEnter(Collision other)
  {
    var collisionTagObject = other.gameObject.tag;

    if (_isTransitioning)
    {
      return;
    }

    switch (collisionTagObject)
    {

      case "Friendly":
        Debug.Log("Friendly object");
        break;
      case "Finish":
        StartSuccessSequence();
        break;

      default:
        StartCrashSequence();
        break;

    }

    Debug.Log(other.gameObject.tag);
  }

  void StartCrashSequence()
  {
    _isTransitioning = true;
    _audioSrc.Stop();

    playClip(crash);
    crashPs.Play();

    GetComponent<Movement>().enabled = false;
    Invoke(nameof(ReloadLevel), levelLoadDelay);
  }

  void ReloadLevel()
  {
    var currentScene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(currentScene.buildIndex);
  }

  void StartSuccessSequence()
  {
    _isTransitioning = true;
    _audioSrc.Stop();

    playClip(success);
    successPs.Play();

    GetComponent<Movement>().enabled = false;
    Invoke(nameof(LoadNextLevel), levelLoadDelay);
  }

  void LoadNextLevel()
  {
    var currentSceneId = SceneManager.GetActiveScene().buildIndex;
    int nextSceneId = currentSceneId + 1;
    if (nextSceneId == SceneManager.sceneCountInBuildSettings)
    {
      nextSceneId = 0;
    }

    SceneManager.LoadScene(nextSceneId);
  }

  private void playClip(AudioClip clip)
  {
    if (!_audioSrc.isPlaying)
    {
      _audioSrc.PlayOneShot(clip);
    }
  }
}
