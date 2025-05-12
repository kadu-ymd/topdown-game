using UnityEngine;

public class PerspectiveZ : MonoBehaviour
{
    void Start() {
        // Obtém todos os SpriteRenderers na cena sem ordenação desnecessária
        SpriteRenderer[] spriteRenderers = Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);

        foreach (SpriteRenderer sprite in spriteRenderers) {
            Transform spriteTranform = sprite.gameObject.transform;

            // Verifica se há outros SpriteRenderers na cena que possuam maior tamanho
            foreach (SpriteRenderer otherSprite in spriteRenderers) {
                if (sprite == otherSprite) continue; 

                if (IsSmallerAndOverlapping(sprite, otherSprite)) {
                    spriteTranform.position = new Vector3(spriteTranform.position.x, spriteTranform.position.y, otherSprite.transform.position.z - 0.01f);
                    break; 
                }
            }
        }
    }

    bool IsSmallerAndOverlapping(SpriteRenderer sprite, SpriteRenderer otherSprite) {
        Bounds spriteBounds = sprite.bounds;
        Bounds otherSpriteBounds = otherSprite.bounds;

        float spriteArea = spriteBounds.size.x * spriteBounds.size.y;
        float otherSpriteArea = otherSpriteBounds.size.x * otherSpriteBounds.size.y;

        if (spriteArea >= otherSpriteArea)
            return false; // sprite não é menor

        if (!spriteBounds.Intersects(otherSpriteBounds))
            return false; // Não há sobreposição

        // Calcula a área de interseção
        float overlapX = Mathf.Min(spriteBounds.max.x, otherSpriteBounds.max.x) - Mathf.Max(spriteBounds.min.x, otherSpriteBounds.min.x);
        float overlapY = Mathf.Min(spriteBounds.max.y, otherSpriteBounds.max.y) - Mathf.Max(spriteBounds.min.y, otherSpriteBounds.min.y);
        float overlapArea = overlapX * overlapY;


        return (overlapArea / spriteArea) >= 0.9f; // Retorna verdadeiro se a sobreposição for 50% ou mais
    }
}
