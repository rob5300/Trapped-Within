using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IInteractable {

    public bool IsOn = false;
    public bool CanBeDeactivated = false;
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

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
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnInteract(Player player)
    {
        if (!IsOn)
        {
            IsOn = true;
            OnActivated.Invoke();
            if (animator) animator.SetTrigger("Activated");
        }
        else if (CanBeDeactivated)
        {
            IsOn = false;
            OnDeactivated.Invoke();
            if (animator) animator.SetTrigger("Deactivated");
        }
    }
}
