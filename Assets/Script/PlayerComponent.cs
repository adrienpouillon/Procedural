using UnityEngine;
using static GenerateDungeon3;

public class PlayerComponent : EntityComponent
{
    [SerializeField] private EndRoomComponent _endRoom;
    protected override void OnLaunchGame()
    {
        RandomCell();
        color = Color.green;
    }

    protected override void OnDraw()
    {
        if(_endRoom != null && _endRoom.Start)
        {
            Cell currentCellEndRoom = _endRoom.CurrentCell;
            if (IsSameCell(currentCellEndRoom, CurrentCell))
            {
                _generateDungeon3.GenerateR();
                _endRoom.RandomCell();
            }
        }
    }

    protected override void OnUpdateGrid()
    {

    }

    protected override void OnExitGrid()
    {
        RandomCell();
    }

    protected override void OnQuitGame()
    {

    }

    [EasyButtons.Button("Up")]
    void MoveUp()
    {
        Vector3 formerIJK = _ijk;
        _ijk = new Vector3(_ijk.x, _ijk.y + 1, _ijk.z);
        if (IsValid() < EnumIsValid.Good)
        {
            _ijk = formerIJK;
        }
    }

    [EasyButtons.Button("Forward")]
    void MoveForward()
    {
        Vector3 formerIJK = _ijk;
        _ijk = new Vector3(_ijk.x, _ijk.y, _ijk.z + 1);
        if (IsValid() < EnumIsValid.Good)
        {
            _ijk = formerIJK;
        }
    }

    [EasyButtons.Button("Left")]
    void MoveLeft()
    {
        Vector3 formerIJK = _ijk;
        _ijk = new Vector3(_ijk.x + 1, _ijk.y, _ijk.z);
        if (IsValid() < EnumIsValid.Good)
        {
            _ijk = formerIJK;
        }
    }

    [EasyButtons.Button("Right")]
    void MoveRight()
    {
        Vector3 formerIJK = _ijk;
        _ijk = new Vector3(_ijk.x - 1, _ijk.y, _ijk.z);
        if (IsValid() < EnumIsValid.Good)
        {
            _ijk = formerIJK;
        }
    }

    [EasyButtons.Button("Backward")]
    void MoveBackward()
    {
        Vector3 formerIJK = _ijk;
        _ijk = new Vector3(_ijk.x, _ijk.y, _ijk.z - 1);
        if (IsValid() < EnumIsValid.Good)
        {
            _ijk = formerIJK;
        }
    }


    [EasyButtons.Button("Down")]
    void MoveDown()
    {
        Vector3 formerIJK = _ijk;
        _ijk = new Vector3(_ijk.x, _ijk.y - 1, _ijk.z);
        if (IsValid() < EnumIsValid.Good)
        {
            _ijk = formerIJK;
        }
    }
}
