using UnityEngine;
using UnityEngine.Events;

public class InteractInvoker : MonoBehaviour, IInteractable {

    public bool Interactable {
        get {
            return interactable;
        }

        set {
            interactable = value;
        }
    }
    [SerializeField]
    private bool interactable = true;
    public UnityEvent InteractEvent;

    public void OnInteract(Player player)
    {
        InteractEvent.Invoke();
    }
}
