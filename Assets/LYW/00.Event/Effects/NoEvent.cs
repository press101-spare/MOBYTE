using UnityEngine;

public class NoEvent : EventEffect
{
    public override void Apply()
    {
        string message =
            $"아쉬움을 뒤로하고 지나쳤다.";

        Debug.Log(message, this);
        ShowResult(message);
    }
}
