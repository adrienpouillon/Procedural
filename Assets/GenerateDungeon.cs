using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using EasyButtons;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int _attempt;
    [SerializeField] List<RoomSO> _listRoom;

    [Header("Seed")]
    [SerializeField] private int _seed;
    [SerializeField] private bool _useRandomSeed;
    [SerializeField] private int _minValueSeed;
    [SerializeField] private int _maxValueSeed;

    [Header("Data required")]
    [SerializeField] MapSO _mapSO;

    List<Room> _roomValidList;

    public struct Room
    {
        public ScriptableObject _sO;
        public ObjectCube _pS;
        public GameObject _gameObject;

        public void Destroy()
        {
            _sO = null;
            DestroyImmediate(_gameObject);
        }
    }

    public struct Cube
    {
        public Vector3 _leftTopUnder;
        public Vector3 _rightDownOnder;

        public void Set(Vector3 leftTopUnder, Vector3 rightDownOnder)
        {
            _leftTopUnder = leftTopUnder;
            _rightDownOnder = rightDownOnder;
        }
    }

    public struct MinMaxObjectCube
    {
        public ObjectCube _min;
        public ObjectCube _max;

        public void Set(ObjectCube min, ObjectCube max)
        {
            _min = min;
            _max = max;
        }
    }

    public struct ObjectCube
    {
        public Vector3 _xyz;
        public Vector3 _whd;

        public void SetXYZWHD(float x, float y, float z, float w, float h, float d)
        {
            _xyz = new Vector3(x, y, z);
            _whd = new Vector3(w, h, d);
        }
        public void SetXYZWHD(Vector3 xyz, Vector3 whd)
        {
            _xyz = xyz;
            _whd = whd;
        }

        public void SetXYZ(float x, float y, float z)
        {
            _xyz = new Vector3(x, y, z);
        }
        public void SetXYZ(Vector3 xyz)
        {
            _xyz = xyz;
        }

        public void SetWHD(float w, float h, float d)
        {
            _whd = new Vector3(w, h, d);
        }
        public void SetWHD(Vector3 xyz)
        {
            _whd = xyz;
        }


        public void SetXW(float x, float w)
        {
            _xyz.x = x;
            _whd.x = w;
        }

        public void SetYH(float y, float h)
        {
            _xyz.y = y;
            _whd.y = h;
        }

        public void SetZD(float z, float d)
        {
            _xyz.z = z;
            _whd.z = d;
        }

        public Cube CalculePos()
        {
            return CalculePos();
        }

    }

    public static Cube CalculePos(Vector3 xyz, Vector3 whd)
    {
        Cube cube = new Cube();
        cube.Set(xyz, xyz + whd);
        return cube;
    }

    void Reset()
    {
        _minValueSeed = int.MinValue;
        _maxValueSeed = int.MaxValue;

        _seed = Random.Range(_minValueSeed, _maxValueSeed);
        _useRandomSeed = false;
    }

    [Button("Generate")]
    public void Generate()
    {
        if (_useRandomSeed)
        {
            _seed = Random.Range(_minValueSeed, _maxValueSeed);
        }
        Random.InitState(_seed);
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        foreach(Room room in _roomValidList)
        {
            room.Destroy();
        }

        ObjectCube mapPS;
        MapSO ob = _mapSO;
        ObjectCube minMapPS = new ObjectCube();
        minMapPS.SetXYZWHD(_mapSO._x, _mapSO._y, _mapSO._z, _mapSO._minWidth, _mapSO._minHeight, _mapSO._minDeepth);

        ObjectCube maxMapPS = new ObjectCube();
        maxMapPS.SetXYZWHD(_mapSO._x, _mapSO._y, _mapSO._z, _mapSO._maxWidth, _mapSO._maxHeight, _mapSO._maxDeepth);

        MinMaxObjectCube minMaxMap = new MinMaxObjectCube();
        minMaxMap.Set(minMapPS, maxMapPS);

        mapPS = RandomValue(minMaxMap);

        int lenghtListRoom = _listRoom.Count;
        int lenght = _mapSO._roomNumber;
        int j = 0;
        for (int i = 0; i < lenght; i++)
        {
            Room room = new Room();
            RoomSO roomSO = _listRoom[Random.Range(0, lenghtListRoom)];
            _roomValidList.Add(room);
            _roomValidList[i]._sO = roomSO;

            while (j < _attempt)
            {

                ObjectCube minRoomPS = new ObjectCube();
                minRoomPS.SetXYZWHD(mapPS._xyz.x, mapPS._xyz.y, mapPS._xyz.z, roomSO._minWidth, roomSO._minHeight, roomSO._minDeepth);

                ObjectCube maxRoomPS = new ObjectCube();
                maxRoomPS.SetXYZWHD(mapPS._xyz.x, mapPS._xyz.y, mapPS._xyz.z, roomSO._maxWidth, roomSO._maxHeight, roomSO._maxDeepth);

                MinMaxObjectCube minMaxRoom = new MinMaxObjectCube();
                minMaxRoom.Set(minRoomPS, maxRoomPS);

                ObjectCube roomPS = RandomValue(minMaxRoom);

                _roomValidList[i]._pS = roomPS;
                _roomValidList[i]._gameObject = ;

                j++;
                
            }
            
        }
    }

    ObjectCube RandomValue(MinMaxObjectCube minMaxObjectCube)
    {
        ObjectCube objectCube = new ObjectCube();
        objectCube._xyz.x = Random.Range(minMaxObjectCube._min._xyz.x, minMaxObjectCube._max._xyz.x);
        objectCube._xyz.y = Random.Range(minMaxObjectCube._min._xyz.y, minMaxObjectCube._max._xyz.y);
        objectCube._xyz.z = Random.Range(minMaxObjectCube._min._xyz.z, minMaxObjectCube._max._xyz.z);
        objectCube._whd.x = Random.Range(minMaxObjectCube._min._whd.x, minMaxObjectCube._max._whd.x);
        objectCube._whd.y = Random.Range(minMaxObjectCube._min._whd.y, minMaxObjectCube._max._whd.y);
        objectCube._whd.z = Random.Range(minMaxObjectCube._min._whd.z, minMaxObjectCube._max._whd.z);
        return objectCube;
    }
}

/*Random.Range
    width
    height


    foreach Room room in rooms
    padding

    for
    list.add(candidate);
AddRoomTofloor(candidate);


GenerateCorridor
    CarveCorridor
    PlaceCoordior
    PlaceCorridorTile


GenerateDungeon
    Room.clear
    IntiRazndom

    For i
    cadidate
    if
    roomsAdd

    SortRoom
    foreach
    Generate
    corridor

    Drawfloor
    DrawWall*/
