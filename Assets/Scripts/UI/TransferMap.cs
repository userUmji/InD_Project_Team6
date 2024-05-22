using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    public Vector3 targetPosition; // �̵��� ��ġ ��ǥ

    // �ڽ� �ݶ��̴��� ��� ���� �̺�Ʈ �߻�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾� ��ġ �̵�
            collision.gameObject.transform.position = targetPosition;
        }
    }
}
