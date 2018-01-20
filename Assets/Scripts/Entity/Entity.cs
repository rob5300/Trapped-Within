using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class Entity : MonoBehaviour
    {
        public string Name;

        public void Start()
        {
            if (gameObject.isStatic)
            {
                Debug.LogWarning("Entities should not be static! This may be saved for no reason.");
            }
        }
    }
}
