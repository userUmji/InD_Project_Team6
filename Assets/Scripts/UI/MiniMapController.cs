using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public RectTransform mapRect; // 지도 UI Rect Transform
    public RectTransform playerIcon; // 플레이어 아이콘 UI Rect Transform

    // 게임 맵의 크기 (예를 들어, 가로 100, 세로 100)
    public Vector2 mapSize = new Vector2(100, 100);

    void Update()
    {
        // 플레이어의 위치를 게임 맵의 좌표로 변환
        Vector2 mapPosition = new Vector2(
            Mathf.Clamp(player.position.x, -mapSize.x, mapSize.x), // x 좌표 제한
            Mathf.Clamp(player.position.y, -mapSize.y, mapSize.y) // y 좌표 제한
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
    }


}