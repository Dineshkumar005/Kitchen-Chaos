using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {

	public static EventHandler OnAnyCut;

	new public static void ResetStaticData() {
		OnAnyCut = null;
    }

	public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
	public event EventHandler OnCut;

	[SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;


	private int cuttingProgress;

	public override void Interact(Player player) {
		if (!HasKitchenObject()) {
			if (player.HasKitchenObject()) {
				if (HasRecipeWithInut(player.GetKitchenObject().GetKitchenObjectSO())) {
					player.GetKitchenObject().SetKitchenObjectParent(this);
					cuttingProgress = 0;

					CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

					OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
						progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttinProgressMax
					});
				}
			}
			else {
				//player not carrying anything
			}
		}
		else {
			if (player.HasKitchenObject()) {
				if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){
					if(plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())){
						GetKitchenObject().DistorySelf();
					}
				}
			}
			else {
				GetKitchenObject().SetKitchenObjectParent(player);
			}
		}
	}

	public override void InteractAlternate(Player player) {
		if (HasKitchenObject() && HasRecipeWithInut(GetKitchenObject().GetKitchenObjectSO())) {
			cuttingProgress++;

			OnCut?.Invoke(this, EventArgs.Empty);
			OnAnyCut?.Invoke(this, EventArgs.Empty);

			CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

			OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
				progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttinProgressMax
			});


			if (cuttingProgress >= cuttingRecipeSO.cuttinProgressMax) {
				KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
				GetKitchenObject().DistorySelf();

				KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
			}
		}
	}

	private bool HasRecipeWithInut(KitchenObjectSO inputKitchenObjectSO) {
		CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
		return cuttingRecipeSO != null;
	}

	private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
		CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
		if (cuttingRecipeSO != null)
			return cuttingRecipeSO.output;
		return null;
	}

	private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
		foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
			if (cuttingRecipeSO.input == inputKitchenObjectSO)
				return cuttingRecipeSO;
		}
		return null;
	}
}
