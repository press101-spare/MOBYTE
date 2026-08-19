using UnityEngine;

public class OnPanel_HTY : MonoBehaviour
{
    [SerializeField] private int _gameNumber;
    [SerializeField] private TestMiniGame_Test _testOnpanel;
    public void OnPanel()
    {
        _testOnpanel.Game(_gameNumber);
    }
}
