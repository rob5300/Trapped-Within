using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public Task TaskToAssign;

    public abstract void OnPuzzleBegin();
    public abstract void OnPuzzleSolved();
}
