using UnityEngine;

public class InteractableWith : InteractableManager {

    public MonoBehaviour parentManager;

    public override void Interact() {
        base.Interact();
        if (parentManager != null) {
            parentManager.Invoke("Interact", 0f);
            
        }
    }
}
