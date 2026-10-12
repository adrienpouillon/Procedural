using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "Dungeon/Room")]
public class RoomBaseSO : ScriptableObject
{
    [Header("Identity")]
    public string styleId = "Normal";

    public Color gizmo_color = Color.white;

    public int height = 6;
    public int width = 6;
    public int deepth = 6;

    [Header("Identity")]
    public GameObject prefab;
}
