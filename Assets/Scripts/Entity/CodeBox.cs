using System;
using UnityEngine;

namespace Entity
{
    public class CodeBox : Box
    {
        public int LockCode = 123;
        public DialLock[] Locks;

        public new void Start()
        {
            foreach (DialLock dlock in Locks)
            {
                dlock.box = this;
            }
            base.Start();
        }

        public int GetInputtedCode()
        {
            string codeToReturn = "";
            foreach (DialLock dlock in Locks)
            {
                codeToReturn += dlock.CurrentDigit.ToString();
            }
            return Convert.ToInt32(codeToReturn);
        }

        public void CheckCode()
        {
            int code = GetInputtedCode();
            if(IsLocked && code == LockCode)
            {
                IsLocked = false;
                Open();
            }
        }
    }
}