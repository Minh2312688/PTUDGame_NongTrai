using UnityEngine;
using System.Collections;

public class EggPickup : MonoBehaviour
{
    private void OnMouseEnter()
    {
        CursorManager.instance.SetBao();
    }

    private void OnMouseExit()
    {
        CursorManager.instance.ResetCursor();
    }

    private void OnMouseDown()
    {
        StartCoroutine(PickupEgg());
    }

    IEnumerator PickupEgg()
    {
        // Tay nắm
        CursorManager.instance.SetBua();

        // Delay để thấy animation
        yield return new WaitForSeconds(0.10f);

        // Chuột bình thường
        CursorManager.instance.ResetCursor();

        // Xóa trứng
        Destroy(gameObject);
    }
}