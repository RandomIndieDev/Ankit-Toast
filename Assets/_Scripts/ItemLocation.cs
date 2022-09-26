using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemLocation : MonoBehaviour, IHolder
{
    public bool isUsed;
    public Spool connectedSpool;

    public void DetatchSpool(Spool spool)
    {
        isUsed = false;
        connectedSpool = null;

        spool.isDocked = false;
        spool.isDockedOnMain = false;
    }

    public void AttachSpool(Spool spool)
    {
        isUsed = true;
        connectedSpool = spool;

        connectedSpool.holder = this;
        connectedSpool.isDocked = true;
        connectedSpool.isDockedOnMain = true;

        connectedSpool.transform.parent = transform;
        connectedSpool.leanChase.enabled = false;


        connectedSpool.transform.DOLocalMove(Vector3.zero, 0.3f);

    }

    public bool CanPlaceSpool(int level)
    {
        return !isUsed;
    }

    public void PrintName()
    {
        print("ITEM HOLDER");
    }
}
