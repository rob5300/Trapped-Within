using Items;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Box : Entity.Entity, IItemInteract, IInteractable {

    public bool Interactable { get { return _interactable; } set { _interactable = value; } }
    [SerializeField]
    private bool _interactable;

    public string KeyId;
    public bool IsLocked = true;
    public bool IsOpen = false;

    private Animator animator;

    public void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public bool OnItemInteract(Item item, Player player)
    {
        if (item is Key)
        {
            Key k = (Key) item;
            if (k.KeyId == KeyId)
            {
                if (IsLocked) Unlock();
                Open();
                return true;
            }
        }
        return false;
    }

    public void OnInteract(Player player)
    {
        if (!IsLocked)
        {
            if (IsOpen) Close();
            else Open();
        }
    }

    protected void Unlock()
    {
        IsLocked = false;
        //Play a sound, animate ect.
    }

    public void Open()
    {
        if (!IsLocked && animator)
        {
            animator.SetTrigger("Open");
            IsOpen = true;
        }
    }

    public void Close()
    {
        if (!IsLocked && animator)
        {
            animator.SetTrigger("Close");
            IsOpen = false;
        }
    }
}
