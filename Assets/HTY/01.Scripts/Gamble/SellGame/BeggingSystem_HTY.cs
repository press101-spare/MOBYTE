using JJB.Script.Battle.Player.Progression;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BeggingSystem_HTY : MonoBehaviour
{
    [SerializeField] private GameObject _beggingCan;
    [SerializeField] private Button _canButton;
    [SerializeField] private TextMeshProUGUI _chipText;
    [SerializeField] private TextMeshProUGUI _getChipText;

    private const int MinChip = 50;

    private void Start()
    {
        _canButton.onClick.AddListener(Begging);
        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    public void Begging()
    {
        int currentChip = PlayerProfileManager.Instance.Profile.money;

        if (currentChip >= MinChip)
            return;

        int getChip = Random.Range(1, 4);

        int newChip = Mathf.Min(currentChip + getChip, MinChip);

        int realGetChip = newChip - currentChip;

        PlayerProfileManager.Instance.Profile.money = newChip;

        _getChipText.text = $"+{realGetChip}칩";
        _chipText.text = newChip.ToString();

        Refresh();
    }

    private void Refresh()
    {
        int currentChip = PlayerProfileManager.Instance.Profile.money;

        _beggingCan.SetActive(currentChip < MinChip);
    }
}