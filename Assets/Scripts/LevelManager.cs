using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //public static LevelManager instance;
    //[SerializeField] GameObject sceneObjects;
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime = 1f;

    public void LevelChanger(string level)
    {
        StartCoroutine(ChangeLevel(level));
    }

    private IEnumerator ChangeLevel(string levelToLoad)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        //sceneObjects.SetActive(false);
        //SceneManager.LoadScene(levelToLoad);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);

        //Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Resources.UnloadUnusedAssets();
        //yield return null;
    }
}
