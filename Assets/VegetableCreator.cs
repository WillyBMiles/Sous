using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class VegetableCreator : MonoBehaviour
{
    [SerializeField]
    List<Rigidbody> vegetables = new();

    [SerializeField]
    Vector3 launchForce;
    [SerializeField]
    float maxSpawnVolume;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Check());
    }


    IEnumerator Check()
    {
        yield return new WaitUntil(() => Menus.hasGameStarted);

        while (true)
        {
            yield return new WaitForSeconds(2f);
            float totalVolume = 0f;
            foreach (Sliceable sliceable in Sliceable.sliceables)
            {
                totalVolume += sliceable.volume;

            }
            if (totalVolume < maxSpawnVolume)
            {
                SpawnNewVegetable();
            }
        }
    }

    void SpawnNewVegetable()
    {
        Rigidbody rbPrefab = vegetables[Random.Range(0, vegetables.Count)];
        Rigidbody spawnedVegetable = Instantiate(rbPrefab, transform.position, transform.rotation);
        spawnedVegetable.AddForce(launchForce, ForceMode.Impulse);
    }


}
