using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public RectTransform mapRect; // 지도 UI Rect Transform
    public RectTransform playerIcon; // 플레이어 아이콘 UI Rect Transform
    public BoxCollider2D regionCollider; // 현재 지역의 박스 콜라이더
    public RectTransform potalIcon;

    // 미니맵의 범위
    public float miniMapMinX = -30f;
    public float miniMapMaxX = 30f;
    public float miniMapMinY = -30f;
    public float miniMapMaxY = -70f;

    void Update()
    {
        if (regionCollider != null)
        {
            // 현재 지역의 박스 콜라이더를 기준으로 플레이어의 위치를 조정
            Vector2 mapPosition = new Vector2(
                Mathf.Clamp(player.position.x, regionCollider.bounds.min.x, regionCollider.bounds.max.x),
                Mathf.Clamp(player.position.y, regionCollider.bounds.min.y, regionCollider.bounds.max.y)
            );

            // 게임 맵의 좌표를 UI 지도의 좌표로 변환
            Vector2 normalizedPosition = new Vector2(
                Mathf.InverseLerp(regionCollider.bounds.min.x, regionCollider.bounds.max.x, mapPosition.x),
                Mathf.InverseLerp(regionCollider.bounds.min.y, regionCollider.bounds.max.y, mapPosition.y)
            );

            // UI 지도의 크기에 맞게 변환
            Vector2 mapRectSize = mapRect.rect.size;
            Vector2 mapPositionPixels = new Vector2(
                Mathf.Lerp(miniMapMinX, miniMapMaxX, normalizedPosition.x),
                Mathf.Lerp(miniMapMinY, miniMapMaxY, 1f - normalizedPosition.y) // 여기서 수정
            );

            // UI 지도 내에서 플레이어 아이콘의 위치 조정
            playerIcon.anchoredPosition = mapPositionPixels;
        }
    }
}
