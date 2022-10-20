using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public Transform InteractionPoint;
    public LayerMask InteractionLayer;
    public float InteractionPointRadius = 1f;
    public bool IsInteracting { get; private set; }
    private PlayerController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);

        // for chest interaction check default key 'E', checks if valid chest nearby
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null)
                {
                    StartInteraction(interactable);
                    HotbarSelectorManager.instance.ActiveInventoryChecker();
                    GameManager.instance.isInventory = true;
                }
            }
        }
    }

    void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        IsInteracting = true;
    }

    void EndInteraction()
    {
        IsInteracting = false;
    }
}
