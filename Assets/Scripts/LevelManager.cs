using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] GameObject sceneObjects;
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime = 1f;

    private void Awake()
    {
        if (instance != null)
        {
            GameObject.Destroy(instance);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);
    }

    public IEnumerator ChangeLevel(string levelToLoad)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        sceneObjects.SetActive(false);
        //SceneManager.LoadScene(levelToLoad, LoadSceneMode.Additive);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        //yield return null;
    }

    public IEnumerator EndLevel(string currentLevel)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentLevel);

        // Wait until the asynchronous scene fully loads
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
        
        Resources.UnloadUnusedAssets();
        sceneObjects.SetActive(true);
        //yield return null;
    }
}
