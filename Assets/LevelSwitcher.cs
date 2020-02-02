using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] string levelToLoad;
    [SerializeField] string playerTag;
    [SerializeField] string button;
    [SerializeField] bool inside;
    private bool canTransition = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Ahhhhhhhhhhhhhhhhhhhhhhhhh");
        if (other.gameObject.CompareTag(tag))
        {
            //Debug.Log("Ahhhhhhhhhhhhhhhhhhhhhhhhh");
            inside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(tag))
        {
            inside = false;
        }
    }
    
    private void Update()
    {
        if (Input.GetButtonDown(button) && canTransition)
        {
            canTransition = false;
            ChangeLevel();
        }    
    }

    public void ChangeLevel()
    {
        LevelManager.instance.LevelChanger(levelToLoad);
    }
}
