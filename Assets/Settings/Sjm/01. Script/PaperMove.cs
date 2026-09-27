using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class PaperMove : MonoBehaviour
{
    private Sequence sq;
    public Transform endPos;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public void Start()
    {
        sq = DOTween.Sequence();
        sq.Append(transform.DOMove(endPos.position, 0.3f));
        sq.AppendInterval(0.3f);
        sq.Append(text1.DOFade(1, 0.2f));
        sq.Join(text2.DOFade(1, 0.2f));
        sq.Join(text3.DOFade(1, 0.2f));

    }
}
