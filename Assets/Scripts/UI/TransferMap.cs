using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    public Vector3 targetPosition; // 이동할 위치 좌표


    // 박스 콜라이더에 닿는 순간 이벤트 발생
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        // 플레이어 위치 이동
        collision.gameObject.transform.position = targetPosition;

    }
}
