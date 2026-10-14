using System.Collections.Generic;
using UnityEngine;
using static GenerateDungeon3;

[CreateAssetMenu(fileName = "RulesSO", menuName = "Scriptable Objects/RulesSO")]
public class RulesSO : ScriptableObject
{
    [SerializeField] protected bool _valueBoolean;
    [SerializeField] protected int _valueInt;

    [SerializeField] public List<Vector3> _neighborList;

    protected void Reset()
    {
        _valueBoolean = true;
        _valueInt = 4;
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
