using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Note : Item
    {
        public string[] Contents;

        public Note()
        {
            Contents = new []{"(This note is illegible)"};
            Name = "Note";
            Equipable = false;
        }

        public Note(string name, string description, string[] contents)
        {
            Name = name;
            Description = description;
            Contents = contents;
        }
    }
}