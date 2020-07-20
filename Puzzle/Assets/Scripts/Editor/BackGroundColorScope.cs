using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CherryHill.Utility
{
    public class BackGroundColorScope : GUI.Scope
    {
        private readonly Color color;
        public BackGroundColorScope(Color color)
        {
            this.color = GUI.backgroundColor;
            GUI.backgroundColor = color;
        }


        protected override void CloseScope()
        {
            GUI.backgroundColor = color;
        }
    }

}