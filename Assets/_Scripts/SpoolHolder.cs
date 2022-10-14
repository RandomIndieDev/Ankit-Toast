using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpoolHolder : MonoBehaviour, IHolder
{

    public Transform spoolStartPlaceLocation;
    public float breadSpawnTime;
    
    public Spool currentlyHeldSpool;
    private int totalLevels;
    private bool isUsed;

    [Header("Other Locations")] 
    public SpoolHolder top;
    public SpoolHolder bottom;
    public SpoolHolder left;
    public SpoolHolder right;

    public Vector3 placementOffset;

    private List<SpoolHolder> activated = new List<SpoolHolder>();

    private float breadSpawnTick;
    
    public static event Action OnSpawnBreadClicked;

    public static void SpawnBreadClicked()
    {
        OnSpawnBreadClicked?.Invoke();
    }

    void OnEnable()
    {
        OnSpawnBreadClicked += SpawnBreadMaybe;
    }

    void OnDisable()
    {
        OnSpawnBreadClicked -= SpawnBreadMaybe;
    }

    public void Update()
    {
        if (CheckBreadSpawnTime())
        {
            SpawnBread();
        }
    }
    
    public void SpawnBreadMaybe()
    {
        if (Random.value <= .5f)
        {
            SpawnBread();
        }
    }


    private bool CheckBreadSpawnTime()
    {
        if (!isUsed) return false;
        if (currentlyHeldSpool == null) return false;
        
        breadSpawnTick += Time.deltaTime;

        if (!(breadSpawnTick >= breadSpawnTime)) return false;
        
        breadSpawnTick = 0f;
        return true;

    }

    private void SpawnBread()
    {
        if (currentlyHeldSpool == null) return;
        currentlyHeldSpool.SpawnBread();
    }
    
    
    public void DetatchSpool(Spool spool)
    {
        currentlyHeldSpool = null;
        isUsed = false;
        placementOffset = Vector3.zero;

        foreach (var spoolll in activated)
        {
            spoolll.isUsed = false;
        }
        
        activated = new List<SpoolHolder>();
    }

    public void AttachSpool(Spool spool)
    {
        spool.isDocked = true;
        spool.holder = this;

        isUsed = true;

        spool.transform.parent = transform;
        
        totalLevels += spool.currentLevel;
        var location = spoolStartPlaceLocation.transform.localPosition + placementOffset;

        spool.transform.DOLocalMove(location, .3f);

        currentlyHeldSpool = spool;
    }

    public bool CanPlaceSpool(int level)
    {
        var hasSpace = false;

        if (level == 0 || level == 1)
        {
            hasSpace = true;
        }

        if (level == 2)
        {
            if (left != null)
            {
                if (!left.isUsed)
                {
                    hasSpace = true;
                    left.isUsed = true;
                    activated.Add(left);
                    placementOffset = new Vector3(-0.5f,0,0);
                }
            }

            if (right != null && !hasSpace)
            {
                if (!right.isUsed)
                {
                    hasSpace = true;
                    right.isUsed = true;
                    activated.Add(right);
                    placementOffset = new Vector3(.5f, 0, 0);
                }
            }
            
        }

        if (level == 3)
        {
            if (left != null)
            {
                if (!left.isUsed)
                {
                    if (right != null)
                    {
                        if (!right.isUsed)
                        {
                            hasSpace = true;

                            left.isUsed = true;
                            right.isUsed = true;
                            activated.Add(left);
                            activated.Add(right);
                        }
                    }
                }

            }


        }
        
        
        print(level);
        print("Has Space " + hasSpace);
        
        
        return !isUsed && hasSpace;
    }

    public void PrintName()
    {
        print("SPOOL HOLDER");
    }
}
