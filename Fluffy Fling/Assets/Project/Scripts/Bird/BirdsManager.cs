using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] birdPrefabs;
    [SerializeField] private List<GameObject> spawnedBirds;
    [SerializeField] private int amount;
    [SerializeField] private float distanceBetweenBirds = 2f;
    [SerializeField] private Vector3 spawnOffset;
    [SerializeField] private float gizmosSphere = 0.2f;

    public List<GameObject> SpawnedBirds { get => spawnedBirds; set => spawnedBirds = value; }

    private void Start()
    {
        SpawnBirds();
    }

    private void SpawnBirds()
    {
        if (birdPrefabs.Length <= 0) return;
        if (amount > birdPrefabs.Length)
        {
            int j = 0;
            for (int i = 0; i < amount; i++)
            {
                GameObject birdClone = Instantiate(birdPrefabs[j], null);
                birdClone.transform.position = Vector3.left * distanceBetweenBirds * (i + 1) + spawnOffset;
                SpawnedBirds.Add(birdClone);
                j++;

                if (j >= birdPrefabs.Length)
                    j = 0;
            }
        }
        else
        {
            for (int i = 0; i < birdPrefabs.Length; i++)
            {
                GameObject birdClone = Instantiate(birdPrefabs[i], null);
                birdClone.transform.position = Vector3.left * distanceBetweenBirds * (i + 1) + spawnOffset;
                SpawnedBirds.Add(birdClone);
            }
        }

        foreach (GameObject bird in spawnedBirds)
        {
            bird.GetComponent<Bird>().Parent = InputManager.instance.GetComponent<Slingshot>();
        }
    }

    public GameObject GetFirstBird()
    {
        return spawnedBirds[0];
    }

    public void RemoveBird(GameObject bird)
    {
        spawnedBirds.Remove(bird);
        for (int i = 0; i < spawnedBirds.Count; i++)
        {
            spawnedBirds[i].transform.position = Vector3.left * distanceBetweenBirds * (i + 1) + spawnOffset;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if (birdPrefabs.Length <= 0) return;
        if (amount > birdPrefabs.Length)
        {
            int j = 0;
            for (int i = 0; i < amount; i++)
            {
                Vector3 pos = Vector3.left * distanceBetweenBirds * (i + 1) + spawnOffset;
                Gizmos.DrawSphere(pos, gizmosSphere);
                j++;

                if (j > birdPrefabs.Length)
                    j = 0;
            }
        }
        else
        {
            for (int i = 0; i < birdPrefabs.Length; i++)
            {
                Vector3 pos = Vector3.left * distanceBetweenBirds * (i + 1) + spawnOffset;
                Gizmos.DrawSphere(pos, gizmosSphere);
            }
        }
    }
}
