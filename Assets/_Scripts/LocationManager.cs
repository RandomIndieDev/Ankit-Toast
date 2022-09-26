using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
   public List<ItemLocation> itemLocations;

   public List<Material> spoolMats;

   public Spool basicSpool;

   
   [ContextMenu("TEST")]
   public void SpawnItemAtFreeLocation()
   {
      foreach (var item in itemLocations)
      {
         if (item.isUsed) continue;
         
         var newSpool = Instantiate(basicSpool.gameObject, transform.position, Quaternion.identity).GetComponent<Spool>();

         item.AttachSpool(newSpool);
         
         return;
      }
   }
}
