using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject attrarctiveObj;
    public GameObject repulsiveObj;

    public int attractiveObjCount;
    public int repulsiveObjCount;

    public bool HasAttractive()
    {
        return this.attractiveObjCount > 0;
    }
    public bool HasRepulsive()
    {
        return this.repulsiveObjCount > 0;
    }

    public GameObject DropAttractive()
    {
        this.attractiveObjCount = Mathf.Max(0, this.attractiveObjCount - 1);
        return attrarctiveObj;
    }
    public GameObject DropRepulsive()
    {
        this.repulsiveObjCount = Mathf.Max(0, this.repulsiveObjCount - 1);
        return repulsiveObj;
    }

    public void AddAttractive()
    {
        this.attractiveObjCount++;
    }
    public void AddAttractive(int count)
    {
        this.attractiveObjCount += count;
    }
    public void AddRepulsive()
    {
        this.repulsiveObjCount++;
    }
    public void AddRepulsive(int count)
    {
        this.repulsiveObjCount += count;
    }
}
