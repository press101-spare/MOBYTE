using System.Collections.Generic;
using UnityEngine;

public class RandomGamble : MonoBehaviour
{
    [SerializeField] private GambleSoList _gambleList;
    [SerializeField] private Transform _tableGroup;
    private List<GambleSoData> _haveData = new List<GambleSoData>();

    private void Start()
    {
        
    }
    private void OnEnable()
    {
        RandomTable();
    }

    public void RandomTable()
    {
        _haveData.Clear();
        for(int i = 0; i<3;i++)
        {
            GambleSoData data = _gambleList._gambleList[Random.Range(0, _gambleList._gambleList.Count)];
            if (_haveData.Count != 0)
            {
                for(int j =0; j<_haveData.Count;j++)
                {
                    if (_haveData[j]==data)
                    {
                        data = _gambleList._gambleList[Random.Range(0, _gambleList._gambleList.Count)];
                        j = -1;
                        continue;
                    }
                }
            }
            _haveData.Add(data);
            GameObject table = Instantiate(data._gambleTable, _tableGroup);
        }
        
        
    }
}
