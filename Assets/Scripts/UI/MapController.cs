using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public RectTransform mapRect; // 지도 UI Rect Transform
    public RectTransform playerIcon; // 플레이어 아이콘 UI Rect Transform
    public PortalController portalController; // 포탈 컨트롤러

    // 게임 맵의 크기 (예를 들어, 가로 200, 세로 200)
    public Vector2 mapSize = new Vector2(200, 200);

    // 지역 정보 클래스
    [System.Serializable]
    public class RegionInfo
    {
        public string mapName; // 지역 이름
        public BoxCollider2D regionCollider; // 지역을 나타내는 박스 콜라이더
        public GameObject miniMap; // 해당 지역의 미니맵
        // 추가적인 지역 정보를 원하는대로 정의할 수 있음
    }

    public List<RegionInfo> regions = new List<RegionInfo>(); // 지역 정보 리스트
    public Text mapNameText; // 지도 이름을 표시할 UI 텍스트

    private string currentMapName = ""; // 현재 플레이어가 위치한 지역 이름

    void Update()
    {
        // 'M' 키를 눌렀을 때 맵을 보이거나 숨기도록 처리
        if (Input.GetKeyDown(KeyCode.M))
        {
            // 현재 맵이 보이는 상태면 맵을 숨김
            mapRect.gameObject.SetActive(!mapRect.gameObject.activeSelf);
        }

        // 맵이 보이는 상태일 때만 맵 위치 업데이트
        if (mapRect.gameObject.activeSelf)
        {
            // 플레이어의 위치를 게임 맵의 좌표로 변환
            Vector2 mapPosition = new Vector2(
                Mathf.Clamp(player.position.x, -mapSize.x / 2f, mapSize.x / 2f), // x 좌표 제한
                Mathf.Clamp(player.position.y, -mapSize.y / 2f, mapSize.y / 2f) // y 좌표 제한
            );

            // 게임 맵의 좌표를 UI 지도의 좌표로 변환
            Vector2 normalizedPosition = new Vector2(
                mapPosition.x / mapSize.x,
                mapPosition.y / mapSize.y
            );

            // UI 지도의 크기에 맞게 변환
            Vector2 mapRectSize = mapRect.rect.size;
            Vector2 mapPositionPixels = new Vector2(
                normalizedPosition.x * mapRectSize.x,
                normalizedPosition.y * mapRectSize.y
            );

            // UI 지도 내에서 플레이어 아이콘의 위치 조정
            playerIcon.anchoredPosition = mapPositionPixels;

            // 플레이어가 어떤 지역에 속해 있는지 확인하여 지도 이름 업데이트 및 미니맵 표시
            UpdateMapAndMiniMap();
        }
    }

    // 지도 이름 업데이트 및 미니맵 표시 함수
    void UpdateMapAndMiniMap()
    {
        bool isInAnyRegion = false; // 플레이어가 어떤 지역에 속해 있는지 여부

        // 플레이어가 어떤 지역에 속해 있는지 확인
        foreach (RegionInfo region in regions)
        {
            // 플레이어의 위치가 지역 내에 있는지 박스 콜라이더를 사용하여 확인
            if (region.regionCollider != null && region.regionCollider.bounds.Contains(player.position))
            {
                // 플레이어가 다른 지역으로 이동한 경우에만 지도 이름 및 미니맵을 업데이트
                if (currentMapName != region.mapName)
                {
                    // 지도 이름 업데이트
                    mapNameText.text = region.mapName;

                    // 미니맵 표시
                    ShowMiniMap(region.miniMap);

                    currentMapName = region.mapName; // 현재 지도 이름 업데이트
                }
                isInAnyRegion = true; // 플레이어가 어떤 지역에 속해 있음을 표시
            }
            else
            {
                // 플레이어가 이 지역에 속해 있지 않으면 미니맵을 숨김
                HideMiniMap(region.miniMap);
            }
        }

        // 플레이어가 어떤 지역에 속해 있지 않으면 지도 이름을 비우고 모든 미니맵을 숨김
        if (!isInAnyRegion)
        {
            mapNameText.text = "";
            currentMapName = ""; // 현재 지도 이름 초기화
        }
    }

    // 미니맵 표시 함수
    void ShowMiniMap(GameObject miniMap)
    {
        // 특정 미니맵을 보여줌
        if (miniMap != null)
        {
            miniMap.SetActive(true);
        }
    }

    // 특정 미니맵을 숨기는 함수
    void HideMiniMap(GameObject miniMap)
    {
        if (miniMap != null)
        {
            miniMap.SetActive(false);
        }
    }
}
