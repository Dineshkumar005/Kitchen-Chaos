using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemlapte;

    private void Awake() {
        recipeTemlapte.gameObject.SetActive(false);
    }

    private void Start() {
        DeliveryManager.Instance.OnReciepeSpawnded += DeliveryManager_OnReciepeSpawnded;
        DeliveryManager.Instance.OnReciepeCompleted += DeliveryManager_OnReciepeCompleted;

        UpdateVisual();
    }

    private void DeliveryManager_OnReciepeCompleted(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void DeliveryManager_OnReciepeSpawnded(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        
        foreach(Transform child in container) {
            if (child == recipeTemlapte) continue;
            Destroy(child.gameObject);
        }

        foreach(RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList()) {
            Transform recipeTransform = Instantiate(recipeTemlapte, container);
            recipeTransform.gameObject.SetActive(true);
            recipeTransform.GetComponent<DeliverManagerSingleUI>().SetRecipeSO(recipeSO);
        }
    }
}
