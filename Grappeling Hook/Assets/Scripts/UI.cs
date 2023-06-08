using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    private GrappelingHook hook;

    [SerializeField]
    private GameObject swingLable;
    [SerializeField]
    private GameObject grappleLable;
    [SerializeField]
    private GameObject menu;

    private void Awake()
    {
        Time.timeScale = 0f;
        hook = GameObject.Find("hookBase").GetComponent<GrappelingHook>();
    }

    private void Update()
    {
        //Sets lable in the UI showing grappeling gun settings.
        if (hook.grappelingHookMode == Modes.Swinging)
        {
            swingLable.SetActive(true);
            grappleLable.SetActive(false);
        }

        if (hook.grappelingHookMode == Modes.Grappeling)
        {
            grappleLable.SetActive(true);
            swingLable.SetActive(false);
        }

        //Ends program when pressing Escape.
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void StartMenu()
    {
        //Starting Program
        Time.timeScale = 1.0f;
        menu.SetActive(false);

        //Sets the cursor to the center of the screen.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
