using UnityEngine;

public class TestMiniGame_Test : MonoBehaviour
{
    [SerializeField] private GameObject[] _gamePanel;
    

    public void Game(int _num)
    {
        _gamePanel[_num].SetActive(true);
    }
}
