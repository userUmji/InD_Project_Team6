using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmJi_Map_Spawn : MonoBehaviour
{
    public GameObject door;
    // �ڽ� �ݶ��̴��� ��� ���� �̺�Ʈ �߻�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            collision.gameObject.transform.position = door.transform.GetChild(0).transform.position;


        }
    }
}
