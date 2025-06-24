using UnityEngine;

public class UIcode : MonoBehaviour
{
    public Player player;

    void Start()
    {
        // 플레이어 자동 연결 시도 (혹시 수동 연결 안 되었을 경우 대비)
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.GetComponent<Player>();
        }
    }

    void Update()
    {
        if (player == null) return;

        // 마우스 클릭 or 모바일 탭 입력
        if (Input.GetMouseButtonDown(0))
        {
            // 화면 좌표를 월드 좌표로 변환
            Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorldPos.z = 0f;

            // 클릭한 위치로 플레이어 이동 시도
            player.MoveToWorldPosition(clickWorldPos);
        }
    }
}
