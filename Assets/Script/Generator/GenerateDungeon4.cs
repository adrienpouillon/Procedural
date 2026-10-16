using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerateDungeon4 : MonoBehaviour
{
    [Header("Config")]
    [SerializeField, ReadOnly] int _iteration;
    [SerializeField] float _randGenerate;
    [SerializeField] GameObject _prefab;

    [Header("Seed")]
    [SerializeField] private int _seed;
    [SerializeField] private bool _useRandomSeed;
    [SerializeField] private int _minValueSeed;
    [SerializeField] private int _maxValueSeed;

    [Header("Data required")]
    [SerializeField] MapSO _mapSO;
    [SerializeField] RoomSO _roomSO;
    [SerializeField] List<RulesSO> _rulesSOList;

    List<List<Cell>> _cellListList;

    public List<List<Cell>> CellListList
    {
        get { return _cellListList; }
    }

    public class Cell
    {
        public ObjectCube _myCube;
        public GameObject _myGameObject;
        public GenerateDungeon4 _generateDungeon4;

        public void Create(GameObject prefab)
        {
            float padding = _generateDungeon4._mapSO._padding;
            _myGameObject = Instantiate(prefab, _generateDungeon4.gameObject.transform);
            _myGameObject.transform.position = _myCube._xyz;
            _myGameObject.transform.localScale = _myCube._whd - new Vector3(padding, padding, padding);
        }

        public void Update()
        {
            float padding = _generateDungeon4._mapSO._padding;
            _myGameObject.transform.position = _myCube._xyz;
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
            return GenerateDungeon4.CalculePosCube(_xyz, _whd);
        }

        public Vector3 CalculePosCenter()
        {
            return GenerateDungeon4.CalculePosCenter(_xyz, _whd);
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

        _seed = UnityEngine.Random.Range(_minValueSeed, _maxValueSeed);
        _useRandomSeed = false;

        _randGenerate = 0.5f;
    }

    [EasyButtons.Button("Generate Grid")]
    public void GenerateG()
    {
        if (_useRandomSeed)
        {
            _seed = UnityEngine.Random.Range(_minValueSeed, _maxValueSeed);
        }
        UnityEngine.Random.InitState(_seed);
        GenerateGrid();
    }

    [EasyButtons.Button("Generate Round")]
    public void GenerateR()
    {
        if (_cellListList != null)
        {
            if (GenerateRound(ref _cellListList, _iteration))
            {
                _iteration++;
            }
        }
    }
    [EasyButtons.Button("Clean")]
    public void Clean()
    {
       foreach ( Transform child in this.transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }
    void GenerateGrid()
    {
        if (_cellListList != null)
        {
            if (_cellListList.Count != 0)
            {
                DestroyGrid(_cellListList);
            }
            _cellListList = new List<List<Cell>>();
        }
        else
        {
            _cellListList = new List<List<Cell>>();
        }
        _iteration = 0;

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


        GridRandomValue(ref _cellListList, roomPS._whd, mapPS._whd, _randGenerate);


    }

    public bool GenerateRound(ref List<List<Cell>> cellListList, int round)
    {
        int lenghtListRoom = _rulesSOList.Count;
        if (lenghtListRoom <= round)
        {
            return false;
        }

        //RulesSO rule = _rulesSOList[round];
        //rule.BuildNeighBor();

        List<List<Cell>> newCellListList = new List<List<Cell>>();

        Vector3 offset = new Vector3(_mapSO._x, _mapSO._y, _mapSO._z);
        //Vector3 scale = new Vector3(UnityEngine.Random.Range(_roomSO._minWidth, _roomSO._maxWidth), UnityEngine.Random.Range(_roomSO._minHeight, _roomSO._maxHeight), UnityEngine.Random.Range(_roomSO._minDeepth, _roomSO._maxDeepth));
        float scale = 10.3f;
        float scaleY = 8.3f;

        Vector2 sizeMap = new Vector2(cellListList[0].Count, cellListList.Count);
        int lenghtI = (int)sizeMap.y;
        for (int i = 0; i < lenghtI; i++)
        {
            newCellListList.Add(new List<Cell>());
            int lenghtJ = (int)sizeMap.x;
            for (int j = 0; j < lenghtJ; j++)
            {
                //Vector3 scale = new Vector3(UnityEngine.Random.Range(_roomSO._minWidth, _roomSO._maxWidth), UnityEngine.Random.Range(_roomSO._minHeight, _roomSO._maxHeight), UnityEngine.Random.Range(_roomSO._minDeepth, _roomSO._maxDeepth));

                float x = ((float)j + 1f) / scale;//+ offset.x) / 100f;
                float z = ((float)i + 1f) / scale;//+ offset.z) / 100f;
                float y = Mathf.PerlinNoise(x,z) * scaleY;//- offset.y;
                Debug.Log("x " + x + " y " + y + " z " + z);
                
                Cell newCell = cellListList[i][j];
                newCell._myCube.SetXYZWHD(j, y, i, 1f, 1f, 1f);
                newCell.Update();
                //SearchNeighbor(cellListList, currente, rule, new Vector2(i, j), sizeMap);
                newCellListList[i].Add(newCell);
            }
        }
        cellListList = newCellListList;
        return true;
    }

    /*Cell SearchNeighbor(List<List<Cell>> cellListList, Cell my, RulesSO rule, Vector2 myIJ, Vector3 sizeListListList)
    {
        List<Vector2> neighborList = rule.NeighBorList;
        int number = 0;
        int indexNull = -1;

        foreach (Vector3 ijk in neighborList)
        {
            //int z = (int)ijk.x + (int)myIJ.x;
            int y = (int)ijk.y + (int)myIJ.x;
            int x = (int)ijk.z + (int)myIJ.y;
            if (indexNull < x && x < sizeListListList.x && indexNull < y && y < sizeListListList.y && indexNull < z && z < sizeListListList.z)
            {
                Cell neighbor = cellListList[y][x];
                bool iss = rule.ConditionNeighborStates(neighbor);
                if (iss)
                {
                    number++;
                }
            }
        }

        bool isss = rule.ConditionNeighborNumber(number);
        if (isss)
        {
            my._myGameObject.SetActive(rule.SetGood());
        }
        else
        {
            my._myGameObject.SetActive(rule.SetBad());
        }
        return my;
    }*/

    void GridRandomValue(ref List<List<Cell>> cellListList, Vector3 sizeCell, Vector3 sizeMap, float rand)
    {
        int lenghtI = (int)sizeMap.z;
        for (int i = 0; i < lenghtI; i++)
        {
            cellListList.Add(new List<Cell>());
            int lenghtJ = (int)sizeMap.y;
            for (int j = 0; j < lenghtJ; j++)
            {
                Cell cell = new Cell();
                cell._myCube.SetXYZ(new Vector3(j, 0f, i));
                cell._myCube.SetWHD(sizeCell);
                cell._generateDungeon4 = this;
                cell.Create(_prefab);
                //cell._myGameObject.SetActive(false);
                //float randValue = UnityEngine.Random.Range(0f, 1f);
                //if (randValue >= rand)
                //{
                cell._myGameObject.SetActive(true);
                //}
                cellListList[i].Add(cell);
            }
        }
    }

    void DestroyGrid(List<List<Cell>> cellListList)
    {
        foreach (List<Cell> cellList in cellListList)
        {
            foreach (Cell cell in cellList)
            {
                cell.Destroy();
            }
        }
    }

    ObjectCube RandomValue(MinMaxObjectCube minMaxObjectCube)
    {
        ObjectCube objectCube = new ObjectCube();
        objectCube._xyz.x = UnityEngine.Random.Range(minMaxObjectCube._min._xyz.x, minMaxObjectCube._max._xyz.x);
        objectCube._xyz.y = UnityEngine.Random.Range(minMaxObjectCube._min._xyz.y, minMaxObjectCube._max._xyz.y);
        objectCube._xyz.z = UnityEngine.Random.Range(minMaxObjectCube._min._xyz.z, minMaxObjectCube._max._xyz.z);
        objectCube._whd.x = UnityEngine.Random.Range(minMaxObjectCube._min._whd.x, minMaxObjectCube._max._whd.x);
        objectCube._whd.y = UnityEngine.Random.Range(minMaxObjectCube._min._whd.y, minMaxObjectCube._max._whd.y);
        objectCube._whd.z = UnityEngine.Random.Range(minMaxObjectCube._min._whd.z, minMaxObjectCube._max._whd.z);
        return objectCube;
    }
}

/*seed
    scale
    octave
    persistance
    lacunarity
    offset

    xy
    noise
    add
    0.1
    tile/biome



    x + offset / scale * fecency
    noise()*2 -i*/