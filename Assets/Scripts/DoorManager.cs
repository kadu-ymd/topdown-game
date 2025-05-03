using UnityEngine;

public class DoorManager : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D spriteCollider;
    private bool isOpen = false;
    public bool isLocked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.spriteCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update() {
    }

    public void Interact() {
        if (!this.isLocked) {
            AnimatorStateInfo currentAnimState = this.animator.GetCurrentAnimatorStateInfo(0);
            if (!this.isOpen & currentAnimState.IsName("Idle")) {
                this.spriteCollider.enabled = false;
                this.isOpen = true;
            } else if (this.isOpen & currentAnimState.IsName("OpenDoor")) {
                this.spriteCollider.enabled = true;
                this.isOpen = false;
            }
        }
        this.animator.SetBool("isLocked", this.isLocked);
    }
}
