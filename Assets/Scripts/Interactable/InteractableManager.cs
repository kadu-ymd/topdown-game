using UnityEngine;

public class InteractableManager : MonoBehaviour {
    private SpriteRenderer parentSpriteRenderer;
    private Animator parentAnimator;
    private Material originalMaterial;
    private Material OutineMaterial;

    void Start() {
        this.parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        this.parentAnimator = transform.parent.GetComponent<Animator>();
        this.originalMaterial = parentSpriteRenderer.material;
        this.OutineMaterial = Resources.Load<Material>("OutlineMaterialShader");
    }

    private void OnTriggerEnter2D(Collider2D gameObject) {
        if (gameObject.CompareTag("Player")) {
            this.parentSpriteRenderer.material = this.OutineMaterial;
        }
    }

    private void OnTriggerExit2D(Collider2D gameObject) {
        if (gameObject.CompareTag("Player")) {
            this.parentSpriteRenderer.material = this.originalMaterial;
        }
    }

    public virtual void Interact() {
        if (parentAnimator != null) {
            this.parentAnimator.SetTrigger("Interact");
        }
    }
}
