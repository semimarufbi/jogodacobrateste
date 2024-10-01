using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea; // A área de jogo onde a comida pode aparecer

    // Gera a comida em uma nova posição dentro da área de jogo
    private void Start()
    {
        RandomizePosition();
    }

    // Método para randomizar a posição da comida dentro da área de jogo
    public void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;

        // Gera uma posição aleatória dentro da área de jogo
        float x = Mathf.Floor(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Floor(Random.Range(bounds.min.y, bounds.max.y));

        // Atualiza a posição da comida
        transform.position = new Vector2(x, y);
    }

    // Quando colidir com a cobra (usaremos a tag "Snake" para identificar)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Snake")
        {
            // Randomiza nova posição quando for comida
            RandomizePosition();
            // Faz a cobra crescer (encontra o script Snake e chama o método Grow)
            other.GetComponent<Snake>().Grow();
        }
    }
}