using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RubiksCube
{
    class RotationItem
    {
        public float angle = MathHelper.PiOver2;
        public string axis;
        public int plane;
        public string name;
        public int direction;//0 for acw 1 for cw

        public RotationItem(string name, string axis, int plane, int direction)
        {
            this.name = name;
            this.axis = axis;
            this.plane = plane;
            this.direction = direction;

        }
    }
}
