using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
    //there is a proper proper way to do UI, I just heavily simplified what we need here 
    public GameObject buttons;
    public GameObject credits;
    public void Start()
    {
        credits = GameObject.Find("CreditsCanvas");
        buttons = GameObject.Find("ButtonsCanvas");

        credits.SetActive(false); 
    }
    public void StartScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
