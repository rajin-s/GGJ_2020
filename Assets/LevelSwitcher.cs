using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] LevelManager manager;
    [SerializeField] private string levelToLoad;
    [SerializeField] private string playerTag;
    [SerializeField] private string button;
    [SerializeField] private bool inside = false;
    private bool canTransition = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Ahhhhhhhhhhhhhhhhhhhhhhhhh");
        if (other.gameObject.CompareTag(playerTag))
        {
            //Debug.Log("Ahhhhhhhhhhhhhhhhhhhhhhhhh");
            Debug.Log(other.tag);
            inside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(playerTag))
        {
            inside = false;
        }
    }
    
    private void Update()
    {
        if (Input.GetButtonDown(button) && canTransition && inside)
        {
            Debug.Log(levelToLoad);
            canTransition = false;
            ChangeLevel();
        }    
    }

    public void ChangeLevel()
    {
        manager.LevelChanger(levelToLoad);
    }
}
