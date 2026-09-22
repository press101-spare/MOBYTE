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

    private void OnEnable()
    {
        RandomTable();
    }

    public void RandomTable()
    {
        _haveData.Clear();
        _spawnPoints.Clear();
        _spawnPoints = _originPoints.ToList();

        for (int i = 0; i<3;i++)
        {
            GambleSoData data = _gambleList._gambleList[Random.Range(0, _gambleList._gambleList.Count)];
            if (_haveData.Count != 0)
            {
                for(int j =0; j<_haveData.Count;j++)
                {
                    if (_haveData[j] == data)
                    {
                        data = _gambleList._gambleList[Random.Range(0, _gambleList._gambleList.Count)];
                        j = -1;
                        continue;
                    }
                }
            }
            _haveData.Add(data);
            Transform point = _spawnPoints[Random.Range(0,_spawnPoints.Count)];
            GameObject table = Instantiate(data._gambleTable, point.position,Quaternion.identity);
            table.transform.parent = _tableGroup;
            _spawnPoints.Remove(point);
        }
        
        
    }
}
