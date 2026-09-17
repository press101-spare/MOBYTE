using TMPro;                  // TextMeshPro UI를 사용하기 위해 불러옴
using UnityEngine;            // Unity 기본 기능을 사용하기 위해 불러옴

public class Talk : MonoBehaviour   // Talk라는 스크립트 생성
{
    public TextMeshProUGUI nameText;   // 캐릭터 이름을 표시할 텍스트

    public TextMeshProUGUI talkText;   // 캐릭터 대사를 표시할 텍스트

    public string[] names;             // 캐릭터 이름들을 저장하는 배열

    [TextArea]                         // Inspector에서 대사를 여러 줄로 입력 가능하게 함
    public string[] talks;             // 대사들을 저장하는 배열

    public float speed = 0.05f;        // 글자 하나가 출력되는 속도

    int talkIndex = 0;                 // 현재 몇 번째 대사인지 저장

    int charIndex = 0;                 // 현재 몇 번째 글자까지 출력했는지 저장

    float timer = 0f;                  // 글자 출력 시간을 계산하는 변수


    void Start()
    {
        ShowTalk();                    // 게임 시작 시 첫 번째 대사를 준비
    }


    void Update()
    {
        // 모든 대사가 끝났으면 아래 코드를 실행하지 않음
        if (talkIndex >= talks.Length)
            return;


        // 현재 대사의 모든 글자가 출력됐으면 실행하지 않음
        if (charIndex >= talks[talkIndex].Length)
            return;


        // 프레임마다 시간을 계속 더함
        timer += Time.deltaTime;


        // timer가 speed보다 커졌을 때
        if (timer >= speed)
        {
            // 현재 대사의 글자를 하나씩 추가해서 출력
            talkText.text += talks[talkIndex][charIndex];


            // 다음 글자로 이동
            charIndex++;


            // 타이머를 다시 0으로 초기화
            timer = 0f;
        }
    }


    void ShowTalk()
    {
        // 대사 배열 범위를 넘어갔다면 실행하지 않음
        if (talkIndex >= talks.Length)
            return;


        // 이전에 출력된 대사를 지움
        talkText.text = "";


        // 글자 순서를 처음부터 시작
        charIndex = 0;


        // 글자 출력 시간도 초기화
        timer = 0f;


        // 이름 배열 안에 현재 번호가 존재하는지 확인
        if (talkIndex < names.Length)
        {
            // 현재 대사의 캐릭터 이름을 표시
            nameText.text = names[talkIndex];
        }
    }


    // 버튼 OnClick에 연결할 함수
    public void Skip()
    {
        // 모든 대사가 끝났다면 버튼을 눌러도 실행하지 않음
        if (talkIndex >= talks.Length)
            return;


        // 현재 대사가 아직 출력 중이라면
        if (charIndex < talks[talkIndex].Length)
        {
            // 현재 문장을 한 번에 전부 출력
            talkText.text = talks[talkIndex];


            // 글자가 모두 출력된 상태로 변경
            charIndex = talks[talkIndex].Length;
        }
        else
        {
            // 현재 대사가 이미 전부 출력됐다면
            // 다음 대사 번호로 이동
            talkIndex++;


            // 아직 다음 대사가 존재한다면
            if (talkIndex < talks.Length)
            {
                // 다음 대사를 준비
                ShowTalk();
            }
            else
            {
                // 모든 대사가 끝났으면 대화창의 글자를 지움
                talkText.text = "";


                // 캐릭터 이름도 지움
                nameText.text = "";
            }
        }
    }
}