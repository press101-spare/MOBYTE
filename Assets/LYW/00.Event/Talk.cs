using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Talk : MonoBehaviour
{
    public TextMeshProUGUI talkText;
    public TextMeshProUGUI nameText;

    public string[] name;
    
    [TextArea]
    public string[] talk;

    public void Start()
    {
        A();
    }
    public void A()
    {
        StopAllCoroutines();
        StopCoroutine(Talking());
    }

    private IEnumerator Talking()
    {
        for (int i = 0; i < talk.Length; i++)
        {
            nameText.text = name[i];
            talkText.text = talk[i];

            talkText.maxVisibleCharacters = 0;
            talkText.ForceMeshUpdate();

            for (int j = 1; j <= talkText.textInfo.characterCount; j++)
            {
                talkText.maxVisibleCharacters += j;
                yield return new WaitForSecondsRealtime(0.05f);
            }

            yield return new WaitForSecondsRealtime(2f);
        }
    }
}
