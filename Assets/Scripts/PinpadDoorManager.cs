using UnityEngine;
using System.Collections;

public class PinpadDoorManager : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D spriteCollider;
    private AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;
    public string lockedText = "A porta está trancada, preciso de uma senha de três dígitos para abri-la...";
    public GameObject pinpadCanvas;
    private Pinpad pinpad;
    private bool isOpen = false;
    private bool isLocked = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spriteCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        pinpad = pinpadCanvas.GetComponent<Pinpad>();

        animator.SetBool("isLocked", true);
    }

    void Update()
    {
        isLocked = !pinpad.unlocked;
        animator.SetBool("isLocked", isLocked);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void Interact()
    {

        if (isLocked)
        {
            ThoughtManager.ShowThought(lockedText);
            StartCoroutine(OpenPinpad());
            animator.ResetTrigger("Interact");
        }
        else
        {
            AnimatorStateInfo currentAnimState = animator.GetCurrentAnimatorStateInfo(0);
            if (!isOpen & currentAnimState.IsName("Idle"))
            {
                spriteCollider.enabled = false;
                isOpen = true;
                PlaySound(openSound);
            }
            else if (isOpen & currentAnimState.IsName("OpenDoor"))
            {
                spriteCollider.enabled = true;
                isOpen = false;
                PlaySound(closeSound);
            }
        }
    }

    IEnumerator OpenPinpad()
    {
        while (PlayerPrefs.GetString("CurrentUI") == "Thought")
            yield return new WaitForEndOfFrame();
        pinpad.DisplayPinpad();
    }
    
    public void PlayLockedSound()
    {
        PlaySound(closeSound);
    }
}
