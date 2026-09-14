using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Test_Roulet_HTY : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Coll());
    }

    private IEnumerator Coll()
    {
        while (true)
        {
            transform.Rotate(0, 0, 1);

            yield return null;
            yield return null;
            yield return null;


        }
    }
}
