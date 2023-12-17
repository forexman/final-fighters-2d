using UnityEngine;

/// <summary>
/// Duplicates a sprite and manages a pool of these duplicates for effects like infinite scrolling.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteDuplicator : MonoBehaviour
{
    [SerializeField] private int poolSize = 5;
    [SerializeField] private int spriteRepositionIndex = 2;
    [SerializeField] private float spriteRepositionCorrection = 0.03f;

    private Transform[] duplicatesPool;
    private float spriteWidth;

    private void Start()
    {
        InitializePool();
    }

    private void Update()
    {
        RepositionSprites();
    }

    /// <summary>
    /// Initializes the pool of sprite duplicates.
    /// </summary>
    private void InitializePool()
    {
        duplicatesPool = new Transform[poolSize];
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        Vector2 startingPos = transform.position;
        duplicatesPool[0] = transform;

        for (int i = 1; i < poolSize; i++)
        {
            Vector2 position = new Vector2(CalculateNewXPosition(startingPos), startingPos.y);
            GameObject duplicate = Instantiate(gameObject, position, Quaternion.identity, transform.parent);
            Destroy(duplicate.GetComponent<SpriteDuplicator>());

            duplicatesPool[i] = duplicate.transform;
            startingPos = position;
        }
    }

    /// <summary>
    /// Repositions the sprites when they move off-screen.
    /// </summary>
    private void RepositionSprites()
    {
        foreach (Transform duplicate in duplicatesPool)
        {
            if (IsSpriteOffScreen(duplicate.position.x))
            {
                Transform rightmostSprite = GetRightMostSprite();
                duplicate.position = CalculateNewPosition(rightmostSprite.position);
            }
        }
    }

    /// <summary>
    /// Calculates the X position for a new or repositioned sprite.
    /// </summary>
    /// <param name="startingPos">The starting position to calculate from.</param>
    /// <returns>The calculated new X position.</returns>
    private float CalculateNewXPosition(Vector3 startingPos)
    {
        return Mathf.FloorToInt(startingPos.x + spriteWidth) - spriteRepositionCorrection * transform.lossyScale.magnitude;
    }

    /// <summary>
    /// Determines if a sprite has moved off-screen.
    /// </summary>
    /// <param name="xPosition">The x position of the sprite.</param>
    /// <returns>True if the sprite is off-screen; otherwise, false.</returns>
    private bool IsSpriteOffScreen(float xPosition)
    {
        return xPosition < -spriteWidth * spriteRepositionIndex;
    }

    /// <summary>
    /// Finds the rightmost sprite in the duplicates pool.
    /// </summary>
    /// <returns>The transform of the rightmost sprite.</returns>
    private Transform GetRightMostSprite()
    {
        float rightmostX = Mathf.NegativeInfinity;
        Transform rightmostSprite = null;

        foreach (Transform duplicate in duplicatesPool)
        {
            if (duplicate.position.x > rightmostX)
            {
                rightmostSprite = duplicate;
                rightmostX = duplicate.position.x;
            }
        }

        return rightmostSprite;
    }

    /// <summary>
    /// Calculates a new position based on the rightmost sprite's position.
    /// </summary>
    /// <param name="basePosition">The position of the rightmost sprite.</param>
    /// <returns>The calculated new position.</returns>
    private Vector2 CalculateNewPosition(Vector2 basePosition)
    {
        return new Vector2(CalculateNewXPosition(basePosition), basePosition.y);
    }
}
