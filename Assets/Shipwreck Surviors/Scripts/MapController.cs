using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public Transform player;
    public int chunkSize = 10;
    public GameObject chunkPrefab;

    private Vector2Int lastLoadedChunk;
    private Dictionary<Vector2Int, GameObject> chunks = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        // Initial loading of chunks
        UpdateChunks();
    }

    void Update()
    {
        // Check if the player has moved to a new chunk
        Vector2Int currentChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.y / chunkSize)
        );

        if (currentChunk != lastLoadedChunk)
        {
            // Update chunks if the player has moved to a new chunk
            UpdateChunks();
        }
    }

    void UpdateChunks()
    {
        // Calculate the bounds of chunks to load around the player
        int loadRadius = 1;
        for (int xOffset = -loadRadius; xOffset <= loadRadius; xOffset++)
        {
            for (int yOffset = -loadRadius; yOffset <= loadRadius; yOffset++)
            {
                Vector2Int chunkCoord = new Vector2Int(
                    lastLoadedChunk.x + xOffset,
                    lastLoadedChunk.y + yOffset
                );

                LoadChunk(chunkCoord);
            }
        }

        lastLoadedChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.y / chunkSize)
        );

        // Unload chunks that are too far from the player
        UnloadDistantChunks();
    }

    void LoadChunk(Vector2Int chunkCoord)
    {
        // Check if the chunk is not already loaded
        if (!IsChunkLoaded(chunkCoord))
        {
            // Instantiate a new chunk at the specified coordinates
            GameObject newChunk = Instantiate(chunkPrefab, new Vector3(chunkCoord.x * chunkSize, chunkCoord.y * chunkSize, 0f), Quaternion.identity);

            // Optionally, you can add terrain generation logic here if it's not handled by the chunkPrefab

            // Add the chunk to the dictionary
            chunks.Add(chunkCoord, newChunk);
        }
    }

    void UnloadDistantChunks()
    {
        // Unload chunks that are too far from the player
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunkCoord in chunks.Keys)
        {
            if (Mathf.Abs(chunkCoord.x - lastLoadedChunk.x) > 1 || Mathf.Abs(chunkCoord.y - lastLoadedChunk.y) > 1)
            {
                UnloadChunk(chunkCoord);
                chunksToRemove.Add(chunkCoord);
            }
        }

        // Remove unloaded chunks from the dictionary
        foreach (var chunkCoord in chunksToRemove)
        {
            chunks.Remove(chunkCoord);
        }
    }

    bool IsChunkLoaded(Vector2Int chunkCoord)
    {
        return chunks.ContainsKey(chunkCoord);
    }

    void UnloadChunk(Vector2Int chunkCoord)
    {
        // Destroy the chunk game object
        Destroy(chunks[chunkCoord]);

        // Optionally, perform any cleanup or save data associated with the chunk before destroying
    }
}
