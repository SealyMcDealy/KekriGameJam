using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debriefing : MonoBehaviour
{
    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Debrief());
    }

    IEnumerator Debrief()
    {

        yield return new WaitForSeconds(audioSource.clip.length);
        SceneManager.LoadScene(2);
    }

}
