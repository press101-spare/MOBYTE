using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawn : MonoBehaviour
{
    public Transform[] _garbage;//쓰레기 종류
    public Transform _pointOne;//가장 낮은 좌표
    public Transform _pointTwo;//그반대

    public Stack<GameObject> _garbage1Pool = new Stack<GameObject>();
    public Stack<GameObject> _garbage2Pool = new Stack<GameObject>();
    public Stack<GameObject> _garbage3Pool = new Stack<GameObject>();

    

    public bool _playerIn;

    private float _timer;
    private void Start()
    {
        CreatePool();
    }
    private void Update()
    {
        if(_playerIn)
        {
            _timer += Time.deltaTime;
            if( _timer >4)
            {
                for(int i =0; i<3;i++)
                {
                    SpawnGarbage();
                }
                _timer = 0;
            }
        }
    }


    public void SpawnGarbage()
    {
        int a=Random.Range(0,10);
        Vector2 _vec = new Vector2(Random.Range(_pointOne.position.x,_pointTwo.position.x), Random.Range(_pointOne.position.y, _pointTwo.position.y));
        if(a>=5)
        {
            if(_garbage1Pool.Count!=0)
            {
                GameObject garbage = _garbage1Pool.Pop();
                garbage.SetActive(true);
                garbage.transform.position= _vec;
            }
            else
            {
                Instantiate(_garbage[0], _vec, Quaternion.identity);
            }
            
        }
        else if(a>1)
        {
            if (_garbage2Pool.Count != 0)
            {
                GameObject garbage = _garbage2Pool.Pop();
                garbage.SetActive(true);
                garbage.transform.position = _vec;
            }
            else
            {
                Instantiate(_garbage[0], _vec, Quaternion.identity);
            }
        }
        else
        {
            if (_garbage3Pool.Count != 0)
            {
                GameObject garbage = _garbage3Pool.Pop();
                garbage.SetActive(true);
                garbage.transform.position = _vec;
            }
            else
            {
                Instantiate(_garbage[0], _vec, Quaternion.identity);
            }
        }
    }

    public void CreatePool()
    {
        for(int i = 0 ; i < 10; i++)
        {
            GameObject garbage = Instantiate(_garbage[0].gameObject);
            garbage.SetActive(false);
            garbage.transform.parent = transform;
            _garbage1Pool.Push(garbage);
        }
        for(int i = 0 ; i < 10; i++)
        {
            GameObject garbage = Instantiate(_garbage[1].gameObject);
            garbage.SetActive(false);
            garbage.transform.parent = transform;
            _garbage2Pool.Push(garbage);
        }
        for(int i = 0 ; i < 10; i++)
        {
            GameObject garbage = Instantiate(_garbage[2].gameObject);
            garbage.SetActive(false);
            garbage.transform.parent = transform;
            _garbage3Pool.Push(garbage);
        }
    }


}
