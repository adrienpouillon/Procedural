using UnityEngine;
using static GenerateDungeon3;

[CreateAssetMenu(fileName = "RMore", menuName = "Scriptable Objects/RulesSOMore")]
public class RMore : RulesSO
{
    public override bool ConditionNeighborStates(Cell neighbor)
    {
        return neighbor._myGameObject.activeSelf == _valueBoolean;
    }

    public override bool ConditionNeighborNumber(int numberGoodStates)
    {
        return numberGoodStates >= _valueInt;
    }

    public override bool SetGood()
    {
        return true;
    }
    public override bool SetBad()
    {
        return false;
    }
}
