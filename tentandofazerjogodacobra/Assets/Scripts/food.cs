using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea; // A �rea de jogo onde a comida pode aparecer

    // Gera a comida em uma nova posi��o dentro da �rea de jogo
    private void Start()
    {
        RandomizePosition();
    }

    // M�todo para randomizar a posi��o da comida dentro da �rea de jogo
    public void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;

        // Gera uma posi��o aleat�ria dentro da �rea de jogo
        float x = Mathf.Floor(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Floor(Random.Range(bounds.min.y, bounds.max.y));

        // Atualiza a posi��o da comida
        transform.position = new Vector2(x, y);
    }

    // Quando colidir com a cobra (usaremos a tag "Snake" para identificar)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Snake")
        {
            // Randomiza nova posi��o quando for comida
            RandomizePosition();
            // Faz a cobra crescer (encontra o script Snake e chama o m�todo Grow)
            other.GetComponent<Snake>().Grow();
        }
    }
}