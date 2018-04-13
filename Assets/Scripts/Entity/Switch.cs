using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour, IInteractable {

    public bool IsOn = false;
    public bool CanBeDeactivated = false;
    public float EventCallDelay = 2f;
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
            if (animator) animator.SetTrigger("Activated");
            else Invoke("Activated", EventCallDelay);
        }
        else if (CanBeDeactivated)
        {
            IsOn = false;
            if (animator) animator.SetTrigger("Deactivated");
            else Invoke("Deactivated", EventCallDelay);
        }
    }

    void Activated()
    {
        OnActivated.Invoke();
    }

    void Deactivated()
    {
        OnDeactivated.Invoke();
    }
}
