using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    class DAVI
    {
        Random rnd = new Random();
        public Cube[] cubeArray;
        Cube cube;

        int[] layersInfo;
        float[] solvedState;
        public int batchSize;
        public int trainingIterations;
        public float learningRate;

        float[] y;
        public DNN myDNN;
        float hOfx;
        
        //variables for printing
        float[] cost = new float[1];
        string print = "";

        
        public DAVI(int[] layersInfo, float[] solvedState, int batchSize, int trainingIterations, float learningRate)
        {
            this.layersInfo = layersInfo;
            this.solvedState = solvedState;
            this.batchSize = batchSize;
            this.trainingIterations = trainingIterations;
            this.learningRate = learningRate;

            y = new float[batchSize];
            myDNN = new DNN(layersInfo, learningRate); //creates the DNN that is used throughout

        }


        //bulk of the DAVI algorithm
        //trains the DNN
        public void run()
        {

            for (int m = 0; m < trainingIterations; m++)
            {

                GetScrambledStates(batchSize);

                for (int j = 0; j < batchSize; j++)
                {
                    cube = cubeArray[j];
                    y[j] = 0;

                    for (int i = 1; i < 13; i++)//calculates hOfx for all 12 cubes that can be reached from this state
                    {
                        cube.ninetyCW(i);
                        hOfx = myDNN.FeedForward(cube.stateReprestentation)[0];

                        

                        if (compareToSolvedState(cube.stateReprestentation, solvedState) == true)//sets hofx to 0 if the solved state is discovered
                            hOfx = 0;


                        cube.ninetyCW(i);
                        cube.ninetyCW(i);
                        cube.ninetyCW(i);

                        if (hOfx < y[j] || i == 1)
                        {
                            y[j] = hOfx;//selects the lowest value of hOfx
                        }
                    }
                    y[j] += 100;//adds 100 to the distance to solved cube because the cube has been rotated once to make that cube
                    
                    //Console.WriteLine(j + ":" + (y[j]));
                    //Console.WriteLine();
                }
                //Console.ReadLine();


                cost[0] = 0;
                for (int j = 0; j < batchSize; j++)
                {
                    cube = cubeArray[j];
                    //Console.WriteLine(yi[0]);
                    cost[0] += myDNN.train(cube.stateReprestentation, new float[1] { y[j]});
                }

                //Console.WriteLine(learningRate + "  -----  " + cost[0]/(batchSize*10E6));




            }

        }

        //public void test(int sampleSize, int scrambles)
        //{
        //    GetScrambledStates(sampleSize);
        //    print = "";
        //    foreach (Cube testCube in cubeArray)
        //    {
        //        print += (myDNN.FeedForward(testCube.stateReprestentation)[0] + "   ");
        //    }
        //    Console.WriteLine(print);
        //}

        // choses the move that takes the given cube closest to being solved out of the 12 possible
        public Tuple<int, float> chooseMove(Cube givenCube, int lastMove)
        {
            float lowestCost = 10E6f;

            int chosenMove = 0;

            for (int i = 1; i < 13; i++)
            {
                givenCube.ninetyCW(i);
                hOfx = myDNN.FeedForward(givenCube.stateReprestentation)[0];//calculates the distance from solved each of the 12 cubes is

                if (compareToSolvedState(givenCube.stateReprestentation, solvedState) == true)
                    hOfx = 0;


                givenCube.ninetyCW(i);
                givenCube.ninetyCW(i);
                givenCube.ninetyCW(i);

                if ((lastMove < 7 & i != lastMove + 6) || (lastMove > 6 & i != lastMove - 6))
                {
                    if (hOfx < lowestCost || i == 1)
                    {
                        lowestCost = hOfx;//sets the lowest cost to hOfX is it is lowe than the current lowest and if it doesnt contradict
                        //the previous rotation made
                        chosenMove = i; //sets the move with the lowest cost to the current chosen move
                    }
                }

            }
            return Tuple.Create<int, float>(chosenMove, lowestCost);//returns the final choice of the chosen move
        }

        //work in progress - not currently called anywhere in the program
        public Queue<int> aStarSolve(Cube initalCube)
        {
            bool solvedFound = false;
            int moveCount = 0;
            AStarCube cubeToSolve = new AStarCube(initalCube, 0, myDNN.FeedForward(initalCube.stateReprestentation)[0], new Queue<int>(), -1);
            Cube currentCube;

            List<AStarCube> openCubes = new List<AStarCube>();
            openCubes.Add(cubeToSolve);


            while (!solvedFound & moveCount < 200)
            {
                moveCount += 1;
                openCubes.OrderBy(x => x.totalCost);
                currentCube = openCubes[0].cube;

                for (int i = 1; i < 13; i++)
                {
                    currentCube.ninetyCW(i);
                    openCubes.Add(new AStarCube(currentCube, openCubes[0].costSoFar + 100, myDNN.FeedForward(currentCube.stateReprestentation)[0], openCubes[0].rotationsQueue, i));
                    if (currentCube.stateReprestentation == solvedState)
                    {
                        solvedFound = true;
                    }
                    currentCube.ninetyCW(i);
                    currentCube.ninetyCW(i);
                    currentCube.ninetyCW(i);
                }
                openCubes.Remove(openCubes[0]);
            }
            openCubes.OrderBy(x => x.totalCost);
            return openCubes[0].rotationsQueue;
        }



        //initialises and returns a batch sized array of cubes that have all been scrambled once
        public Cube[] GetScrambledStates(int batchSize)
        {
            cubeArray = new Cube[batchSize];

            for (int i = 0; i < batchSize; i++)
            {
                cubeArray[i] = new Cube();
                for (int j = 0; j < i + 1; j++)//Math.Ceiling((decimal)(i / (batchSize / maxScrambles))); j++)
                {
                    cubeArray[i].ninetyCW(rnd.Next(1, 12));
                }
            }

            return cubeArray;

            
        }

        public bool compareToSolvedState(float[] item1, float[] item2)
        {
            bool x = true;

            for (int k = 0; k < item1.Length; k++)
            {
                if (item1[k] != item2[k])
                {
                    x = false;
                }
            }

            return x;
        }
    }
}
