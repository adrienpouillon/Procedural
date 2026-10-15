using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using static GenerateDungeon3;

public class PlayerComponent : MonoBehaviour
{
    [SerializeField] GenerateDungeon3 generateDungeon3;
    Vector3 _ijk;
    List<List<List<Cell>>> _rCellListListList;

    void Reset()
    {
        _ijk = new Vector3(-1, -1, -1);
    }

    [EasyButtons.Button("Start Game")]
    void LaunchGame()
    {
        bool isIncorrecte = true;
        Cell currentPlayer;
        while (isIncorrecte)
        {
            Vector3 size = new Vector3(_rCellListListList[0][0].Count, _rCellListListList[0].Count, _rCellListListList.Count);
            _ijk = new Vector3(Random.Range(0, (int)size.x), Random.Range(0, (int)size.y), Random.Range(0, (int)size.z));

            currentPlayer = _rCellListListList[(int)_ijk.x][(int)_ijk.y][(int)_ijk.z];
            if (currentPlayer._myGameObject.activeSelf == true)
            {
                isIncorrecte = false;
            }
        }
    }

    bool IsValid()
    {
        

        currentPlayer = _rCellListListList[(int)_ijk.x][(int)_ijk.y][(int)_ijk.z];
        if (currentPlayer._myGameObject.activeSelf == true)
        {
            isIncorrecte = false;
        }
    }

    [EasyButtons.Button("Up")]
    void MoveUp()
    {

    }



    private void OnDrawGizmos()
    {
        Cell currentPlayer = _rCellListListList[(int)ijk.x][(int)ijk.y][(int)ijk.z];
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, currentPlayer._myCube._whd);

    }
}
