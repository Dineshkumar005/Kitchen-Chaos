using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player){

        //transfer KitchenObject between ClearCounter and Player
        if(!HasKitchenObject()){
            if(player.HasKitchenObject()){
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        } else {
            if(player.HasKitchenObject()){
				//player have something
				if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
					//player have a plate
					if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
						GetKitchenObject().DistorySelf();
					}
				} else {
					if(GetKitchenObject().TryGetPlate(out plateKitchenObject)){
						//counter have a plate
						if(plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())){
							player.GetKitchenObject().DistorySelf();
						}
					}
				}
            } else {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
