using System.Collections.Generic;
using UnityEngine;
using static GenerateDungeon3;

[CreateAssetMenu(fileName = "RulesSO", menuName = "Scriptable Objects/RulesSO")]
public class RulesSO : ScriptableObject
{
    [SerializeField] protected bool _valueBoolean;
    [SerializeField] protected int _valueInt;

    [SerializeField] public List<Vector3> _neighborList;

    [SerializeField] public List<Vector3> _neighborDownList;
    [SerializeField] public List<Vector3> _neighborCenterList;
    [SerializeField] public List<Vector3> _neighborUpList;

    protected void Reset()
    {
        _valueBoolean = true;
        _valueInt = 4;
    }

    void BuildNeighBor()
    {
        _neighborList = new List<Vector3>();

        AddAllNeighBor(ref _neighborList, _neighborDownList);
        AddAllNeighBor(ref _neighborList, _neighborCenterList);
        AddAllNeighBor(ref _neighborList, _neighborUpList);
    }

    void AddAllNeighBor(ref List<Vector3> neighborList, List<Vector3> toAddList)
    {
        int lenght = neighborList.Count;
        for (int i = 0; i < lenght; i++)
        {
            _neighborList.Add(toAddList[i]);
        }
    }

    public virtual bool ConditionNeighborStates(Cell neighbor)
    {
        return neighbor._myGameObject.activeSelf == _valueBoolean;
    }

    public virtual bool ConditionNeighborNumber(int numberGoodStates)
    {
        return numberGoodStates == _valueInt;
    }

    public virtual bool SetGood()
    {
        return true;
    }
    public virtual bool SetBad()
    {
        return false;
    }
}
