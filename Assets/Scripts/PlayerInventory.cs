using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInventory : MonoBehaviour
{
    private Inventory inventory;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inventory = FindFirstObjectByType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Aucun inventaire trouvé dans la scène !");
        }
    }

    private void OnEnable()
    {
        playerInput.actions.Enable();
        playerInput.actions["NextSlot"].performed += ctx => inventory.NextSlot();
        playerInput.actions["PreviousSlot"].performed += ctx => inventory.PreviousSlot();

        playerInput.actions["SelectSlot1"].performed += ctx => inventory.SelectItem(0);
        playerInput.actions["SelectSlot2"].performed += ctx => inventory.SelectItem(1);
        playerInput.actions["SelectSlot3"].performed += ctx => inventory.SelectItem(2);
        playerInput.actions["SelectSlot4"].performed += ctx => inventory.SelectItem(3);
    }

    private void OnDisable()
    {
        playerInput.actions.Disable();
        playerInput.actions["NextSlot"].performed -= ctx => inventory.NextSlot();
        playerInput.actions["PreviousSlot"].performed -= ctx => inventory.PreviousSlot();
    }
}
