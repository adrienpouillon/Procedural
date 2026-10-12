using UnityEngine;

[CreateAssetMenu(fileName = "RoomSO", menuName = "Scriptable Objects/RoomSO")]
public class RoomSO : ScriptableObject
{
    [Header("Identity")]
    public string _styleId;

    public Color _gizmo_color;

    [Header("Size")]
    public int _minHeight;
    public int _maxHeight;
    public int _minWidth;
    public int _maxWidth;
    public int _minDeepth;
    public int _maxDeepth;

    [Header("Prefab")]
    public GameObject _prefab;

    public float _tilling;

    void Reset()
    {
        _styleId = "Room";
        _gizmo_color = Color.white;

        _minWidth = 2;
        _maxWidth = 2;
        _minHeight = 6;
        _maxHeight = 6;
        _minDeepth = 6;
        _maxDeepth = 6;

        _tilling = 4;
    }
}
