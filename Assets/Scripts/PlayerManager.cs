using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    public SpriteRenderer EkeySpriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        this.EkeySpriteRenderer = transform.Find("E key").GetComponent<SpriteRenderer>();
        this.EkeySpriteRenderer.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D gameObject) {
        if (gameObject.CompareTag("Interactable")) {
            this.EkeySpriteRenderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D gameObject) {
        if (gameObject.CompareTag("Interactable")) {
            this.EkeySpriteRenderer.enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D gameObject) {
        if (gameObject.gameObject.CompareTag("Enemy")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
