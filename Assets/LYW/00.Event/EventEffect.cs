using UnityEngine;

/// <summary>
/// 선택지가 실행할 결과의 공통 규약입니다.
/// 각 이벤트 결과는 이 클래스를 상속하고 Apply만 구현합니다.
/// </summary>
public abstract class EventEffect : MonoBehaviour
{
    // 선택지를 눌렀을 때 각 효과가 실제 결과를 적용합니다.
    public abstract void Apply();

    // 모든 효과가 같은 방식으로 화면 결과 알림을 띄우도록 공통 처리합니다.
    protected void ShowResult(string message)
    {
        EventResultDisplay.Show(message, this);
    }
}
