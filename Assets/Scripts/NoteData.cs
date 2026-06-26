using UnityEngine;

[System.Serializable]
public class NoteData
{
    public string title;
    [TextArea(5, 10)] public string content;

    public Sprite noteImage;
}
