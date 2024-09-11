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
    class MenuItem
    {
        public int drawX, drawY, width, height;
        public string itemText;
        public bool hoveredOver = false;
        public bool selected = false, button = false;
        public Vector2 drawTextTo;


        public MenuItem(int drawX, int drawY, int width, int height, string itemText)
        {
            this.drawX = drawX;
            this.drawY = drawY;
            this.width = width;
            this.height = height;
            this.itemText = itemText;     
            
            drawTextTo.X = drawTextTo.X = drawX + width / 2 - (int)Math.Round(itemText.Length * 3.5, 0);
            drawTextTo.Y = drawY + height / 2 - 8;
        }
    }
}
