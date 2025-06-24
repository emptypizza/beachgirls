using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Hex 이동 설정")]
    public float moveSpeed = 5f;

    private bool isMoving = false;
    private Rigidbody2D rb;

    private Vector2Int hexPos = Vector2Int.zero; // 현재 육각 좌표
    private Vector3 targetPos;

    // Flat-Top 기준 육각형 방향 (E, NE, NW, W, SW, SE)
    private static readonly Vector2Int[] hexDirections = new Vector2Int[]
    {
        new Vector2Int(1, 0),    // E
        new Vector2Int(0, 1),    // NE
        new Vector2Int(-1, 1),   // NW
        new Vector2Int(-1, 0),   // W
        new Vector2Int(0, -1),   // SW
        new Vector2Int(1, -1)    // SE
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hexPos = WorldToHex(transform.position);
        transform.position = HexToWorld(hexPos); // 스냅
    }
    public void MoveToWorldPosition(Vector3 worldPos)
    {
        if (isMoving) return;

        Vector2Int targetHex = WorldToHex(worldPos);
        Vector3 targetWorld = HexToWorld(targetHex);

        StartCoroutine(MoveRoutine(targetWorld));
        hexPos = targetHex;
    }


    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.Keypad6)) TryMove(0); // E
        if (Input.GetKeyDown(KeyCode.Keypad9)) TryMove(1); // NE
        if (Input.GetKeyDown(KeyCode.Keypad7)) TryMove(2); // NW
        if (Input.GetKeyDown(KeyCode.Keypad4)) TryMove(3); // W
        if (Input.GetKeyDown(KeyCode.Keypad1)) TryMove(4); // SW
        if (Input.GetKeyDown(KeyCode.Keypad3)) TryMove(5); // SE
    }

    public void MoveByIndex(int dirIndex)
    {
        TryMove(dirIndex);
    }

    private void TryMove(int dirIndex)
    {
        if (isMoving || dirIndex < 0 || dirIndex > 5) return;

        Vector2Int nextHex = hexPos + hexDirections[dirIndex];
        Vector3 worldTarget = HexToWorld(nextHex);
        StartCoroutine(MoveRoutine(worldTarget));
        hexPos = nextHex;
    }

    private IEnumerator MoveRoutine(Vector3 destination)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, destination) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = destination;
        isMoving = false;
    }

    // 육각형 좌표 → 월드 좌표 변환 (Flat-Top 기준)
    private Vector3 HexToWorld(Vector2Int hex)
    {
        float width = 1f;
        float height = Mathf.Sqrt(3f) / 2f * width;

        float x = width * (3f / 4f * hex.x);
        float y = height * (hex.y + 0.5f * (hex.x & 1)); // 홀수열 보정

        return new Vector3(x, y, 0);
    }

    // 월드 좌표 → 육각형 좌표 변환
    private Vector2Int WorldToHex(Vector3 pos)
    {
        float width = 1f;
        float height = Mathf.Sqrt(3f) / 2f * width;

        int q = Mathf.RoundToInt(pos.x / (width * 0.75f));
        int r = Mathf.RoundToInt((pos.y - (q & 1) * height * 0.5f) / height);

        return new Vector2Int(q, r);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GridCell"))
        {
            var cell = collision.GetComponent<Cell>();
            cell?.ActivateCell();
        }
    }
}
