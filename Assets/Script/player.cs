using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("이동 설정")]
    [Tooltip("한 번에 이동할 그리드 크기")]
    public float moveDistance = 1f;
    [Tooltip("초당 이동 속도")]
    public float moveSpeed = 5f;

    // 이동 상태 플래그
    private bool isMoving = false;
    private Vector3 targetPos;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPos = transform.position;
    }

    void Update()
    {
        // 이동 중이면 키 입력 무시
        if (isMoving) return;

        Vector2 dir = Vector2.zero;

        // 키보드 입력 체크
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            dir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            dir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            dir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            dir = Vector2.right;

        if (dir != Vector2.zero)
            TryMove(dir);
    }

    // UI 버튼에서 호출될 공개 메서드들
    public void MoveUp() => TryMove(Vector2.up);
    public void MoveDown() => TryMove(Vector2.down);
    public void MoveLeft() => TryMove(Vector2.left);
    public void MoveRight() => TryMove(Vector2.right);

    // 실제로 이동 시작
    private void TryMove(Vector2 dir)
    {
        if (isMoving) return;           // 이미 이동 중이면 무시
        if (dir == Vector2.zero) return;

        Vector3 dest = transform.position + (Vector3)dir * moveDistance;
        // (선택) 여기에 Raycast2D나 Bounds 체크하여 이동 가능 여부 검사 가능
        StartCoroutine(MoveRoutine(dest));
    }

    // 부드러운 슬라이딩 코루틴
    private IEnumerator MoveRoutine(Vector3 destination)
    {
        isMoving = true;
        targetPos = destination;

        while (Vector3.Distance(transform.position, targetPos) > 0.001f)
        {
            // Rigidbody2D.MovePosition 버전 사용하고 싶으면 아래 주석 해제
            // rb.MovePosition(Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.deltaTime));

            // 간단히 Transform 위치 조정
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        // 정확히 딱 맞춰 놓고 이동 종료
        transform.position = targetPos;
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 그리드 셀 태그가 붙은 오브젝트와 부딪혔을 때
        if (collision.CompareTag("GridCell"))
        {
            var cell = collision.GetComponent<Cell>();
            cell?.ActivateCell();
        }
    }
}
