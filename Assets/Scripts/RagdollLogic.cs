using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollLogic : MonoBehaviour
{
    public GameObject physicsRagdoll;
    public GameObject standingRagdoll;

    private GameManager manager;

    public List<AudioClip> voiceLines;
    public AudioSource source;
    void Start()
    {
        standingRagdoll = gameObject.transform.GetChild(0).gameObject;
        physicsRagdoll = standingRagdoll.transform.GetChild(6).gameObject;

        manager = GameObject.FindObjectOfType<GameManager>();

        source = gameObject.GetComponent<AudioSource>();
        physicsRagdoll.SetActive(false);
    }
    private void OnDestroy()
    {
        manager.civillianCount++;
    }
}
