using System.Collections;
using UnityEngine;

public class HealthpackSpawner : MonoBehaviour
{
    [SerializeField] private float TimeBetweenSpawns = 1f;
    [SerializeField] private float TimeDeviationBetweenSpawns = 0.5f;

    [SerializeField] GameObject Healthpack;

    void Start()
    {
        StartCoroutine(SpawnHealthpacks());
    }

    private IEnumerator SpawnHealthpacks()
    {
        while(true)
        {
            yield return new WaitForSeconds(TimeBetweenSpawns + Random.Range(-1f, 1f) * TimeDeviationBetweenSpawns);

            float RandX = Random.Range(-8.9f, 8.9f);
            float RandY = Random.Range(-5f, 5f);
            Instantiate(Healthpack, new Vector2(RandX, RandY), Quaternion.identity);
        }
    }
}
