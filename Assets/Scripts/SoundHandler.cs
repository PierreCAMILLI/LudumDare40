using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class SoundHandler : MonoBehaviour {
    Animator animator;
    AudioSource audioSource;

    public AudioClip clipWalking;
    public AudioClip clipAttack;
    public AudioClip clipHurt;
    public AudioClip clipStun;

    int walkHash = Animator.StringToHash("walking");
    int attackHash = Animator.StringToHash("attack");
    int hurtHash = Animator.StringToHash("damaged");
    int stunHash = Animator.StringToHash("stunned");

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
	}
	
    bool walkingBefore = false;
    bool stunBefore = false;

    [SerializeField]
    bool attackNow = false;
    [SerializeField]
    bool hurtNow = false;

    bool triggered = false;
	// Update is called once per frame
	void LateUpdate () {

        bool walkingNow = animator.GetBool(walkHash);
        bool stunNow = animator.GetBool(stunHash);

        
        triggered = triggered & audioSource.isPlaying;
        if (triggered) {
        }
        else
        {
            if (hurtNow)
            {
                audioSource.clip = clipHurt;
                audioSource.Play(0);
                audioSource.loop = false;
                triggered = true;
            }
            else if (animator.GetBool(stunHash) && !stunBefore)
            {
                audioSource.clip = clipStun;
                audioSource.Play(0);
                audioSource.loop = false;
                stunBefore = true;
                triggered = true;
            }
            else if (!stunNow && stunBefore)
            {
                stunBefore = false;
            }
            else if (attackNow)
            {
                audioSource.clip = clipAttack;
                audioSource.Play(0);
                audioSource.loop = false;
                stunBefore = true;
                triggered = true;
            }
            else if (!walkingBefore && walkingNow)
            {
                audioSource.clip = clipWalking;
                audioSource.Play(0);
                audioSource.loop = true;
                walkingBefore = true;
            }
            else if (!walkingNow && walkingBefore)
            {
                audioSource.Pause();
                walkingBefore = false;
            }
        }
        attackNow = false;
        hurtNow = false;
    }

    public void TriggerAttack()
    {
        attackNow = true;
    }
    public void TriggerHurt()
    {
        attackNow = true;
    }
}
