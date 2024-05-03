using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform[] portals; // 포탈들의 Transform 배열

    // 이 메서드는 버튼에서 호출될 것입니다.
    public void MovePlayerToPortal(int portalIndex)
    {
        // 지정된 인덱스에 해당하는 포탈 위치로 플레이어를 이동시킴
        if (portalIndex >= 0 && portalIndex < portals.Length)
        {
            // 'Player' 태그를 가진 게임 오브젝트를 찾음
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                // 플레이어의 위치를 지정된 포탈의 위치로 이동시킴
                playerObject.transform.position = portals[portalIndex].position;
            }
        }
        else
        {
            // 잘못된 포탈 인덱스에 대한 오류 메시지 출력
            Debug.LogError("Invalid portal index: " + portalIndex);
        }
    }
}
