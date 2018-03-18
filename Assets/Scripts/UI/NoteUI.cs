using System;
using Items;

public static class NoteUI
{
    public static string[] CurrentNoteContents;
    public static int CurrentNotePage;

    private static readonly EventHandler<EventArgs> _noteviewEventHandle = HideNote;

    public static void ViewNote()
    {
        if (Ui.CurrentItem is Note)
        {
            CurrentNoteContents = ((Note)Ui.CurrentItem).Contents;
            //Fix the new line character escape that unity performs.
            for (int i = 0; i < CurrentNoteContents.Length; i++)
            {
                CurrentNoteContents[i] = CurrentNoteContents[i].Replace("\\n", "\n");
            }
            if (CurrentNoteContents.Length > 0) UIMonoHelper.Instance.NoteText.text = CurrentNoteContents[0];
            CurrentNotePage = 0;
            UIMonoHelper.Instance.NoteNextButton.interactable = CurrentNoteContents.Length > 1;
            UIMonoHelper.Instance.NotePrevButton.interactable = false;
            UIMonoHelper.Instance.NoteParent.SetActive(true);
        }

        Ui.EscapeEvents.Add(_noteviewEventHandle);
        Ui.ExtraWindowEvents.Add(_noteviewEventHandle);
    }

    public static void HideNote(object sender, EventArgs e)
    {
        CurrentNoteContents = null;
        UIMonoHelper.Instance.NoteParent.SetActive(false);
        if(Ui.EscapeEvents.Contains(_noteviewEventHandle)) Ui.EscapeEvents.Remove(_noteviewEventHandle);
        if (Ui.ExtraWindowEvents.Contains(_noteviewEventHandle)) Ui.ExtraWindowEvents.Remove(_noteviewEventHandle);
    }

    public static void NextPage()
    {
        if (CurrentNotePage < CurrentNoteContents.Length - 1)
        {
            UIMonoHelper.Instance.NoteText.text = CurrentNoteContents[++CurrentNotePage];
            UIMonoHelper.Instance.NotePrevButton.interactable = true;
        }
        if (CurrentNotePage == CurrentNoteContents.Length - 1) UIMonoHelper.Instance.NoteNextButton.interactable = false;
    }

    public static void PreviousPage()
    {
        if (CurrentNotePage > 0)
        {
            UIMonoHelper.Instance.NoteText.text = CurrentNoteContents[--CurrentNotePage];
            UIMonoHelper.Instance.NoteNextButton.interactable = true;
        }
        if(CurrentNotePage == 0) UIMonoHelper.Instance.NotePrevButton.interactable = false;
    }
}

