using DG.Tweening;
using TMPro;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class PaperMove : MonoBehaviour
{
    private Sequence sq;
    public Transform endPos;
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;
    public TextMeshProUGUI text4;
    public void Start()
    {
        sq = DOTween.Sequence();
        sq.Append(transform.DOMove(endPos.position, 0.6f));
        sq.AppendCallback(() => text1.rectTransform.DOAnchorPosY(text1.rectTransform.anchoredPosition.y - 700f, 0.2f));
        sq.AppendInterval(0.15f);
        sq.AppendCallback(() => text2.rectTransform.DOAnchorPosY(text2.rectTransform.anchoredPosition.y - 700f, 0.2f));
        sq.AppendInterval(0.15f);
        sq.AppendCallback(() => text3.rectTransform.DOAnchorPosY(text3.rectTransform.anchoredPosition.y - 700f, 0.2f));
        sq.AppendInterval(0.15f);
        sq.AppendCallback(() => text4.rectTransform.DOAnchorPosY(text4.rectTransform.anchoredPosition.y - 700f, 0.2f));
    }
}
