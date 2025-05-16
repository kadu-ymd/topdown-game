using UnityEngine;

public class InteractableMessage : InteractableManager {
    public string onIteractionText = "Não há nada aqui, preciso continuar procurando...";

    public override void Interact() {
        base.Interact();
        ThoughtManager.ShowThought(onIteractionText);
    }
}
