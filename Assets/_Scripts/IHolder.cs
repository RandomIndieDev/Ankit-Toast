using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHolder
{
    void DetatchSpool(Spool spool);
    void AttachSpool(Spool spool);

    bool CanPlaceSpool(int spoolLevel);

    void PrintName();

}
