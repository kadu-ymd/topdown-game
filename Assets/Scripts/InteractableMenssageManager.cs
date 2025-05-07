using UnityEngine;

public class InteractableMenssageManager : MonoBehaviour {
    private SpriteRenderer parentSpriteRenderer;
    private Animator parentAnimator;
    private Material originalMaterial;
    private Material OutineMaterial;
    public string onIteractionText = "Não há nada aqui, preciso continuar procurando...";
    private bool playerInRange = false;

    void Start() {
        this.parentSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        this.parentAnimator = transform.parent.GetComponent<Animator>();
        this.originalMaterial = parentSpriteRenderer.material;
        this.OutineMaterial = Resources.Load<Material>("OutlineMaterialShader");
    }

    void Update() {
        if (this.playerInRange && Input.GetKeyDown(KeyCode.E)) {
            this.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D gameObject) {
        if (gameObject.CompareTag("Player")) {
            this.playerInRange = true;
            this.parentSpriteRenderer.material = this.OutineMaterial;
        }
    }

    private void OnTriggerExit2D(Collider2D gameObject) {
        if (gameObject.CompareTag("Player")) {
            this.playerInRange = false;
            this.parentSpriteRenderer.material = this.originalMaterial;
        }
    }

    public void Interact() {
        if (parentAnimator != null) {
            this.parentAnimator.SetTrigger("Interact");
        }
        ThoughtManager.ShowThought(onIteractionText);
    }
}
