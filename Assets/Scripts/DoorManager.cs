using UnityEngine;

public class DoorManager : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D spriteCollider;
    private bool isOpen = false;
    public bool isLocked;

    private AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.spriteCollider = GetComponent<Collider2D>();
        this.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
    }

    private void PlaySound(AudioClip clip) {
        if (clip != null && this.audioSource != null) {
            this.audioSource.PlayOneShot(clip);
        }
    }

    public void Interact() {
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
        this.animator.SetBool("isLocked", this.isLocked);
    }
}
