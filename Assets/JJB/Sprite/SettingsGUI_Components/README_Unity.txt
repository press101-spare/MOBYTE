카지노 픽셀 설정 GUI — 개별 부속품
=================================

이 폴더의 PNG 파일은 원본 GUI 시트에서 부속품별로 분리한 파일입니다.
모든 부속품은 서로 다른 PNG이며 밝은 체크무늬는 실제 투명 배경으로 제거했습니다.
버튼 글자는 포함하지 않았으므로 Unity의 TextMeshPro로 올려 사용하세요.

파일 구성
---------
- panel_settings_9slice.png : 설정창 본체
- header_9slice.png : 제목 영역
- button_normal_9slice.png : 기본 버튼
- button_selected_9slice.png : 선택된 버튼
- button_pressed_9slice.png : 눌린 버튼
- icon_button_normal.png : 기본 카테고리 아이콘 칸
- icon_button_selected.png : 선택된 카테고리 아이콘 칸
- button_close.png : 닫기 버튼
- slider_track.png : 슬라이더 선
- slider_knob.png : 슬라이더 손잡이
- checkbox_off.png / checkbox_on.png : 체크박스
- toggle_off.png / toggle_on.png : 토글
- icon_account.png : 계정 아이콘
- icon_sound.png : 사운드 아이콘
- icon_display.png : 화면 아이콘
- icon_game.png : 게임 아이콘
- icon_notification.png : 알림 아이콘
- action_burgundy_9slice.png : 붉은 동작 버튼
- action_gold_9slice.png : 금색 동작 버튼
- arrow_left.png / arrow_right.png : 좌우 화살표
- dropdown_full_9slice.png : 드롭다운 전체
- dropdown_arrow.png : 드롭다운 화살표

Unity 가져오기
-------------
1. PNG 폴더를 Unity의 Assets/UI/Settings 폴더로 옮깁니다.
2. Texture Type: Sprite (2D and UI)
3. Sprite Mode: Single
4. Filter Mode: Point (no filter)
5. Compression: None
6. Alpha Is Transparency: On
7. Wrap Mode: Clamp

9-Slice 설정
------------
이름에 _9slice가 붙은 파일은 Sprite Editor에서 다음 Border 값을 권장합니다.

- panel_settings_9slice: Left 24, Right 24, Top 24, Bottom 24
- header_9slice: Left 20, Right 20, Top 20, Bottom 20
- button_normal/selected/pressed: Left 20, Right 20, Top 20, Bottom 20
- action_burgundy/action_gold: Left 20, Right 20, Top 20, Bottom 20
- dropdown_full_9slice: Left 20, Right 72, Top 20, Bottom 20

사용할 Image 컴포넌트에서 Image Type을 Sliced로 바꾸면 크기를 늘려도 모서리가 유지됩니다.

주의
----
- SettingsGUI_Components_Preview.png는 확인용 이미지이며 게임에 넣을 파일이 아닙니다.
- 실제 게임에는 PNG 폴더 안의 개별 파일만 넣으면 됩니다.

