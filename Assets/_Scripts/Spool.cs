using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using TMPro;
using UnityEngine;

public class Spool : MonoBehaviour
{
    public LeanChase leanChase;

    public bool isDocked;
    public bool isDockedOnMain;

    public GameObject levelUpEffect;

    private BreadSpawner breadSpawner;

    public IHolder holder;
    private Vector3 previousPosition;

    public int currentLevel;
    public int currentValue;
    

    public void Start()
    {
        leanChase.enabled = false;
        isDocked = true;
        isDockedOnMain = true;
        currentLevel = 0;
        currentValue = 1;

        breadSpawner = transform.GetChild(0).GetComponent<BreadSpawner>();
    }
    
    public void AllowMove()
    {
        leanChase.enabled = true;
    }

    public void DisableMove()
    {
        leanChase.enabled = false;
    }
    
    public void ResetToOriginalLocation()
    {
        DisableMove();
        holder.AttachSpool(this);
        transform.position = previousPosition;
    }

    public void DetachSpoolFromHolder()
    {
        previousPosition = transform.position;
        
        holder.DetatchSpool(this);

        isDocked = false;
        isDockedOnMain = false;
    }

    public void SpawnBread()
    {
        breadSpawner.SpawnBread();
    }

    public void LevelSpoolUp()
    {
        
        if (currentLevel == 3) return;
        
        transform.GetChild(currentLevel).gameObject.SetActive(false);
        currentLevel++;
        transform.GetChild(currentLevel).gameObject.SetActive(true);
        breadSpawner = transform.GetChild(currentLevel).gameObject.GetComponent<BreadSpawner>();

        Instantiate(levelUpEffect, transform.position + new Vector3(0,.7f,0), Quaternion.identity);
        
        

        currentValue += currentValue;

    }
}
