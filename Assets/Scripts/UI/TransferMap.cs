using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransferMap : MonoBehaviour
{
    public Vector3 targetPosition; // 이동할 위치 좌표

    public Image blackImage; // 검정 사진을 불러오기 위한 이미지

    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    private bool imageActive = false; // 사진이 활성화되었는지 여부를 저장하는 변수

    // 박스 콜라이더에 닿는 순간 이벤트 발생
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 위치 이동
        collision.gameObject.transform.position = targetPosition;

        // 검은 사진을 즉시 활성화하고 서서히 사라지는 코루틴 시작
        StartCoroutine(FadeOutImage());
    }

    // 검은 사진을 즉시 활성화하고 서서히 사라지게 하는 코루틴
    IEnumerator FadeOutImage()
    {
        // 이미지가 이미 활성화되어 있는 경우 중복해서 처리하지 않도록 합니다.
        if (imageActive)
            yield break;

        // 검은 사진을 활성화합니다.
        blackImage.gameObject.SetActive(true);
        imageActive = true;

        // 알파 값을 서서히 낮춰 페이드 아웃 효과 생성
        for (float i = 1; i >= 0; i -= Time.deltaTime / fadeTime)
        {
            // 검은 사진의 알파 값을 조절하여 사라지게 합니다.
            blackImage.color = new Color(0, 0, 0, i);
            yield return null;
        }

        // 사라지는 효과가 끝난 후 검은 사진을 비활성화하여 화면에서 숨깁니다.
        blackImage.gameObject.SetActive(false);
        imageActive = false;
    }
}
