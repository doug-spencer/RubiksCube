using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


namespace RubiksCube
{
    class VisualCube
    {
        Random rnd = new Random();

        int Mousex = 0;
        int Mousey = 0;
        bool mousePressed = false;
        float mouseControlDamper = MathHelper.Pi / 300;

        public Vector3 CubeRotation = new Vector3(0, 0, 0);
        Queue<RotationItem> rotationsQueue = new Queue<RotationItem>();
        List<RotationItem> rotationsList = new List<RotationItem>();
        int rotationVelocity = 2;
        string moves = "";

        Menu subMenu = new Menu();


        int qbCount = -1;
        cubie[] qb = new cubie[27];

        Cube myCube = new Cube();
        Cube solvedCube = new Cube();

        DAVI myDAVI;
        int[] layers = new int[3] { 324, 5, 1 };

        int maxScrambles = 5;//10,5
        int batchSize;
        int trainingIterations = 5000;//8000
        float learningRate = 10E-6f;

        Stopwatch stopwatch = new Stopwatch();


        public VisualCube(Menu subMenu)
        {
            this.batchSize = this.maxScrambles;
            this.subMenu = subMenu;
            float[] solvedState = solvedCube.stateReprestentation;
            myDAVI = new DAVI(layers, solvedState, batchSize, trainingIterations, learningRate);
            myDAVI.run();
            stopwatch.Start();

            //testingObjs();
        }

        public void testingObjs()//testing
        {


            

            //DNN testDNN = new DNN(new int[3] { 2, 4, 1 }, 0.04f);

            //Console.WriteLine(testDNN.FeedForward(new float[2] { 2, 4 })[0].ToString());//test 3

            ////test 4
            //for (int i = 0; i < 1000; i++)
            //{
            //    testDNN.train(new float[2] { 0, 0 }, new float[1] { 0 });
            //    testDNN.train(new float[2] { 1, 0 }, new float[1] { 1 });
            //    testDNN.train(new float[2] { 0, 1 }, new float[1] { 1 });
            //    testDNN.train(new float[2] { 1, 1 }, new float[1] { 0 });
            //}
            //Console.WriteLine(testDNN.FeedForward(new float[2] { 0, 0 })[0].ToString());
            //Console.WriteLine(testDNN.FeedForward(new float[2] { 1, 0 })[0].ToString());
            //Console.WriteLine(testDNN.FeedForward(new float[2] { 0, 1 })[0].ToString());
            //Console.WriteLine(testDNN.FeedForward(new float[2] { 0, 0 })[0].ToString());



            //solvedCube.printCubeRepresentation(); //test 5
            //solvedCube.printStateRepresentation(); //test 6

            solvedCube.ninetyCW(1);//test 7
            solvedCube.printCubeRepresentation();
            solvedCube.printStateRepresentation();
        }

        public void Update(KeyboardState previousKey, KeyboardState currentKey, MouseState mouseState)
        {
            mouseState = Mouse.GetState();
             if (subMenu.menuItemList[0].selected == true)
            {
                Scramble();
                subMenu.menuItemList[0].selected = false;
            }
            if (subMenu.menuItemList[1].selected == true)
            {
                Solve();
                subMenu.menuItemList[1].selected = false;
            }
            if (subMenu.menuItemList[2].selected == true)
            {
                resetQBs();
                subMenu.menuItemList[2].selected = false;

                rotationsQueue.Clear();
                myCube = new Cube();
                for (int i = 0; i < 27; i++)
                {
                    qb[i].round();
                }
            }


            subMenu.update(mouseState);

            if (currentKey.IsKeyDown(Keys.G) & previousKey.IsKeyUp(Keys.G))
            {
                rotationsQueue.Enqueue(rotationsList[rotationsList.FindIndex(x => x.name == "r")]);
                myCube.ninetyCW(2);
            }
            if (currentKey.IsKeyDown(Keys.B) & previousKey.IsKeyUp(Keys.B))
            {
                rotationsQueue.Enqueue(rotationsList[rotationsList.FindIndex(x => x.name == "l")]);
                myCube.ninetyCW(4);
            }
            if (currentKey.IsKeyDown(Keys.O) & previousKey.IsKeyUp(Keys.O))
            {
                rotationsQueue.Enqueue(rotationsList[rotationsList.FindIndex(x => x.name == "u")]);
                myCube.ninetyCW(6);
            }
            if (currentKey.IsKeyDown(Keys.R) & previousKey.IsKeyUp(Keys.R))
            {
                rotationsQueue.Enqueue(rotationsList[rotationsList.FindIndex(x => x.name == "d")]);
                myCube.ninetyCW(5);
            }
            if (currentKey.IsKeyDown(Keys.Y) & previousKey.IsKeyUp(Keys.Y))
            {
                rotationsQueue.Enqueue(rotationsList[rotationsList.FindIndex(x => x.name == "f")]);
                myCube.ninetyCW(1);
            }
            if (currentKey.IsKeyDown(Keys.W) & previousKey.IsKeyUp(Keys.W))
            {
                rotationsQueue.Enqueue(rotationsList[rotationsList.FindIndex(x => x.name == "b")]);
                myCube.ninetyCW(3);
            }




            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (mousePressed == true)
                {
                    mousePressed = false;
                    CubeRotation.X += (mouseState.Position.Y - Mousey) * mouseControlDamper;
                    CubeRotation.Y += (mouseState.Position.X - Mousex) * mouseControlDamper;
                }
                else
                {
                    Mousex = mouseState.Position.X;
                    Mousey = mouseState.Position.Y;
                    mousePressed = true;
                }
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                mousePressed = false;
            }



            //responsible for the rotations queue
            //complex
            if (rotationsQueue.Count != 0)
            {
                RotationItem frontOfQueue = rotationsQueue.Peek();//selects the first item in the queue

                for (int i = 0; i < 27; i++)
                {
                    qb[i].Update(frontOfQueue, rotationVelocity);//updates each cubie with part of the rotation
                }

                frontOfQueue.angle -= MathHelper.ToRadians(rotationVelocity);//takes the angle just rotated through off the angle left

                if (frontOfQueue.angle < 0)//if that angle is less than 0
                {
                    frontOfQueue.angle = MathHelper.PiOver2;//resets the angle
                    rotationsQueue.Dequeue();//dequeues that rotation
                    for (int i = 0; i < 27; i++)//rounds the position of each cubie to maintatin accuracy over the course of many rotations
                    {
                        qb[i].round();
                    }
                }
            }

        }
        
        public void Draw(GraphicsDeviceManager graphics, SpriteBatch spriteBatch, SpriteFont font)
        {
            for (int i = 0; i < 27; i++)
            { 
                qb[i].Draw(graphics, CubeRotation);
            }
            spriteBatch.Begin();
            subMenu.draw(spriteBatch, font);
            spriteBatch.DrawString(font, moves, new Vector2(200, 25), Color.White);
            spriteBatch.End();
        }

        public void Scramble()
        {
            int chosenScrambleNber;
            for (int i = 0; i < maxScrambles; i++)
            {
                chosenScrambleNber = rnd.Next(0, 11);
                rotationsQueue.Enqueue(rotationsList[chosenScrambleNber]);
                myCube.ninetyCW(chosenScrambleNber + 1);
            }
            

        }
        public void Solve()
        {
            int lastMove = -1;
            Tuple<int, float> result = Tuple.Create<int, float>(1, 1);
            for (int i = 0; i < 20; i++) // while(result.item2 != 0)
            {
                result = myDAVI.chooseMove(myCube, lastMove);
                lastMove = result.Item1;
                myCube.ninetyCW(result.Item1);
                rotationsQueue.Enqueue(rotationsList[(result.Item1) - 1]);
                if (result.Item2 == 0)
                    break;
            }

            //work in progress - not used in current program
            //Queue<int> movvvess;
            //movvvess = myDAVI.aStarSolve(myCube);
            //while(movvvess.Count != 0)
            //{
            //    rotationsQueue.Enqueue(rotationsList[(movvvess.Peek()) - 1]);
            //    movvvess.Dequeue();
            //}
            
        }



        public void createQBs(Model model)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        qbCount += 1;
                        qb[qbCount] = new cubie(model, new Vector3(2 * i, 2 * j, 2 * k));
                        // doubles the value so that the cubes dont overlap
                    }
                }
            }
        }

        public void resetQBs()
        {
            qbCount = -1;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        qbCount += 1;
                        qb[qbCount].drawPosition = new Vector3(2 * i, 2 * j, 2 * k);
                        qb[qbCount].drawOrientation = Matrix.Identity;
                    }
                }
            }
        }



        public void createRotations()
        {
            rotationsList.Add(new RotationItem("f", "z", 2, -1));
            rotationsList.Add(new RotationItem("r", "x", 2, -1));
            rotationsList.Add(new RotationItem("b", "z", -2, 1));
            rotationsList.Add(new RotationItem("l", "x", -2, 1));
            rotationsList.Add(new RotationItem("d", "y", -2, 1));
            rotationsList.Add(new RotationItem("u", "y", 2, -1));

            rotationsList.Add(new RotationItem("f'", "z", 2, 1));
            rotationsList.Add(new RotationItem("r'", "x", 2, 1));
            rotationsList.Add(new RotationItem("b'", "z", -2, -1));
            rotationsList.Add(new RotationItem("l'", "x", -2, -1));
            rotationsList.Add(new RotationItem("d'", "y", -2, -1));
            rotationsList.Add(new RotationItem("u'", "y", 2, 1));
        }

        public static void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("Raised: {0}", e.SignalTime);
        }

        class cubie
        {
            public Model model;
            public Vector3 drawPosition;
            public Matrix drawOrientation = Matrix.Identity;
            private int d = 0; //direction (cw or acw)

            public cubie(Model _model, Vector3 _drawPosition)
            {
                model = _model;
                drawPosition = _drawPosition;
            }

            
            //complex algorithm
            //alters the position and orientation of each cubie based on the rotation that is in progress
            public void Update(RotationItem currentRotation, int rotationVelocity)
            {
                d = currentRotation.direction;
                switch (currentRotation.axis)//selects by the plane in which the rotation operates
                { 
                    case "x":
                        if (drawPosition.X == currentRotation.plane)// selects the cubies which lie in that plane
                        {
                            drawPosition = MatVecMul(Matrix.CreateRotationX(MathHelper.ToRadians(d * rotationVelocity)), drawPosition);//uses linear algebra to alter the position
                            drawOrientation *= Matrix.CreateRotationX(MathHelper.ToRadians(d * rotationVelocity));//uses linear algebra to alter the orientation
                        }
                        break;

                    case "y":
                        if (drawPosition.Y == currentRotation.plane)
                        {
                            drawPosition = MatVecMul(Matrix.CreateRotationY(MathHelper.ToRadians(d * rotationVelocity)), drawPosition);
                            drawOrientation *= Matrix.CreateRotationY(MathHelper.ToRadians(d * rotationVelocity));
                        }
                        break;

                    case "z":
                        if (drawPosition.Z == currentRotation.plane)
                        {
                            drawPosition = MatVecMul(Matrix.CreateRotationZ(MathHelper.ToRadians(d * rotationVelocity)), drawPosition);
                            drawOrientation *= Matrix.CreateRotationZ(MathHelper.ToRadians(d * rotationVelocity));
                        }
                        break;

                    case "Y":
                        drawPosition = MatVecMul(Matrix.CreateRotationY(MathHelper.ToRadians(d * rotationVelocity)), drawPosition);
                        drawOrientation *= Matrix.CreateRotationY(MathHelper.ToRadians(d * rotationVelocity));
                        break;

                    case "X":
                        drawPosition = MatVecMul(Matrix.CreateRotationX(MathHelper.ToRadians(d * rotationVelocity)), drawPosition);
                        drawOrientation *= Matrix.CreateRotationX(MathHelper.ToRadians(d * rotationVelocity));
                        break;


                }


            }

            public void Draw(GraphicsDeviceManager graphics, Vector3 CubeRotation)
            {
                foreach (var mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {


                        // rotation, translation, scaling

                        effect.World = drawOrientation;
                        effect.World *= Matrix.CreateTranslation(drawPosition);

                        //transforms the 3d world by a rotation matrix with an angle created from the values in CubeRotation

                        effect.World *= Matrix.CreateRotationX(CubeRotation.X);
                        effect.World *= Matrix.CreateRotationY(CubeRotation.Y);
                        effect.World *= Matrix.CreateRotationZ(CubeRotation.Z);

                        

                    }

                    mesh.Draw();
                }
            }

            public void round()
            {
                drawPosition.X = (int)Math.Round(drawPosition.X, 0);
                drawPosition.Y = (int)Math.Round(drawPosition.Y, 0);
                drawPosition.Z = (int)Math.Round(drawPosition.Z, 0);
            }

            public static Vector3 MatVecMul(Matrix mat, Vector3 vec)
            {
                Vector3 newVec;

                newVec.X = mat.M11 * vec.X +
                        mat.M21 * vec.Y +
                        mat.M31 * vec.Z;

                newVec.Y = mat.M12 * vec.X +
                        mat.M22 * vec.Y +
                        mat.M32 * vec.Z;

                newVec.Z = mat.M13 * vec.X +
                        mat.M23 * vec.Y +
                        mat.M33 * vec.Z;

                return newVec;
            }
        }

    }
}



