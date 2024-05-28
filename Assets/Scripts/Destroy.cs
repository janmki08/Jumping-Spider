using System.Collections;
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

    private List<GameObject> platforms = new List<GameObject>(); // 플랫폼 리스트
    private List<GameObject> springs = new List<GameObject>(); // 스프링 리스트
    private List<GameObject> rings = new List<GameObject>(); // 링 리스트
    private List<GameObject> enemies = new List<GameObject>(); // 적 리스트

    private int maxPlatforms = 20; // 최대 플랫폼 개수
    private int maxSprings = 3; // 최대 스프링 개수
    private int maxRings = 3; // 최대 링 개수
    private int maxEnemies = 3; // 최대 적 개수

    private float minX = -3.5f; // 최소 X 위치
    private float maxX = 3.5f; // 최대 X 위치
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        cameraPosition.y += destroyerOffsetY;
        cameraPosition.z = transform.position.z;
        transform.position = cameraPosition;
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
        platforms.Remove(platform);
        Destroy(platform);
    }

    private void DestroySpring(GameObject spring)
    {
        springs.Remove(spring);
        Destroy(spring);
    }

    private void DestroyRing(GameObject ring)
    {
        rings.Remove(ring);
        Destroy(ring);
    }

    private void DestroyEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            enemies.Remove(enemy);
            Destroy(enemy);
        }
    }

    private void SpawnPlatform()
    {
        if (platforms.Count >= maxPlatforms) // 최대 개수를 초과하면 새로운 플랫폼을 생성하지 않음
            return;

        float randomY;
        float minY = 1f; // 최소 간격
        float maxY = 3.5f; // 최대 간격

        if (platforms.Count == 0) // 첫 번째 플랫폼인 경우
        {
            randomY = Random.Range(6f, 10f) + player.transform.position.y;
        }
        else // 이전 플랫폼이 있는 경우
        {
            GameObject lastPlatform = platforms[platforms.Count - 1];
            float lastPlatformY = lastPlatform.transform.position.y;
            randomY = Random.Range(lastPlatformY + minY, lastPlatformY + maxY);
        }

        float randomX = Random.Range(minX, maxX);
        Vector2 randomPosition = new Vector2(randomX, randomY);

        currentPlat = (GameObject)Instantiate(platformPrefab, randomPosition, Quaternion.identity);
        currentPlat.tag = "Platform"; // 태그 설정
        platforms.Add(currentPlat); // 플랫폼 리스트에 추가

        SpawnSpring();
        SpawnRing();
        SpawnEnemy();
    }
    private void SpawnSpring()
    {
        if (springs.Count < maxSprings)
        {
            float minY = 3f; // 최소 간격
            float maxY = 8f; // 최대 간격

            float randomY;
            if (springs.Count == 0) // 첫 번째 스프링인 경우
            {
                randomY = Random.Range(5f, 15f) + player.transform.position.y;
            }
            else // 이전 스프링이 있는 경우
            {
                GameObject lastSpring = springs[springs.Count - 1];
                float lastSpringY = lastSpring.transform.position.y;
                randomY = Random.Range(lastSpringY + minY, lastSpringY + maxY);
            }

            float randomX = Random.Range(minX, maxX);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            currentSpring = (GameObject)Instantiate(springPrefab, randomPosition, Quaternion.identity);
            currentSpring.tag = "Spring"; // 태그 설정
            springs.Add(currentSpring); // 스프링 리스트에 추가
        }
    }

    private void SpawnRing()
    {
        if (rings.Count < maxRings)
        {
            float minY = 5f; // 최소 간격
            float maxY = 15f; // 최대 간격

            float randomY;
            if (rings.Count == 0) // 첫 번째 링인 경우
            {
                randomY = Random.Range(12f, 30f) + player.transform.position.y;
            }
            else // 이전 링이 있는 경우
            {
                GameObject lastRing = rings[rings.Count - 1];
                float lastRingY = lastRing.transform.position.y;
                randomY = Random.Range(lastRingY + minY, lastRingY + maxY);
            }

            float randomX = Random.Range(minX, maxX);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            currentRing = (GameObject)Instantiate(ringPrefab, randomPosition, Quaternion.identity);
            currentRing.tag = "Ring"; // 태그 설정
            rings.Add(currentRing); // 링 리스트에 추가
        }
    }

    private void SpawnEnemy()
    {
        if (enemies.Count < maxEnemies)
        {
            float minY = 5f; // 최소 간격
            float maxY = 15f; // 최대 간격

            float randomY;
            if (enemies.Count == 0) // 첫 번째 적인 경우
            {
                randomY = Random.Range(10f, 20f) + player.transform.position.y;
            }
            else // 이전 적이 있는 경우
            {
                if (enemies.Count > 0) // 리스트가 비어 있는지 확인
                {
                    GameObject lastEnemy = enemies[enemies.Count - 1];
                    if (lastEnemy != null) // 마지막 적이 존재하는지 확인
                    {
                        float lastEnemyY = lastEnemy.transform.position.y;
                        randomY = Random.Range(lastEnemyY + minY, lastEnemyY + maxY);
                    }
                    else
                    {
                        randomY = Random.Range(10f, 20f) + player.transform.position.y;
                    }
                }
                else
                {
                    randomY = Random.Range(10f, 20f) + player.transform.position.y;
                }
            }

            float randomX = Random.Range(minX, maxX);
            Vector2 randomPosition = new Vector2(randomX, randomY);

            currentEnemy = (GameObject)Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            currentEnemy.tag = "Enemy"; // 태그 설정
            enemies.Add(currentEnemy); // 적 리스트에 추가
        }
    }
}