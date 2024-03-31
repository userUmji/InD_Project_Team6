using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform g_playerTransform; // 플레이어의 Transform 컴포넌트를 가리키는 변수

    void Update()
    {
        if (g_playerTransform != null) // 플레이어 Transform이 유효한 경우에만 실행
        {
            // 카메라의 위치를 플레이어의 위치로 설정
            Vector3 newPosition = new Vector3(g_playerTransform.position.x, g_playerTransform.position.y, transform.position.z);
            transform.position = newPosition;
        }
    }
}
