using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Animator animator;

    public Transform startLoc;
    public SandwichCustomers sandwichCustomers;

    public GameObject smileEffect;

    public int neededSandwiches;
    
    public List<GameObject> sandwiches = new List<GameObject>();
    
    private bool checkedDone;

    public bool GiveSandwich(GameObject sandwich)
    {
        sandwiches.Add(sandwich);
        neededSandwiches -= 1;

        return neededSandwiches > 0;
    }

    public void ReduceAmt()
    {
        
    }

    public void CheckIfDone()
    {
        if (checkedDone) return;
        
        if (neededSandwiches <= 0)
        {
            checkedDone = true;
            foreach (var bread in sandwiches)
            {
                Destroy(bread);
            }
            animator.SetTrigger("Walk");
            transform.DORotate(new Vector3(0, 180, 0), .3f, RotateMode.WorldAxisAdd);
            transform.DOMove(startLoc.transform.position, 2f).onComplete += MoveToLocation;
        }
    }

    public void MoveToLocation()
    {
        transform.DORotate(new Vector3(0,0,0), .3f);
        transform.DOMove(transform.position + new Vector3(0,0,-8), 2f).SetEase(Ease.Linear).onComplete += PlayIdleAnimation;
    }

    public void PlayIdleAnimation()
    {
        animator.SetTrigger("Idle");
        sandwichCustomers.AddCustomerToQueue(this);
        neededSandwiches = Random.Range(6, 15);
        checkedDone = false;
    }

    public void PlaySmileEffect()
    {
        Instantiate(smileEffect, transform.position + new Vector3(0, 4.5f, 0), Quaternion.identity);
    }
}
