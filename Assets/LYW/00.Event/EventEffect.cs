using UnityEngine;

/// <summary>
/// 선택지가 실행할 결과의 공통 규약입니다.
/// 각 이벤트 결과는 이 클래스를 상속하고 Apply만 구현합니다.
/// </summary>
public abstract class EventEffect : MonoBehaviour
{
    public abstract void Apply();

    protected void ShowResult(string message)
    {
        EventResultDisplay.Show(message, this);
    }
}
