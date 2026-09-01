using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TitleAnim : MonoBehaviour
{

    private Sequence sequence;
    public TextMeshProUGUI text;
    public TextMeshProUGUI title;
    public RectTransform rectTransform;
    public RectTransform dicepo;
    public Image backGround;
    public RectTransform rotate;
    private void Start()
    {   
        sequence = DOTween.Sequence();
        sequence.Append(rotate.DORotate(new Vector3(0, 0, 180), 0.4f));
        Debug.Log("D");
        sequence.Append(text.rectTransform.DOAnchorPos(rectTransform.anchoredPosition, 0.6f));
        sequence.Append(title.rectTransform.DOAnchorPos(dicepo.anchoredPosition, 0.5f)); ;
        sequence.AppendInterval(0.4f);
        sequence.AppendCallback(() =>
        {
           backGround.gameObject.SetActive(true);
        });
      
    }
   
}
