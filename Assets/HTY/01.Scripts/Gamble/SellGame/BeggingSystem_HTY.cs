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
       
        PlayerProfileManager.Instance.Profile.money += 50;

        _getChipText.text = $"+{50}칩";
        _chipText.text = PlayerProfileManager.Instance.Profile.money.ToString();

        Refresh();
    }

    private void Refresh()
    {
        int currentChip = PlayerProfileManager.Instance.Profile.money;

        _beggingCan.SetActive(currentChip < MinChip);
    }
}