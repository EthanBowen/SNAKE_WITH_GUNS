using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public Animator animator;
    public AudioSource AS;

    public AudioClip[] audioClips;

    public float volume = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if(audioClips != null)
        {
            AS.clip = audioClips[Random.Range(0,audioClips.Length)];
            AS.volume = volume;
            AS.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(animator != null && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) 
        {
            Destroy(gameObject);
        }
    }
}
