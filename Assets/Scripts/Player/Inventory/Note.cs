using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Note : Item
    {
        public string[] Contents;
        public new bool Equipable = false;

        public Note()
        {
            Contents = new []{"(This note is illegible)"};
            Name = "Note";
        }

        public Note(string name, string description, string[] contents)
        {
            Name = name;
            Description = description;
            Contents = contents;
        }
    }
}