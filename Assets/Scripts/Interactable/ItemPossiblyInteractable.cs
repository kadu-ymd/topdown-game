using UnityEngine;
using TMPro;

public class ItemPossiblyInteractable : ItemInteractable {

    public bool haveItem = false;
    public string withoutItemIteractionText = "Não há nada aqui, preciso continuar procurando...";

    protected void Interact()
    {
        if (haveItem)
            base.Interact();
        else
            ThoughtManager.ShowThought(withoutItemIteractionText);
    }
}
