using UnityEngine;

public class DoorManager : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D spriteCollider;
    public bool isLocked;
    public bool isOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.animator = GetComponent<Animator>();
        this.spriteCollider = GetComponent<Collider2D>();

        if (this.isLocked) this.isOpen = false;
    }

    // Update is called once per frame
    void Update() {
        this.animator.SetBool("isOpen", this.isOpen);
        this.animator.SetBool("isLocked", this.isLocked);
        this.spriteCollider.enabled = !this.isOpen;
    }

    public void Interact() {
        if (!this.isLocked) {
            this.isOpen = !this.isOpen;
        }
    }
}
