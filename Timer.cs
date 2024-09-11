using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace RubiksCube
{
    class Timer
    {
        public bool start = false;
        public bool spaceHeldDown = false;
        Stopwatch timer = new Stopwatch();


        //logic of the timer
        public string update(KeyboardState previousKey, KeyboardState currentKey)
        {
            if (currentKey.IsKeyDown(Keys.Space) & previousKey.IsKeyUp(Keys.Space))
            {
                if (spaceHeldDown == true)
                {
                    spaceHeldDown = false;
                }
                else
                {
                    timer.Restart();
                    start = true;
                    timer.Start(); 
                }
            }
            else if (start == true & currentKey.IsKeyDown(Keys.Space))
            {
                start = false;
                spaceHeldDown = true;
                timer.Stop();
            }


            TimeSpan timeTaken = timer.Elapsed;

            return timeTaken.ToString(@"m\:ss\.fff");            
        }


    }
}