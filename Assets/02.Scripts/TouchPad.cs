using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour
{
    [SerializeField]
    private RectTransform touchPad;
    private int _touchId = -1; // 만약 터치가 된다면 (0부터 양수값, 터치를 했는지 안했는지 판단하기 위해서 -1 로 시작함)
    // bool 로 치면 처음에 false 로 선언한거랑 비슷함ㅋ
    private Vector3 _startPos = Vector3.zero;
    public float _dragRadius = 50f; // 반지름
    private bool _buttonPress = false; // 눌렀냐?
    [SerializeField]
    private PlayerMove playerMove;
    void Start()
    {
        touchPad = GetComponent<RectTransform>();
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        _startPos = touchPad.position;
    }
    public void ButtonDown()
    {
        _buttonPress = true;
    }
    public void ButtonUp()
    {
        _buttonPress = false;
        HandleInput(_startPos);
    }
    void HandleTouchInput() // 모바일 터치용 함수
    {
        int i = 0;
        if(Input.touchCount > 0) // 몇 번 터치했는지 자동으로 카운트
        {
            foreach(Touch _touch in Input.touches)
            {
                i++;
                Vector3 touchPos = new Vector3(_touch.position.x, _touch.position.y);
                if(_touch.phase == TouchPhase.Began) // 터치 유형 = 터치가 시작되었다면
                {
                    if(_touch.position.x <= (_startPos.x + _dragRadius)) // 터치가 반지름 안에 들어와있다면
                    {
                        _touchId = i;
                    }
                    if (_touch.position.y <= (_startPos.y + _dragRadius)) // 터치가 반지름 안에 들어와있다면
                    {
                        _touchId = i;
                    }
                }
                if(_touch.phase == TouchPhase.Moved || _touch.phase == TouchPhase.Stationary) // 터치가 움직이거나 멈춰져있으면
                {
                    if(_touchId == i)
                    {
                        HandleInput(touchPos);
                    }
                }
                if(_touch.phase == TouchPhase.Ended)
                {
                    if(_touchId == i)
                    {
                        _touchId = -1;
                    }
                }
            }
        }
    }
    void HandleInput(Vector3 pos)
    {
        if(_buttonPress)
        {
            Vector3 diffVector = (pos - _startPos);
            if(diffVector.sqrMagnitude > _dragRadius * _dragRadius) // 터치 지점이 원의 넓이를 넘어갔다면
            {
                diffVector.Normalize(); // 정규화 작업을 거쳐야 방향으로 움직임
                touchPad.position = _startPos + diffVector * _dragRadius; // 방향만 유지하고 원 밖으로 나가지 않게 하기
                Debug.Log(_startPos + diffVector);
            }
            else
            {
                touchPad.position = pos;
            }
        }
        else
        {
            touchPad.position = _startPos;
        }
        Vector3 diff = touchPad.position - _startPos;
        Vector3 normalDiff = new Vector3(diff.x / _dragRadius, diff.y / _dragRadius, diff.z / _dragRadius);
        // 거리를 구한 값에서 원의 반지름으로 나누면 방향이 구해진다
        if(playerMove!=null)
        {
            playerMove.OnStickChange(normalDiff); // 방향을 구해서 넘긴다
        }
    }
    private void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            HandleTouchInput();
        }
        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            HandleInput(Input.mousePosition);
        }
    }
}
