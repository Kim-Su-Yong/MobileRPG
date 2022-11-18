using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour
{
    [SerializeField]
    private RectTransform touchPad;
    private int _touchId = -1; // ���� ��ġ�� �ȴٸ� (0���� �����, ��ġ�� �ߴ��� ���ߴ��� �Ǵ��ϱ� ���ؼ� -1 �� ������)
    // bool �� ġ�� ó���� false �� �����ѰŶ� ����Ԥ�
    private Vector3 _startPos = Vector3.zero;
    public float _dragRadius = 50f; // ������
    private bool _buttonPress = false; // ������?
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
    void HandleTouchInput() // ����� ��ġ�� �Լ�
    {
        int i = 0;
        if(Input.touchCount > 0) // �� �� ��ġ�ߴ��� �ڵ����� ī��Ʈ
        {
            foreach(Touch _touch in Input.touches)
            {
                i++;
                Vector3 touchPos = new Vector3(_touch.position.x, _touch.position.y);
                if(_touch.phase == TouchPhase.Began) // ��ġ ���� = ��ġ�� ���۵Ǿ��ٸ�
                {
                    if(_touch.position.x <= (_startPos.x + _dragRadius)) // ��ġ�� ������ �ȿ� �����ִٸ�
                    {
                        _touchId = i;
                    }
                    if (_touch.position.y <= (_startPos.y + _dragRadius)) // ��ġ�� ������ �ȿ� �����ִٸ�
                    {
                        _touchId = i;
                    }
                }
                if(_touch.phase == TouchPhase.Moved || _touch.phase == TouchPhase.Stationary) // ��ġ�� �����̰ų� ������������
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
            if(diffVector.sqrMagnitude > _dragRadius * _dragRadius) // ��ġ ������ ���� ���̸� �Ѿ�ٸ�
            {
                diffVector.Normalize(); // ����ȭ �۾��� ���ľ� �������� ������
                touchPad.position = _startPos + diffVector * _dragRadius; // ���⸸ �����ϰ� �� ������ ������ �ʰ� �ϱ�
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
        // �Ÿ��� ���� ������ ���� ���������� ������ ������ ��������
        if(playerMove!=null)
        {
            playerMove.OnStickChange(normalDiff); // ������ ���ؼ� �ѱ��
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
