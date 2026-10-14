using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateDungeon3 : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] List<RulesSO> _rulesSOList;
    [SerializeField] int _goToIteration;
    [SerializeField] float _randGenerate;
    [SerializeField] GameObject _prefabFill;
    [SerializeField] GameObject _prefabEmpty;

    [Header("Seed")]
    [SerializeField] private int _seed;
    [SerializeField] private bool _useRandomSeed;
    [SerializeField] private int _minValueSeed;
    [SerializeField] private int _maxValueSeed;

    [Header("Data required")]
    [SerializeField] MapSO _mapSO;
    [SerializeField] RoomSO _roomSO;

    List<List<List<Cell>>> _cellListListList;

    public struct Cell
    {
        public ObjectCube _myCube;
        public GameObject _myGameObject;
        public GenerateDungeon3 _generateDungeon3;

        public void Create(GameObject prefab)
        {
            float padding = _generateDungeon3._mapSO._padding;
            _myGameObject = Instantiate(prefab, _generateDungeon3.gameObject.transform);
            _myGameObject.transform.localPosition = _myCube._xyz;
            _myGameObject.transform.localScale = _myCube._whd - new Vector3(padding, padding, padding);
        }

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
            return GenerateDungeon3.CalculePosCube(_xyz, _whd);
        }

        public Vector3 CalculePosCenter()
        {
            return GenerateDungeon3.CalculePosCenter(_xyz, _whd);
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
        _minValueSeed = int.MinValue;
        _maxValueSeed = int.MaxValue;

        _seed = Random.Range(_minValueSeed, _maxValueSeed);
        _useRandomSeed = false;


        _goToIteration = 0;
        _randGenerate = 0.5f;
    }

    [EasyButtons.Button("Generate Grid")]
    public void GenerateG()
    {
        if (_useRandomSeed)
        {
            _seed = Random.Range(_minValueSeed, _maxValueSeed);
        }
        Random.InitState(_seed);
        GenerateGrid();
    }

    [EasyButtons.Button("Generate Round")]
    public void GenerateR()
    {
        if (_cellListListList != null)
        {
            if (_useRandomSeed)
            {
                _seed = Random.Range(_minValueSeed, _maxValueSeed);
            }
            Random.InitState(_seed);
            GenerateRound();
        }
    }

    void GenerateGrid()
    {
        if (_cellListListList != null)
        {
            if (_cellListListList.Count != 0)
            {
                DestroyGrid(_cellListListList);
            }
        }
        else
        {
            _cellListListList = new List<List<List<Cell>>>();
        }

        ObjectCube mapPS;
        ObjectCube minMapPS = new ObjectCube();
        minMapPS.SetXYZWHD(_mapSO._x, _mapSO._y, _mapSO._z, _mapSO._minWidth, _mapSO._minHeight, _mapSO._minDeepth);

        ObjectCube maxMapPS = new ObjectCube();
        maxMapPS.SetXYZWHD(_mapSO._x, _mapSO._y, _mapSO._z, _mapSO._maxWidth, _mapSO._maxHeight, _mapSO._maxDeepth);

        MinMaxObjectCube minMaxMap = new MinMaxObjectCube();
        minMaxMap.Set(minMapPS, maxMapPS);

        mapPS = RandomValue(minMaxMap);


        ObjectCube roomPS;
        ObjectCube minRoomPS = new ObjectCube();
        minRoomPS.SetXYZWHD(0f, 0f, 0f, _roomSO._minWidth, _roomSO._minHeight, _roomSO._minDeepth);

        ObjectCube maxRoomPS = new ObjectCube();
        maxRoomPS.SetXYZWHD(0f, 0f, 0f, _roomSO._maxWidth, _roomSO._maxHeight, _roomSO._maxDeepth);

        MinMaxObjectCube minMaxRoom = new MinMaxObjectCube();
        minMaxRoom.Set(minRoomPS, maxRoomPS);

        roomPS = RandomValue(minMaxRoom);

        GridRandomValue(ref _cellListListList, roomPS._whd, mapPS._whd, _randGenerate);


        // GenerateR()

    }

    public void GenerateR()
    {
        int lenghtListRoom = _rulesSOList.Count;
        if (lenghtListRoom < 0)
        {
            return;
        }

        Vector3 sizeMap = new Vector3(_cellListListList.Count, _cellListListList[0].Count, _cellListListList[0][0].Count);
        int lenghtI = (int)sizeMap.z;
        for (int i = 0; i < lenghtI; i++)
        {
            int lenghtJ = (int)sizeMap.y;
            for (int j = 0; j < lenghtJ; j++)
            {
                int lenghtK = (int)sizeMap.x;
                for (int k = 0; k < lenghtK; k++)
                {
                    //_cellListListList[x][y][z];

                }
            }
        }

    void GridRandomValue(ref List<List<List<Cell>>> cellListListList, Vector3 sizeCell, Vector3 sizeMap, float rand)
    {
        int lenghtI = (int)sizeMap.z;
        for (int i = 0; i < lenghtI; i++)
        {
            cellListListList.Add(new List<List<Cell>>());
            int lenghtJ = (int)sizeMap.y;
            for (int j = 0; j < lenghtJ; j++)
            {
                cellListListList[i].Add(new List<Cell>());
                int lenghtK = (int)sizeMap.x;
                for (int k = 0; k < lenghtK; k++)
                {
                    Cell cell = new Cell();
                    cell._myCube.SetXYZ(new Vector3(k,j,i));
                    cell._myCube.SetWHD(sizeCell);
                    cell._generateDungeon3 = this;
                    if (Random.Range(0, 1) >= rand)
                    {
                        cell.Create(_prefabFill);
                    }
                    else
                    {
                        cell.Create(_prefabEmpty);
                    }
                    cellListListList[i][j].Add(cell);
                }
            }
        }
    }

    void DestroyGrid(List<List<List<Cell>>> cellListListList)
    {
        foreach (List<List<Cell>> cellListList in cellListListList)
        {
            foreach (List<Cell> cellList in cellListList)
            {
                foreach (Cell cell in cellList)
                {
                    cell.Destroy();
                }
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

/*
 Grille
etat
Voisin
Règle
4 voisins orthogonals alors B sinon A

Height
    width
    heibor
    if
*/


