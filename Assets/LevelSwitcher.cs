using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField]string levelToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(LevelManager.instance.ChangeLevel(levelToLoad));
    }
}
