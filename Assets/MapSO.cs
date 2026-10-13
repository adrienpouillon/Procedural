using UnityEngine;

[CreateAssetMenu(fileName = "MapBase", menuName = "Scriptable Objects/MapBase")]
public class MapSO : ScriptableObject
{
    [Header("Identity")]
    public string _styleId;

    public Color _gizmo_color;

    [Header("Tranform")]
    public int _x;
    public int _y;
    public int _z;
    public int _minHeight;
    public int _maxHeight;
    public int _minWidth;
    public int _maxWidth;
    public int _minDeepth;
    public int _maxDeepth;

    [Header("Config")]
    public int _difficulty;
    public int _roomNumber;

    void Reset()
    {
        _styleId = "Map";

        _gizmo_color = Color.grey;

        _x = 0;
        _y = 0;
        _z = 0;

        _minHeight = 100;
        _maxHeight = 100;
        _minWidth = 100;
        _maxWidth = 100;
        _minDeepth = 100;
        _maxDeepth = 100;

        _difficulty = 0;
        _roomNumber = 0;
    }
}
/* min area
min room
*/