using System.Collections.Generic;
using UnityEngine;
using static GenerateDungeon3;
using static UnityEngine.UIElements.UxmlAttributeDescription;

[CreateAssetMenu(fileName = "RulesSO", menuName = "Scriptable Objects/RulesSO")]
public class RulesSO : ScriptableObject
{
    [SerializeField] protected bool _valueBoolean;
    [SerializeField] protected int _valueInt;

    [SerializeField] public List<Vector3> _neighborDownList;
    [SerializeField] public List<Vector3> _neighborCenterList;
    [SerializeField] public List<Vector3> _neighborUpList;

    protected List<Vector3> _neighborList;



    public List<Vector3> NeighBorList
    {
        get { return _neighborList; }
    }

    protected void Reset()
    {
        _valueBoolean = true;
        _valueInt = 4;

        _neighborList.Clear();
        ShowPreList();
        
    }

    public virtual bool ConditionNeighborStates(Cell neighbor)
    {
        return true;
    }

    public virtual bool ConditionNeighborNumber(int numberGoodStates)
    {
        return true;
    }

    public virtual bool SetGood()
    {
        return true;
    }
    public virtual bool SetBad()
    {
        return false;
    }


    protected void ShowPreList()
    {
        _neighborList = new List<Vector3>();

        RectList(_neighborDownList, false, true, false, -1);
        RectList(_neighborCenterList, false, true, false, 0);
        RectList(_neighborUpList, false, true, false, 1);

    }

    protected void RectList(List<Vector3> neighborList, bool useX, bool useY, bool useZ, int rept)
    {
        neighborList.Add(ListUseXYZ(useX, useY, useZ, rept, -1, -1));
        neighborList.Add(ListUseXYZ(useX, useY, useZ, rept, -1, 0));
        neighborList.Add(ListUseXYZ(useX, useY, useZ, rept, -1, 1));
        neighborList.Add(ListUseXYZ(useX, useY, useZ, rept, 0, 1));
        neighborList.Add(ListUseXYZ(useX, useY, useZ, rept, 1, 1));
        neighborList.Add(ListUseXYZ(useX, useY, useZ, rept, 1, 0));
        neighborList.Add(ListUseXYZ(useX, useY, useZ, rept, 1, -1));
        neighborList.Add(ListUseXYZ(useX, useY, useZ, rept, 0, -1));
        neighborList.Add(ListUseXYZ(useX, useY, useZ, rept, 0, 0));
    }

    protected Vector3 ListUseXYZ(bool useX, bool useY, bool useZ, int rept, int first = 0, int second = 0, int third = 0)
    {
        int x = rept;
        int y = rept;
        int z = rept;
        if (useX == false)
        {
            x = first;
        }
        if (useY == false)
        {
            if(useX)
            {
                y = first;
            }
            else
            {
                y = second;
            }
        }
        if (useZ == false)
        {
            if(useX)
            {
                if(useY)
                {
                    z = first;
                }
                else
                {
                    z = second;
                }
            }
            else if(useY)
            {
                z = second;
            }
            else
            {
                z = third;
            }
        }
        return new Vector3(x, y, z);
    }


    public void BuildNeighBor()
    {
        _neighborList = new List<Vector3>();

        AddAllNeighBor(ref _neighborList, _neighborDownList);
        AddAllNeighBor(ref _neighborList, _neighborCenterList);
        AddAllNeighBor(ref _neighborList, _neighborUpList);
        
    }

    void AddAllNeighBor(ref List<Vector3> neighborList, List<Vector3> toAddList)
    {
        int lenght = toAddList.Count;
        for (int i = 0; i < lenght; i++)
        {
            _neighborList.Add(toAddList[i]);
        }
    }
}
