using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject player;
    public GameObject platformPrefab;
    public GameObject springPrefab;
    public GameObject ringPrefab;
    public GameObject enemyPrefab;

    private GameObject currentPlat;
    private GameObject currentSpring;
    private GameObject currentRing;
    private GameObject currentEnemy;

    private Camera mainCamera;
    public float destroyerOffsetY = -8.5f;

    private List<GameObject> platforms = new List<GameObject>();
    private List<GameObject> springs = new List<GameObject>();
    private List<GameObject> rings = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>();

    private int maxPlatforms = 20;
    private int maxSprings = 3;
    private int maxRings = 3;
    private int maxEnemies = 3;

    private float minX = -3.5f;
    private float maxX = 3.5f;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateCameraPosition();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            DestroyPlatform(other.gameObject);
            SpawnPlatform();
        }
        else if (other.gameObject.CompareTag("Spring"))
        {
            DestroySpring(other.gameObject);
            SpawnSpring();
        }
        else if (other.gameObject.CompareTag("Ring"))
        {
            DestroyRing(other.gameObject);
            SpawnRing();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            DestroyEnemy(other.gameObject);
            SpawnEnemy();
        }
    }

    private void DestroyPlatform(GameObject platform)
    {
        platforms.RemoveAll(item => item == null);
        platforms.Remove(platform);
        Destroy(platform);
    }

    private void DestroySpring(GameObject spring)
    {
        springs.RemoveAll(item => item == null);
        springs.Remove(spring);
        Destroy(spring);
    }

    private void DestroyRing(GameObject ring)
    {
        rings.RemoveAll(item => item == null);
        rings.Remove(ring);
        Destroy(ring);
    }

    private void DestroyEnemy(GameObject enemy)
    {
        enemies.RemoveAll(item => item == null);
        if (enemy != null)
        {
            enemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    private void SpawnPlatform()
    {
        if (platforms.Count >= maxPlatforms)
            return;

        float randomY = CalculateRandomY(platforms, player.transform.position.y, 6f, 10f, 3f, 4f);
        float randomX = Random.Range(minX, maxX);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        currentPlat = Instantiate(platformPrefab, randomPosition, Quaternion.identity);
        currentPlat.tag = "Platform";
        platforms.Add(currentPlat);

        SpawnSpring();
        SpawnRing();
        SpawnEnemy();
    }

    private void SpawnSpring()
    {
        if (springs.Count < maxSprings)
        {
            float randomY = CalculateRandomY(springs, player.transform.position.y, 5f, 15f, 3f, 8f);
            float randomX = Random.Range(minX, maxX);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            currentSpring = Instantiate(springPrefab, randomPosition, Quaternion.identity);
            currentSpring.tag = "Spring";
            springs.Add(currentSpring);
        }
    }

    private void SpawnRing()
    {
        if (rings.Count < maxRings)
        {
            float randomY = CalculateRandomY(rings, player.transform.position.y, 12f, 30f, 5f, 15f);
            float randomX = Random.Range(minX, maxX);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            currentRing = Instantiate(ringPrefab, randomPosition, Quaternion.identity);
            currentRing.tag = "Ring";
            rings.Add(currentRing);
        }
    }

    private void SpawnEnemy()
    {
        enemies.RemoveAll(item => item == null);

        if (enemies.Count < maxEnemies)
        {
            float randomY = CalculateRandomY(enemies, player.transform.position.y, 10f, 20f, 5f, 15f);
            float randomX = CalculateRandomX();
            Vector2 randomPosition = new Vector2(randomX, randomY);

            currentEnemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            currentEnemy.tag = "Enemy";
            enemies.Add(currentEnemy);
        }
    }

    private float CalculateRandomY(List<GameObject> objects, float playerY, float minRange, float maxRange, float minGap, float maxGap)
    {
        float randomY;

        if (objects.Count == 0)
            randomY = Random.Range(minRange, maxRange) + playerY;
        else
        {
            GameObject lastObject = objects[objects.Count - 1];
            float lastObjectY = lastObject.transform.position.y;
            randomY = Random.Range(lastObjectY + minGap, lastObjectY + maxGap);
        }

        return randomY;
    }

    private float CalculateRandomX()
    {
        int randomSide = Random.Range(0, 2);
        float randomX = randomSide == 0 ? minX - 1f : maxX + 1f;
        return randomX;
    }

    private void UpdateCameraPosition()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        cameraPosition.y += destroyerOffsetY;
        cameraPosition.z = transform.position.z;
        transform.position = cameraPosition;
    }
}