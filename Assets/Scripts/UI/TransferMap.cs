using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    public Vector3 targetPosition; // �̵��� ��ġ ��ǥ


    // �ڽ� �ݶ��̴��� ��� ���� �̺�Ʈ �߻�
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        // �÷��̾� ��ġ �̵�
        collision.gameObject.transform.position = targetPosition;

    }
}
