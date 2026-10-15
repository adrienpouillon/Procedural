using System.Collections.Generic;
using UnityEngine;
using static GenerateDungeon3;

public class EntityComponent : MonoBehaviour
{
    protected Vector3 _ijk;
    protected List<List<List<Cell>>> _cellListListList = null;
    bool _isPlug = false;

    [Header("component Required")]
    [SerializeField] GenerateDungeon3 _generateDungeon3;
    bool _start = false;

    public bool Start
    {
        get { return _start; }
    }

    public Cell CurrentCell
    {
        get { return GetCell(_ijk); }
        set
        {
            Cell setCell = value;
            if(setCell == null)
            {
                _ijk = new Vector3(-1, -1, -1);
            }
            Vector3 xyz = setCell._myCube._xyz;
            _ijk = new Vector3(xyz.z, xyz.y, xyz.x);
        }
    }

    public enum EnumIsValid
    {
        None,
        Bad,
        Good,
    }

    public Vector3 GetSizeCell()
    {
        return new Vector3(_cellListListList[0][0].Count, _cellListListList[0].Count, _cellListListList.Count);
    }

    public bool IsSameCell(Cell A, Cell B)
    {
        return A._myCube._xyz == B._myCube._xyz;
    }

    protected virtual void Reset()
    {
        _ijk = new Vector3(-1, -1, -1);
        _cellListListList = null;
        PlugOnEventGenerateDungeon(true);
        _start = false;
    }

    [EasyButtons.Button("Start Game")]
    public void LaunchGame()
    {
        PlugOnEventGenerateDungeon(true);
        if (_cellListListList == null)
        {
            UpdateListCell();
        }
        OnLaunchGame();
        _start = true;
    }

    [EasyButtons.Button("Quit Game")]
    public void QuitGame()
    {
        OnQuitGame();
        PlugOnEventGenerateDungeon(false);
        _ijk = new Vector3(-1, -1, -1);
        _cellListListList = null;
        _start = false;
    }

    protected virtual void OnLaunchGame()
    {

    }

    protected virtual void OnUpdate()
    {

    }

    protected virtual void OnUpdateGrid()
    {

    }

    protected virtual void OnExitGrid()
    {

    }

    protected virtual void OnDraw()
    {

    }

    protected virtual void OnQuitGame()
    {

    }

    protected void RandomCell()
    {
        bool isCorrecte = false;
        while (isCorrecte == false)
        {
            Vector3 size = GetSizeCell();
            _ijk = new Vector3(Random.Range(0, (int)size.x), Random.Range(0, (int)size.y), Random.Range(0, (int)size.z));

            isCorrecte = IsValid() == EnumIsValid.Good;
        }
    }

    public EnumIsValid IsValid()
    {
        Cell currentCell = GetCell(_ijk);
        if (currentCell == null)
        {
            return EnumIsValid.None;
        }
        
        if (currentCell._myGameObject.activeSelf == true)
        {
            return EnumIsValid.Good;
        }
        return EnumIsValid.Bad;
    }

    void OnDrawGizmos()
    {
        if (_cellListListList == null)
        {
            return;
        }

        Cell currentCell = GetCell(_ijk);
        if(currentCell == null)
        {
            return;
        }

        OnDraw();

        Gizmos.color = UnityEngine.Color.green;
        ObjectCube entityCube = currentCell._myCube;
        Gizmos.DrawWireCube(entityCube._xyz, entityCube._whd);
        transform.position = entityCube._xyz;
    }
    void PlugOnEventGenerateDungeon(bool plug)
    {
        if (plug)
        {
            if (_isPlug == false)
            {
                _generateDungeon3.OnGridUpdate += UpdateListCell;
                _isPlug = true;
            }
        }
        else
        {
            if (_isPlug)
            {
                _generateDungeon3.OnGridUpdate -= UpdateListCell;
                _isPlug = false;
            }
        }
    }

    void UpdateListCell()
    {
        _cellListListList = _generateDungeon3.CellListListList;

        OnUpdateGrid();

        if (IsValid() == EnumIsValid.Bad)
        {
            OnExitGrid();
        }
    }

    protected Cell GetCell(Vector3 ijk)
    {
        Vector3 size = GetSizeCell();
        if ((ijk.x < 0f || ijk.y < 0f || ijk.z < 0f) || (ijk.x >= size.x || ijk.y >= size.y || ijk.z >= size.z))
        {
            return _cellListListList[(int)_ijk.x][(int)_ijk.y][(int)_ijk.z];
        }
        return null;
    }


}
