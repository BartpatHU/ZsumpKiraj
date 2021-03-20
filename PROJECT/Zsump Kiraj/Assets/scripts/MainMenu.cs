using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    float progress;
    public AudioSource audioSource;



    public void PlayGame(int sceneIndex)
    {
        audioSource.Stop();
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / .9f);
            Debug.Log(progress);
            yield return null;
        }
    }

    void FixedUpdate()
    {

        slider.value = progress;
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }



}
