using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AnimationStatus
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

    public static bool IsHit;

    public float timer = 10;

    private bool IsBlinking;

    void Start()
    {
        statusImage = GameObject.Find("StatusImage").GetComponent<Image>();
    }

    void Update()
    {
        timer -= Time.deltaTime; 
        if (IsHit == true)
        {
            StopAllCoroutines();
            StartCoroutine(GotHit());
        }

        if (timer <= 0 && IsHit == false && IsBlinking == false)
        {
            timer = 0;
            IsBlinking = true;
            StartCoroutine(Blinking());

        }
    }
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
        IsBlinking = false;
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
