using UnityEngine;
using UnityEngine.UI;

public class EndWindowExit : MonoBehaviour, IInteractable
{
    public Text EndText;

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

    public void OnInteract(Player player)
    {
        EndText.gameObject.SetActive(false);
        player.movement.DoMovement = false;
        player.movement.DoRotation = false;
        Ui.EndGame();
        Destroy(this);
    }
}
