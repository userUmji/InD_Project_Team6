using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform[] portals; // 포털의 Transform 배열
    public MapController mapController; // MapController 참조
    public GameObject optionImage; // 옵션 이미지 GameObject

    public float activationDistance = 2f; // 플레이어가 포털과의 활성화 거리

    bool optionImageActive = false; // 옵션 이미지의 활성화 여부

    void Update()
    {
        // 플레이어가 포털 근처에 있고 스페이스바를 눌렀을 때
        if (IsPlayerNearPortal() && Input.GetKeyDown(KeyCode.Space))
        {
            if (optionImage.activeSelf == false)
            {
                optionImage.SetActive(true);
                // GameManager의 상태를 PAUSE로 설정
                GameManager.Instance.SetGameState(GameManager.GameState.PORTAL);


                // 맵 컨트롤러 비활성화
                ToggleMapController(false);

                // 옵션 이미지 활성화 또는 비활성화
                if (GameManager.Instance.GetInventoryGO().transform.localScale == new Vector3(1, 1, 1))
                    GameManager.Instance.GetInventoryGO().transform.localScale = new Vector3(0, 0, 1);

                for (int i = 0; i < GameManager.Instance.m_UnitManager.CheckUnitAmount(); i++)
                {
                    GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().ResetUnit();
                }

                
            }
            else if (optionImage.activeSelf == true)
            {
                optionImage.SetActive(false);

                // GameManager의 상태를 다시 INPROGRESS로 설정 
                GameManager.Instance.SetGameState(GameManager.GameState.INPROGRESS);
                
            }
        }
    }


    // 플레이어가 포털 근처에 있는지 확인하는 메서드
    bool IsPlayerNearPortal()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            foreach (Transform portal in portals)
            {
                if (Vector3.Distance(playerObject.transform.position, portal.position) <= activationDistance)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // 포털로 플레이어를 이동시키는 메서드
    public void MovePlayerToPortal(int portalIndex)
    {
        if (portalIndex >= 0 && portalIndex < portals.Length)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerObject.transform.position = portals[portalIndex].position;
                ToggleMapController(false);
                optionImage.SetActive(false);
                GameManager.Instance.g_GameState = GameManager.GameState.INPROGRESS;
            }
        }
        else
        {
            Debug.LogError("Invalid portal index: " + portalIndex);
        }
    }

    // UI 버튼에서 호출할 함수
    public void ToggleMapController(bool mapActive)
    {
        if (mapController != null)
        {
            mapController.gameObject.SetActive(mapActive);
        }
    }

    public void ShowDic()
    {
        GameManager.Instance.GetDictionaryGO().SetActive(true);
        optionImage.SetActive(false);
    }

}
