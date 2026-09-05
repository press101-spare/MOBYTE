using UnityEngine;

public class Garbage : MonoBehaviour
{
    public float _timer;
    public int id;

    private GarbageSpawn _pool;


    private void Start()
    {
        _pool = GetComponentInParent<GarbageSpawn>();
    }
    private void OnEnable()
    {
        _timer = 0;
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer>7)
        {
            RePooling();
        }
    }
    private void OnMouseDown()
    {
        RePooling();
    }

    public void RePooling()
    {
        
        if (id==0)
        {
            _pool._garbage1Pool.Push(gameObject);
        }
        else if(id==1)
        {
            _pool._garbage2Pool.Push(gameObject);
        }
        else
        {
            _pool._garbage3Pool.Push(gameObject);
        }

        gameObject.SetActive(false);
    }
}
