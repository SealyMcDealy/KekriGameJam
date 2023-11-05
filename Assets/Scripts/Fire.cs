using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private GameManager manager;   
    private ParticleSystem fireParticles;
    void Start()
    {
        fireParticles = GetComponent<ParticleSystem>();
        manager = GameObject.FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (fireParticles.maxParticles <= 0)
        {
            Destroy(gameObject);
            manager.fireCount++;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("hit collision");
        fireParticles.maxParticles -= 5;
    }
    private void OnParticleTrigger()
    {
        Debug.Log("hit trigger");
        fireParticles.maxParticles -= 5;
    }
}
