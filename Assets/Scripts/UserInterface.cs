using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
    public GameObject buttons;
    public GameObject credits;
    public void Start()
    {
        credits = GameObject.Find("CreditsCanvas");
        buttons = GameObject.Find("ButtonsCanvas");

        credits.SetActive(false); 
    }

    public void Credits()
    {

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
