using UnityEngine;

public class CropHoverManager : MonoBehaviour
{
    private void Update()
    {
        Vector2 mousePos =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit =
            Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            Crop crop =
                hit.collider.GetComponent<Crop>();

            if (crop != null)
            {
                float progress =
                    crop.GetGrowthPercent();

                float remain =
                    crop.GetRemainingTime();

                string water =
                    crop.IsWatered()
                    ? "💧 Watered"
                    : "❗ Needs Water";

                CropTooltipUI.Instance.Show(
                    crop.gameObject.name.Replace("(Clone)", "") +
                    "\n" +
                    Mathf.RoundToInt(progress) + "%" +
                    "\n" +
                    remain.ToString("0") + "s remaining" +
                    "\n" +
                    water
                );

                return;
            }
        }

        CropTooltipUI.Instance.Hide();
    }
}
