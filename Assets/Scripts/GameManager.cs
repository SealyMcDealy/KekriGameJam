using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AnimationStatus //basically an array of ints, useful to implement if you need to do a bunch of sorting and make it less ambigious
{
    Aware = 0,
    Closed = 1,
    Damage = 2,
    Tired = 3

}


public class GameManager : MonoBehaviour
{
    public List<Sprite> StatusImages; //ensure list indexes match the enum's indexes
    public Image statusImage;

    public static bool IsHit;  //static means there's only ever 1 instance of this variable as it's class bound and not object bound
                                //so if you had 15 game manager objects, the value would be changing globally across them all

    public float timer = 10; //you can set the default value right from the get go. Usually prefferable to set it in the inspector

    private bool isBlinking;

    public TextMeshProUGUI civillianText;    //also general coding practices dictate that if it's a private field you do somethingLikeThis 
    public TextMeshProUGUI fireText;        //but if it's public you do SomethingLikeThis.
                                            //I don't care that much about it, just make sure you understand what is what
    public int civillianCount;             
    public int fireCount;

    private AudioSource audioSource;

    public RagdollLogic[] Civillians;
    public Fire[] Fire;
    void Start()
    {
        statusImage = GameObject.Find("StatusImage").GetComponent<Image>();

        audioSource = GetComponent<AudioSource>();      //GetComponent searches for the specified component on the object the script is attached to

        Civillians = GameObject.FindObjectsOfType<RagdollLogic>();  //so in order to get an array of other objects, we do FindObjectsOfType
        Fire = GameObject.FindObjectsOfType<Fire>();                //be sure that it's Objects and not Object 
    }

    void TextUpdater()
    {
        civillianText.text = "Civillians rescued " + civillianCount + " out of " + Civillians.Length;   //Spaces in " " are intentional
        fireText.text = "Fires put out " + fireCount + " out of " + Fire.Length;                        //they uh space out text 
    }

    void Update()
    {
        TextUpdater();
        timer -= Time.deltaTime;     //same as timer = timer - Time.deltaTime;
        if (IsHit == true)
        {
            StopAllCoroutines();
            StartCoroutine(GotHit());
        }

        if (timer <= 0 && IsHit == false && isBlinking == false)  // == equal, && and, || or
        {
            timer = 0;
            isBlinking = true;
            StartCoroutine(Blinking());
        }

        if (civillianCount == Civillians.Length && fireCount == Fire.Length)   //since we have an array Civillians, we can just use its length for an if statement
        {
            audioSource.Play();
        }
    }
                    //IEnumerators or Coroutines are an async function.
                    //Async means it runs in parallel to the main stuff and there can be multiple of them running at once from the same script
                    //since stuff like Update can only be called once
                    //They also have some internal WaitForSomething logic so they're useful as mini Update functions 
                    //there's more to them but that's essentially why they're used
    IEnumerator Blinking()          
    {
        statusImage.sprite = StatusImages[Convert.ToInt32(AnimationStatus.Tired)];

        yield return new WaitForSeconds(0.6f);

        statusImage.sprite = StatusImages[Convert.ToInt32(AnimationStatus.Closed)];

        yield return new WaitForSeconds(0.6f);

        statusImage.sprite = StatusImages[Convert.ToInt32(AnimationStatus.Tired)];

        yield return new WaitForSeconds(0.6f);

        statusImage.sprite = StatusImages[Convert.ToInt32(AnimationStatus.Aware)];

        timer = 10 + UnityEngine.Random.Range(-4, 4);
        isBlinking = false;
        yield return null;
    }
    IEnumerator GotHit()
    {
        IsHit = false;

        //could I have used ints instead? yes. Do I want it to be scaleable in the future for no reason whatsoever?
        //yes

        //robo remind me to walk you through the scripts and the patterns I'm using
        statusImage.sprite = StatusImages[Convert.ToInt32(AnimationStatus.Damage)];

        yield return new WaitForSeconds(2f);

        statusImage.sprite = StatusImages[Convert.ToInt32(AnimationStatus.Aware)];

        if (timer <= 0)
        {
            timer = 10 + UnityEngine.Random.Range(-4, 4);
        }
        yield return null;
    }
}
