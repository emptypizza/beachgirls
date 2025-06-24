

using UnityEngine;

public class UIcode : MonoBehaviour
{
    public Player player;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.GetComponent<Player>();
        }
    }

    void Update()
    {
        if (player == null || GameManager.Instance == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickWorldPos.z = 0f;

            Vector2Int targetHex = player.WorldToHex(clickWorldPos);
            Vector3 targetWorld = player.HexToWorld(targetHex);

            // GameManager가 관리하는 셀이 존재하면만 이동
            if (GameManager.Instance.IsCellExists(targetHex))
            {
                player.MoveToWorldPosition(targetWorld);
            }
        }
    }
}
