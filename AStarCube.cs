using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    //this class is a work in progress and is not currently in use
    class AStarCube
    {
        public Cube cube;
        public float costSoFar;
        public float costToGo;
        public float totalCost;
        public Queue<int> rotationsQueue;

        public AStarCube(Cube cube, float costSoFar, float costToGo, Queue<int> rotationsQueue, int rotationToQueue)
        {
            this.cube = cube;
            this.costSoFar = costSoFar;
            this.costToGo = costToGo;

            this.rotationsQueue = rotationsQueue;
            if(rotationToQueue != -1)
                rotationsQueue.Enqueue(rotationToQueue);

            totalCost = costSoFar + costToGo;
        }
    }
}
