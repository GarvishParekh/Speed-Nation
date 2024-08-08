using System.Collections.Generic;
using UnityEngine;

public class DestroyedVehicleController : MonoBehaviour
{
    [SerializeField] private List<DestroyedVehicleParts> parts = new List<DestroyedVehicleParts>();

    public void OnSpawn()
    {
        foreach (DestroyedVehicleParts part in parts)
        {
            part.ResetPart();
        }

        Invoke(nameof(DisableObject), 1f);
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
