using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    //[SerializeField] GameObject sceneObjects;
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

        //DontDestroyOnLoad(this);
    }

    public void LevelChanger(string level)
    {
        StartCoroutine(ChangeLevel(level));
    }

    private IEnumerator ChangeLevel(string levelToLoad)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        //sceneObjects.SetActive(false);
        //SceneManager.LoadScene(levelToLoad, LoadSceneMode.Additive);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Resources.UnloadUnusedAssets();
        //yield return null;
    }
}
