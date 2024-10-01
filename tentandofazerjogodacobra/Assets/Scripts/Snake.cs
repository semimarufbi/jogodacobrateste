using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private int rows = 30;
    private int cols = 30;
    private int[,] grid;
    public Transform foodPrefab; // Prefab da comida
    private Transform foodInstance; // Instância da comida


    private Vector2Int snakePosition; // Posição atual da cabeça da cobra na matriz
    private List<Vector2Int> bodyPositions = new List<Vector2Int>(); // Posições do corpo da cobra na matriz

    private Vector2Int direction = Vector2Int.right; // Direção inicial da cobra

    public Transform snakeBodyPrefab; // Prefab do corpo da cobra
    private List<Transform> bodyParts = new List<Transform>(); // Lista de objetos dos segmentos do corpo

    public float moveDelay = 0.2f; // Atraso entre os movimentos da cobra
    private float moveTimer;

    private Vector3 gridOffset; // Offset para ajustar a posição visual da grade

    void Start()
    {
        // Inicializar a matriz (grade)
        grid = new int[rows, cols];

        // Definir a posição inicial da cobra
        snakePosition = new Vector2Int(rows / 2, cols / 2);

        // Marcar a posição da cobra na matriz
        grid[snakePosition.x, snakePosition.y] = 1;

        // Inicializar o temporizador de movimento
        moveTimer = moveDelay;

        // Definir o offset para ajustar a visualização da grade
        gridOffset = new Vector3(-cols / 2, -rows / 2, 0);

        // Inicializar a comida na matriz
        SpawnFood();
    }

    void Update()
    {
        // Atualiza o temporizador de movimento
        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0)
        {
            Move();
            moveTimer = moveDelay;
        }

        // Controle de direção (input do jogador)
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2Int.down)
            direction = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S) && direction != Vector2Int.up)
            direction = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A) && direction != Vector2Int.right)
            direction = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) && direction != Vector2Int.left)
            direction = Vector2Int.right;
    }

    void Move()
    {
        // Atualizar a posição da cabeça da cobra
        Vector2Int newPosition = snakePosition + direction;

        // Verificar se a cobra ultrapassou os limites da matriz e fazer o wrap-around
        if (newPosition.x < 0)
            newPosition.x = cols - 1;
        else if (newPosition.x >= cols)
            newPosition.x = 0;

        if (newPosition.y < 0)
            newPosition.y = rows - 1;
        else if (newPosition.y >= rows)
            newPosition.y = 0;

        // Verificar se a nova posição tem comida
        if (grid[newPosition.x, newPosition.y] == 2)
        {
            // Crescer a cobra
            Grow();
            // Randomizar uma nova posição para a comida
            SpawnFood();
        }
        else if (grid[newPosition.x, newPosition.y] == 1)
        {
            // Se a cobra colidir com o próprio corpo (fim de jogo)
            Debug.Log("Game Over");
            return;
        }

        // Atualizar a posição da cobra na matriz
        bodyPositions.Insert(0, snakePosition);
        snakePosition = newPosition;

        // Mover a cabeça da cobra
        grid[snakePosition.x, snakePosition.y] = 1;

        // Atualizar a posição dos segmentos do corpo
        if (bodyPositions.Count > 0)
        {
            // Limpar a última posição do corpo na matriz
            Vector2Int tailPosition = bodyPositions[bodyPositions.Count - 1];
            grid[tailPosition.x, tailPosition.y] = 0;

            // Remover o último segmento
            bodyPositions.RemoveAt(bodyPositions.Count - 1);
        }

        // Atualizar visualmente a posição da cobra e do corpo
        UpdateSnakeVisual();
    }

    // Método para fazer a cobra crescer quando comer a comida
   public void Grow()
    {
        // Adiciona um novo segmento ao corpo da cobra
        Transform newBodyPart = Instantiate(snakeBodyPrefab);
        newBodyPart.position = new Vector3(snakePosition.x, snakePosition.y, 0) + gridOffset;
        bodyParts.Add(newBodyPart);

        // Adicionar uma nova posição ao corpo
        bodyPositions.Add(snakePosition);
    }

    // Método para randomizar a posição da comida na matriz
    void SpawnFood()
    {
        Vector2Int foodPosition;

        // Encontrar uma célula vazia aleatória
        do
        {
            foodPosition = new Vector2Int(Random.Range(0, cols), Random.Range(0, rows));
        } while (grid[foodPosition.x, foodPosition.y] != 0);

        // Marcar a posição da comida na matriz
        grid[foodPosition.x, foodPosition.y] = 2;

        // Atualizar visualmente a comida
        UpdateFoodVisual(foodPosition);
    }

    // Método para atualizar visualmente a posição da cobra
    void UpdateSnakeVisual()
    {
        // Atualizar a posição visual da cabeça da cobra
        transform.position = new Vector3(snakePosition.x, snakePosition.y, 0) + gridOffset;

        // Atualizar a posição visual dos segmentos do corpo
        for (int i = 0; i < bodyParts.Count; i++)
        {
            if (i < bodyPositions.Count)
            {
                bodyParts[i].position = new Vector3(bodyPositions[i].x, bodyPositions[i].y, 0) + gridOffset;
            }
        }
    }

    // Método para atualizar visualmente a comida
    void UpdateFoodVisual(Vector2Int foodPosition)
    {
        // Se a comida ainda não foi instanciada, instanciar a comida
        if (foodInstance == null)
        {
            foodInstance = Instantiate(foodPrefab);
        }

        // Posicionar a comida dentro da grid com o offset aplicado
        foodInstance.position = new Vector3(foodPosition.x, foodPosition.y, 0) + gridOffset;
    }
}
