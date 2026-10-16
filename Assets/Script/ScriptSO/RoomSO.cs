using UnityEngine;

[CreateAssetMenu(fileName = "RoomSO", menuName = "Scriptable Objects/RoomSO")]
public class RoomSO : ScriptableObject
{
    [Header("Identity")]
    public string _styleId;

    public Color _gizmo_color;

    [Header("Size")]
    public float _minWidth;
    public float _maxWidth;
    public float _minHeight;
    public float _maxHeight;
    public float _minDeepth;
    public float _maxDeepth;

    [Header("Prefab")]
    public GameObject _prefab;

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
    }
}
