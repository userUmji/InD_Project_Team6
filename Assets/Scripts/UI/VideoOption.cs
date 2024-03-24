using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    // 전체 화면 모드를 저장할 변수
    FullScreenMode screenMode;

    // 해상도 설정을 위한 드롭다운 UI를 참조할 변수
    public Dropdown resolutionDropdown;

    // 전체 화면 토글 버튼을 참조할 변수
    public Toggle fullscreenBtn;

    // 가능한 해상도 목록을 저장할 리스트
    List<Resolution> resolutions = new List<Resolution>();

    // 선택된 해상도의 인덱스를 저장할 변수
    public int resolutionNum;

    void Start()
    {
        // UI 초기화 함수 호출
        InitUI();
    }

    // UI를 초기화하는 함수
    void InitUI()
    {
        // 화면 해상도 중 주사율이 60인 것만 추가
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        // 드롭다운 옵션 초기화
        resolutionDropdown.options.Clear();

        int optionNum = 0;
        // 가능한 해상도 목록을 드롭다운에 추가하고 현재 해상도에 해당하는 옵션을 선택
        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
        // 드롭다운 값을 갱신
        resolutionDropdown.RefreshShownValue();

        // 전체 화면 토글 버튼 초기화
        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    // 드롭다운 옵션이 변경되었을 때 호출되는 함수
    public void DropboxOptionChange(int x)
    {
        // 선택된 해상도의 인덱스를 저장
        resolutionNum = x;
    }

    // 전체 화면 토글 버튼이 변경되었을 때 호출되는 함수
    public void FullScreenBtn(bool isFull)
    {
        // 전체 화면 모드를 설정
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    // 확인 버튼이 클릭되었을 때 호출되는 함수
    public void OkBtnClick()
    {
        // 선택된 해상도와 전체 화면 모드로 화면 설정 변경
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
    }
}
