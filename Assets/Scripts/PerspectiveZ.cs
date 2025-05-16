using UnityEngine;

public class PerspectiveZ : MonoBehaviour
{
    void Start()
    {
        // Obtém todos os SpriteRenderers na cena sem ordenação desnecessária
        SpriteRenderer[] spriteRenderers = Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);

        foreach (SpriteRenderer sprite in spriteRenderers) {

            Transform spriteTranform = sprite.gameObject.transform;
            spriteTranform.position = new Vector3(spriteTranform.position.x, spriteTranform.position.y, spriteTranform.position.y);

            // Verifica se há outros SpriteRenderers na cena que possuam maior tamanho
            foreach (SpriteRenderer otherSprite in spriteRenderers) {
                if (sprite == otherSprite) continue;

                if (IsSmallerAndOverlapping(sprite, otherSprite)) {
                    spriteTranform.position = new Vector3(spriteTranform.position.x, spriteTranform.position.y, otherSprite.transform.position.y - 0.1f);
                }
            }
        }
    }

    bool IsSmallerAndOverlapping(SpriteRenderer sprite, SpriteRenderer otherSprite) {
        Bounds spriteBounds = sprite.bounds;
        Bounds otherSpriteBounds = otherSprite.bounds;

        float spriteArea = spriteBounds.size.x * spriteBounds.size.y;
        float otherSpriteArea = otherSpriteBounds.size.x * otherSpriteBounds.size.y;

        // Verifica se as áreas são válidas para evitar divisão por zero
        if (spriteArea <= 0 || otherSpriteArea <= 0) {
            return false;
        }

        // Calcula a área de interseção
        float overlapX = Mathf.Max(0, Mathf.Min(spriteBounds.max.x, otherSpriteBounds.max.x) - Mathf.Max(spriteBounds.min.x, otherSpriteBounds.min.x));
        float overlapY = Mathf.Max(0, Mathf.Min(spriteBounds.max.y, otherSpriteBounds.max.y) - Mathf.Max(spriteBounds.min.y, otherSpriteBounds.min.y));
        float overlapArea = overlapX * overlapY;

        // Verifica se o primeiro sprite é menor e se há sobreposição
        return spriteArea < otherSpriteArea && (overlapArea/spriteArea) > 0.6f;
    }
}
