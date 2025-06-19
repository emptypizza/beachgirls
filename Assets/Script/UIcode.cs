using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIcode : MonoBehaviour
{
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();

        TryBindButton("UpButton", () => player.MoveUp());
        TryBindButton("DownButton", () => player.MoveDown());
        TryBindButton("LeftButton", () => player.MoveLeft());
        TryBindButton("RightButton", () => player.MoveRight());
    }

    private void TryBindButton(string name, UnityEngine.Events.UnityAction action)
    {
        GameObject btnObj = GameObject.Find(name);
        if (btnObj == null)
        {
            Debug.LogWarning($"{name} not found.");
            return;
        }

        Button btn = btnObj.GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogWarning($"{name} is missing Button component.");
            return;
        }

        btn.onClick.AddListener(action);
    }
}
