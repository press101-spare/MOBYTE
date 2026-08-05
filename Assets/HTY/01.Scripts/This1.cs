using UnityEngine;

public class This1 : MonoBehaviour
{
    [SerializeField] private ThisNumber _num;
    private string[] a;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        a=gameObject.name.Split('_');
        if(collision.gameObject.name == "TTY")
        {
            _num.Re(int.Parse(a[1]));
        }
    }
}
