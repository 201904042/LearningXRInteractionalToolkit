using static Enums;
using UnityEngine;

public class GunUIManager : MonoBehaviour
{
    public GunUI ui;

    public void SetUIActive(bool tf)
    {
        if (ui == null)
            return;
        ui.gameObject.SetActive(tf);
    }

    public void UpdateUI(GunState state, int bulletCount)
    {
        switch (state)
        {
            case GunState.NoMag: ui.SetText("NoMag"); break;
            case GunState.NoSlide: ui.SetText("Unloaded"); break;
            case GunState.NoAmmo: ui.SetText("reload"); break;
            case GunState.Ready:
            case GunState.Delay: ui.SetText(bulletCount); break;
            default: ui.SetText("~"); break;
        }
    }
}
