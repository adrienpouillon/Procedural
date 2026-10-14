using EasyButtons;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GenerateDungeon2 : MonoBehaviour
{
    [Header("Config")]
    [SerializeField, ReadOnly] List<Partionning> _partionningValidList;
    [SerializeField] List<RoomSO> _listRoom;
    [SerializeField] float _padding;
    [SerializeField] int _maxSize;
    [SerializeField] Vector3 _minSize;
    [SerializeField] GameObject _prefabEmpty;

    [Header("Seed")]
    [SerializeField] private int _seed;
    [SerializeField] private bool _useRandomSeed;
    [SerializeField] private int _minValueSeed;
    [SerializeField] private int _maxValueSeed;

    [Header("Data required")]
    [SerializeField] MapSO _mapSO;

    public class Partionning
    {
        public Partionning _parent;
        public Partionning _child1;
        public Partionning _child2;
        public ObjectCube _myCube;
        public GameObject _myGameObject;


        public void Destroy()
        {
            DestroyImmediate(_myGameObject);
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
            return GenerateDungeon2.CalculePosCube(_xyz, _whd);
        }

        public Vector3 CalculePosCenter()
        {
            return GenerateDungeon2.CalculePosCenter(_xyz, _whd);
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
        _minSize = new Vector3(1f,1f,1f);

        _minValueSeed = int.MinValue;
        _maxValueSeed = int.MaxValue;

        _seed = Random.Range(_minValueSeed, _maxValueSeed);
        _useRandomSeed = false;
        //_roomValidList = new List<Room>();
    }

    public Vector3 Padding(Vector3 scale, float padding)
    {
        scale.x = scale.x - padding;
        scale.y = scale.y - padding;
        scale.z = scale.z - padding;
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
        if (_partionningValidList != null)
        {
            if (_partionningValidList.Count != 0)
            {
                foreach (Partionning partionning in _partionningValidList)
                {
                    partionning.Destroy();
                }
            }
        }
        else
        {
            _partionningValidList = new List<Partionning>();
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
        if (lenghtListRoom < 0)
        {
            return;
        }

        Partionning map = new Partionning();
        map._myCube = mapPS;
        Debug.Log(mapPS + "  xyz " + mapPS._xyz + "  whd " + mapPS._whd);
        CreateGameObject(ref map, _prefabEmpty, _padding, gameObject.transform);
        ObjectCube roomPS = mapPS;
        Vector3 roomWHD = mapPS._whd;
        roomPS.SetWHD(roomWHD/100f);
        CreateChild(map, roomPS, _listRoom[0], _padding, _minSize);

        _partionningValidList.Add(map);
    }

    void CreateChild(Partionning parent, ObjectCube myObjectCube, RoomSO room, float padding, Vector3 minSize)
    {
        Debug.Log("Create Child");
        Partionning my = new Partionning();
        my._myCube = myObjectCube;
        my._parent = parent;
        my._child1 = new Partionning();
        my._child2 = new Partionning();

        GameObject prefab = _prefabEmpty;
        bool createChild1 = false;
        bool createChild2 = false;

        
        Vector3 whd1 = Vector3.one;
        Vector3 whd2 = Vector3.one;
        Vector3 pos1 = Vector3.zero;
        Vector3 pos2 = Vector3.zero;
        float rand = 1;
        switch (Random.Range(0, 3))
        {
            case 0:
                {
                    rand = Random.Range(0, my._myCube._whd.x);
                    whd1 = new Vector3(rand, my._myCube._whd.y, my._myCube._whd.z);
                    whd2 = new Vector3(my._myCube._whd.x - rand, my._myCube._whd.y, my._myCube._whd.z);
                    pos1 = new Vector3(my._myCube._xyz.x, my._myCube._xyz.y, my._myCube._xyz.z);
                    pos2 = new Vector3(my._myCube._xyz.x + rand, my._myCube._xyz.y, my._myCube._xyz.z);
                    break;
                }
            case 1:
                {
                    rand = Random.Range(0, my._myCube._whd.y);
                    whd1 = new Vector3(my._myCube._whd.x, rand, my._myCube._whd.z);
                    whd2 = new Vector3(my._myCube._whd.x, my._myCube._whd.y - rand, my._myCube._whd.z);
                    pos1 = new Vector3(my._myCube._xyz.x, my._myCube._xyz.y, my._myCube._xyz.z);
                    pos2 = new Vector3(my._myCube._xyz.x, my._myCube._xyz.y + rand, my._myCube._xyz.z);
                    break;
                }
            case 2:
                {
                    rand = Random.Range(0, my._myCube._whd.z);
                    whd1 = new Vector3(my._myCube._whd.x, my._myCube._whd.y, rand);
                    whd2 = new Vector3(my._myCube._whd.x, my._myCube._whd.y, my._myCube._whd.z - rand);
                    pos1 = new Vector3(my._myCube._xyz.x, my._myCube._xyz.y, my._myCube._xyz.z);
                    pos2 = new Vector3(my._myCube._xyz.x, my._myCube._xyz.y, my._myCube._xyz.z + rand);
                    break;
                }
        }
        if (whd1.x > minSize.x && whd1.y > minSize.y && whd1.z > minSize.z)
        {
            createChild1 = true;

            if (whd2.x > minSize.x && whd2.y > minSize.y && whd2.z > minSize.z)
            {
                createChild2 = true;
            }
        }
        else if (whd2.x > minSize.x && whd2.y > minSize.y && whd2.z > minSize.z)
        {
            createChild2 = true;
        }
        else
        {
            prefab = room._prefab;
        }

        CreateGameObject(ref my, prefab, padding, parent._myGameObject.transform);

        if(createChild1)
        {
            
            ObjectCube objectCube = new ObjectCube();
            objectCube.SetXYZ(pos1);
            objectCube.SetWHD(whd1);
            CreateChild(my, objectCube, room, padding, minSize);
        }
        if (createChild2)
        {
            ObjectCube objectCube = new ObjectCube();
            objectCube.SetXYZ(pos2);
            objectCube.SetWHD(whd2);
            CreateChild(my, objectCube, room, padding, minSize);
        }
    }
    
    bool CreateGameObject(ref Partionning my, GameObject prefab, float padding, Transform transformParent)
    {
        Vector3 pos;
        Vector3 scale;
        if(prefab == _prefabEmpty)
        {
            pos = Vector3.zero;
            scale = Vector3.one;
        }
        else
        {
            pos = my._myCube.CalculePosCenter();
            scale = my._myCube._whd;
        }
        scale = Padding(scale, padding);

        my._myGameObject = Instantiate(prefab, transformParent);
        my._myGameObject.transform.localPosition = pos;
        my._myGameObject.transform.localScale = scale;//new Vector3(scale.x / transformParent.lossyScale.x, scale.y / transformParent.lossyScale.y, scale.z / transformParent.lossyScale.z);
        Debug.Log(my._myGameObject.transform + "  xyz " + pos + "  whd " + scale);
        return true;
        //return false;
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
