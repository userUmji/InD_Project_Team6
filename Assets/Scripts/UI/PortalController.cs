using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform[] portals; // ������ Transform �迭
    public MapController mapController; // MapController ����
    public GameObject optionImage; // �ɼ� �̹��� GameObject

    public float activationDistance = 2f; // �÷��̾ ���а��� Ȱ��ȭ �Ÿ�

    bool optionImageActive = false; // �ɼ� �̹����� Ȱ��ȭ ����

    void Update()
    {
        // �÷��̾ ���� ��ó�� �ְ� �����̽��ٸ� ������ ��
        if (IsPlayerNearPortal() && Input.GetKeyDown(KeyCode.Space))
        {
            optionImageActive = !optionImageActive; // �� ���� ����

            // GameManager�� ���¸� PAUSE�� ����
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetGameState(GameManager.GameState.PAUSE);
            }

            // �� ��Ʈ�ѷ� ��Ȱ��ȭ
            ToggleMapController(false);

            // �ɼ� �̹��� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
            if (optionImage != null)
            {
                optionImage.gameObject.SetActive(optionImageActive);
            }

            
            // ��Ż�� Ȱ��ȭ�ϸ� ���� ü�� ȸ��
            if (optionImageActive)
            {
                for (int i = 0; i < GameManager.Instance.m_UnitManager.CheckUnitAmount(); i++)
                    GameManager.Instance.m_UnitManager.g_PlayerUnits[i].GetComponent<UnitEntity>().Heal(100000);
            }

            // GameManager�� ���¸� �ٽ� INPROGRESS�� ����
            if (!optionImageActive && GameManager.Instance != null)
            {
                GameManager.Instance.SetGameState(GameManager.GameState.INPROGRESS);
            }
        }
    }


    // �÷��̾ ���� ��ó�� �ִ��� Ȯ���ϴ� �޼���
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

    // ���з� �÷��̾ �̵���Ű�� �޼���
    public void MovePlayerToPortal(int portalIndex)
    {
        if (portalIndex >= 0 && portalIndex < portals.Length)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                playerObject.transform.position = portals[portalIndex].position;
            }
        }
        else
        {
            Debug.LogError("Invalid portal index: " + portalIndex);
        }
    }

    // UI ��ư���� ȣ���� �Լ�
    public void ToggleMapController(bool mapActive)
    {
        if (mapController != null)
        {
            mapController.gameObject.SetActive(mapActive);
        }
    }
}
