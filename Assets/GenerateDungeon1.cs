using EasyButtons;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static GenerateDungeon1;

public class GenerateDungeon1 : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] int _attempt;
    [SerializeField, ReadOnly] int _attemptUse;
    [SerializeField] List<RoomSO> _listRoom;
    [SerializeField] int _padding;

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

        public Cube CalculePosCube()
        {
            return GenerateDungeon1.CalculePosCube(_xyz, _whd);
        }

        public Vector3 CalculePosCenter()
        {
            return GenerateDungeon1.CalculePosCenter(_xyz, _whd);
        }

    }

    public static Cube CalculePosCube(Vector3 xyz, Vector3 whd)
    {
        Cube cube = new Cube();
        cube.Set(xyz, xyz + whd);
        return cube;
    }

    public static Vector3 CalculePosCenter(Vector3 xyz, Vector3 whd)
    {
        Vector3 halfwhd = whd / 2;
        return xyz + halfwhd;
    }

    void Reset()
    {
        _padding = 0;
        _attempt = 200;

        _minValueSeed = int.MinValue;
        _maxValueSeed = int.MaxValue;

        _seed = Random.Range(_minValueSeed, _maxValueSeed);
        _useRandomSeed = false;
        //_roomValidList = new List<Room>();
    }

    public bool IsOverlapping(Vector3 position, Vector3 scale, float padding)
    {
        scale = Padding(scale, padding);
        Physics.SyncTransforms();
        return Physics.CheckBox(position, scale / 2f, Quaternion.identity);
    }

    public Vector3 Padding(Vector3 scale, float padding)
    {
        scale.x = scale.x + padding;
        scale.z = scale.z + padding;
        return scale;
    }

    [EasyButtons.Button("Generate")]
    public void Generate()
    {
        if (_useRandomSeed)
        {
            _seed = Random.Range(_minValueSeed, _maxValueSeed);
        }
        Random.InitState(_seed);
        GenerateRoomDungeon();
    }

    void GenerateRoomDungeon()
    {
        if (_roomValidList != null)
        {
            if (_roomValidList.Count != 0)
            {
                foreach (Room room in _roomValidList)
                {
                    room.Destroy();
                }
            }
        }
        else
        {
            _roomValidList = new List<Room>();
        }
        /*_roomValidList.Clear();
        int lenghtchild = gameObject.transform.childCount;
        for (int i = 0; i < lenghtchild; i++)
        {
            DestroyImmediate(gameObject.transform.GetChild(i));
        }*/

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
        int lenghtRoom = _mapSO._roomNumber;
        _attemptUse = 0;

        if(lenghtListRoom < 1)
        {
            return;
        }

        for (int i = 0; i < lenghtRoom; i++)
        {
            Debug.Log("i: " + i);
            Room room = new Room();
            RoomSO roomSO = _listRoom[Random.Range(0, lenghtListRoom-1)];
            room._sO = roomSO;
            

            while (_attemptUse < _attempt)
            {
                Debug.Log("attempt: " + _attemptUse);
                ObjectCube minRoomPS = new ObjectCube();
                Vector3 whd = mapPS._whd / 2;
                minRoomPS.SetXYZWHD(-whd.x, -whd.y, -whd.z, roomSO._minWidth, roomSO._minHeight, roomSO._minDeepth);

                ObjectCube maxRoomPS = new ObjectCube();
                maxRoomPS.SetXYZWHD(whd.x, whd.y, whd.z, roomSO._maxWidth, roomSO._maxHeight, roomSO._maxDeepth);
                Debug.Log("minpos: " + minRoomPS._xyz + "minwhd" + minRoomPS._whd);
                Debug.Log("maxpos: " + maxRoomPS._xyz + "maxwhd" + maxRoomPS._whd);
                MinMaxObjectCube minMaxRoom = new MinMaxObjectCube();
                minMaxRoom.Set(minRoomPS, maxRoomPS);

                ObjectCube roomPS = RandomValue(minMaxRoom);
                bool isCreate = CreateGameObject(ref room, roomPS, roomSO._prefab, _padding, gameObject.transform);
                _attemptUse++;
                if (isCreate)
                {
                    break;
                }
                
            }
            _roomValidList.Add(room);
        }
    }

    bool CreateGameObject(ref Room room, ObjectCube objectPS, GameObject prefab, int padding, Transform transformParent)
    {
        Vector3 pos = objectPS.CalculePosCenter();
        Vector3 scale = objectPS._whd;
        if (IsOverlapping(pos, scale, padding) == false)
        {
            room._pS = objectPS;
            room._gameObject = Instantiate(prefab, transformParent);
            room._gameObject.transform.localPosition = pos;
            room._gameObject.transform.localScale = scale;
            return true;
        }
        return false;
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
