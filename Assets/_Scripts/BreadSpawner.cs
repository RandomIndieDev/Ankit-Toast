using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BreadSpawner : MonoBehaviour
{

    public GameObject breadGameObject;
    public GameObject breadCompleted;

    public GameObject poofEffect;

    public List<Transform> breadSpawnLocations;

    private Queue<GameObject> spawnedBreads = new Queue<GameObject>();

    public void SpawnBread()
    {
        foreach (var location in breadSpawnLocations)
        {
            var spawnedBread = Instantiate(breadGameObject, location.transform.position, Quaternion.identity);

            spawnedBread.transform.DOMove(spawnedBread.transform.position + new Vector3(0, 2, 0), .5f).onComplete += MergeBread;

            spawnedBreads.Enqueue(spawnedBread);

        }
    }

    private void MergeBread()
    {
        var bread = spawnedBreads.Dequeue();
        
        var completeBread = Instantiate(breadCompleted, bread.transform.position, Quaternion.identity);
        completeBread.transform.DORotate(new Vector3(0, 360, 0), .3f, RotateMode.WorldAxisAdd);
        
        SandwichCustomers.Instance.GiveSandwichToCustomer(completeBread);
        
        Instantiate(poofEffect, bread.transform.position, Quaternion.identity);
        
        Destroy(bread);
        
        
    }
}
