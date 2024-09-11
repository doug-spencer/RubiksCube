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
    class Menu
    {
        public List<MenuItem> menuItemList = new List<MenuItem>();

        public Texture2D highligtedBox;
        public Texture2D box;
        Vector2 drawTextTo;
        bool withinBoxBoundary = false;
        bool mousePressed = false;
        int indexOfSelectedItem;

        public void update(MouseState mouseState)
        {
            foreach (MenuItem item in menuItemList)
            {
                item.hoveredOver = false;
                withinBoxBoundary = false;

                if (mouseState.Position.X - item.drawX < item.width & 0 < mouseState.Position.X - item.drawX &
                    mouseState.Position.Y - item.drawY < item.height & 0 < mouseState.Position.Y - item.drawY)
                {
                    withinBoxBoundary = true;
                }

                if (withinBoxBoundary == true)
                {
                    item.hoveredOver = true;
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        mousePressed = true;
                    }
                }

                if (withinBoxBoundary == true & mousePressed == true & mouseState.LeftButton == ButtonState.Released)
                {
                    mousePressed = false;
                    indexOfSelectedItem = menuItemList.FindIndex(x => x.selected == true);
                    item.selected = true;
                    
                    if (indexOfSelectedItem != -1)
                        menuItemList[indexOfSelectedItem].selected = false;
                        
                }
                // selection of boxes and highlighting of boxes logic
            }
        }

        

        public void draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            foreach(MenuItem item in menuItemList)
            {
                //selects which menu box texture to draw to the screen depending on whether it is selected or hovered over
                if (item.hoveredOver == true || item.selected == true)
                {
                    spriteBatch.Draw(highligtedBox, new Rectangle(item.drawX, item.drawY, item.width, item.height), Color.White);
                }
                else
                {
                    spriteBatch.Draw(box, new Rectangle(item.drawX, item.drawY, item.width, item.height), Color.White);
                }

                
                //draws the text of each menu item to the screen
                spriteBatch.DrawString(font, item.itemText, item.drawTextTo, Color.White);               
            }
        }
    }
}
