using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public GameObject pauseImg;
    public RectTransform pauseMenu;
    public GameObject player;
    public RectTransform soundMenu;
    public RectTransform screenMenu;
    void Awake()
    {
        pauseImg = GameObject.Find("Canvas-UI").transform.GetChild(2).gameObject;
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        pauseMenu = pauseImg.transform.GetChild(0).GetComponent<RectTransform>();
        soundMenu = pauseImg.transform.GetChild(1).GetComponent<RectTransform>();
        screenMenu = pauseImg.transform.GetChild(2).GetComponent<RectTransform>();
    }

    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKeyDown(KeyCode.Escape)) // 모바일에서는 뒤로가기 버튼이 Escape 키임
            {
                Pause();
            }
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        if(pauseImg.gameObject.activeInHierarchy == false) // 캔버스 안에 RectTransform 오브젝트 활성화 / 비활성화
        {
            if(pauseMenu.gameObject.activeInHierarchy == false)
            {
                pauseMenu.gameObject.SetActive(true);
            }
            pauseImg.gameObject.SetActive(true);
            Time.timeScale = 0f; // 게임 정지
            player.SetActive(false); // 플레이어 정지
        }
        else
        {
            pauseImg.gameObject.SetActive(false);
            screenMenu.gameObject.SetActive(false);
            soundMenu.gameObject.SetActive(false);
            Time.timeScale = 1f;
            player.SetActive(true);
        }
    }

    public void Sounds(bool isopen)
    {
        if(isopen == true)
        {
            soundMenu.gameObject.SetActive(true);
            pauseMenu.gameObject.SetActive(false);
        }
        if(!isopen)
        {
            soundMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
        }
    }
    
    public void ScreenSetting(bool isopen)
    {
        if (isopen == true)
        {
            screenMenu.gameObject.SetActive(true);
            soundMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(false);
        }
        if (!isopen)
        {
            screenMenu.gameObject.SetActive(false);
            soundMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
