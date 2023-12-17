using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {

        if (player.HasKitchenObject()) {
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                if (plateKitchenObject.TryAddIngredient(kitchenObjectSO)) {
                    OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);

                }
            }

        } else {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
