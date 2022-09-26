using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask placementLayers;
    public LayerMask spoolLayers;
    public Camera camera;

    public Spool currentHeldSpool;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootRayForSpool();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentHeldSpool == null) return;
            
            if (!ShootRayForSpoolUpgrade())   
                ShootRayForPlacement();

            
        }
    }

    private void ShootRayForSpool()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100,spoolLayers))
        {
            currentHeldSpool = hit.transform.gameObject.GetComponent<Spool>();
            
            currentHeldSpool.DetachSpoolFromHolder();
            currentHeldSpool.AllowMove();
        }
    }

    private bool ShootRayForSpoolUpgrade()
    {

        RaycastHit[] hits;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(ray, 10, spoolLayers);

        if (hits.Length <= 1 || hits.Length >= 3) return false;

        var otherSpool = hits[hits.Length -1].transform.GetComponent<Spool>();

        if (!otherSpool.isDockedOnMain) return false;
        
        if (otherSpool.currentLevel != currentHeldSpool.currentLevel) return false;

        Destroy(currentHeldSpool.gameObject);
        
        otherSpool.LevelSpoolUp();
        return true;

    }

    private void ShootRayForPlacement()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, 100 ,placementLayers))
        {
            var placementLocation = hit.transform.gameObject.GetComponent<IHolder>();

            if (!placementLocation.CanPlaceSpool(currentHeldSpool.currentLevel))
            {
                currentHeldSpool.ResetToOriginalLocation();
                return;
            }
            
            currentHeldSpool.DisableMove();
            placementLocation.AttachSpool(currentHeldSpool);

            currentHeldSpool = null;

        }
        else
        {
            currentHeldSpool.ResetToOriginalLocation();
        }
        
        
    }

    private void MouseUp()
    {
        if (currentHeldSpool != null)
            currentHeldSpool.ResetToOriginalLocation();
    }

}
