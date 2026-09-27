using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomGamble : MonoBehaviour
{
    [SerializeField] private GambleSoList _gambleList;
    [SerializeField] private Transform _tableGroup;
    private List<GambleSoData> _haveData = new List<GambleSoData>();

    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private Transform[] _originPoints;

    private void Start()
    {
        RandomTable();
    }

    public void RandomTable()
    {
        _haveData.Clear();
        _spawnPoints.Clear();
        _spawnPoints = _originPoints.ToList();

        List<GambleSoData> randomList = new List<GambleSoData>(_gambleList._gambleList);

        for (int i = 0; i<3;i++)
        {
            int randomIndex = Random.Range(0, randomList.Count);
            GambleSoData data = randomList[randomIndex];
            randomList.RemoveAt(randomIndex);
            _haveData.Add(data);
            Transform point = _spawnPoints[Random.Range(0,_spawnPoints.Count)];
            GameObject table = Instantiate(data._gambleTable, point.position,Quaternion.identity);
            table.transform.parent = _tableGroup;
            _spawnPoints.Remove(point);
        }
    }
}
