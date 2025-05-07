using UnityEngine;

public class DoorManager : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D spriteCollider;
    private AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound; 
    private bool isOpen = false;
    public bool isLocked;
    public string unlockerItemName;
    public int unlockWithOpenAttempts;
    private int openAttempts = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.spriteCollider = GetComponent<Collider2D>();
        this.audioSource = GetComponent<AudioSource>();

        if (unlockWithOpenAttempts > 0 && !string.IsNullOrEmpty(unlockerItemName)) {
            Debug.LogWarning("You cannot use both unlockWithOpenAttempts and unlockerItemName at the same time. Please choose one.");
        }
        if (unlockWithOpenAttempts > 0 || !string.IsNullOrEmpty(unlockerItemName)) {
            isLocked = true;
        }
        this.animator.SetBool("isLocked", this.isLocked);
    }

    private void PlaySound(AudioClip clip) {
        if (clip != null && this.audioSource != null) {
            this.audioSource.PlayOneShot(clip);
        }
    }

    public void Interact() {
        if (this.isLocked) {
            if (unlockWithOpenAttempts > 0) {
                openAttempts++;
                if (openAttempts >= unlockWithOpenAttempts) {
                    this.isLocked = false;
                    this.animator.SetBool("isLocked", false);
                }
            } else if (!string.IsNullOrEmpty(unlockerItemName)) {
                if (PlayerPrefs.GetInt(unlockerItemName) == 1) {
                    this.isLocked = false;
                    this.animator.SetBool("isLocked", false);
                } else {
                    Debug.LogWarning("You need to collect the " + unlockerItemName + " item to unlock the door.");
                }
            }
        }

        if (!this.isLocked) {
            AnimatorStateInfo currentAnimState = this.animator.GetCurrentAnimatorStateInfo(0);
            if (!this.isOpen & currentAnimState.IsName("Idle")) {
                this.spriteCollider.enabled = false;
                this.isOpen = true;
                PlaySound(openSound);
            } else if (this.isOpen & currentAnimState.IsName("OpenDoor")) {
                this.spriteCollider.enabled = true;
                this.isOpen = false;
                PlaySound(closeSound);
            }
        }
    }
}
