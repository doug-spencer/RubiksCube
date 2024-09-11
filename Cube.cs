using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube 
{
    class Cube
    {
        public int[,] cubeRepresentation = new int[6, 9]; //6 faces and nine squares on each face of cube represented in a 6 by 9 array
        public float[] stateReprestentation = new float[324]; //one hot encoded version of cubeRepresentation

        //cubeRepresentation is simpler and more intuitive to perform rotations on and stateRepresentation is needed for the DNN
        public Cube()
        {

            //initialises the cube array which is a 6 by 9 cube with numbers 1 to 6 representing colours
            // RED    1
            // GREEN  2
            // ORANGE 3
            // BLUE   4
            // WHITE  5
            // YELLOW 6
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cubeRepresentation[i, j] = i + 1;
                }
            }

            //converts the cube array to a 323 long 1d array with one hot encoded values
            // each tile is represented by 6 digits
            // RED    100000
            // GREEN  010000
            // ORANGE 001000
            // BLUE   000100
            // WHITE  000010
            // YELLOW 000001
            int x = 0;
            foreach (int i in cubeRepresentation)
            {
                for (int count = 0; count < 6; count++)
                {
                    stateReprestentation[x + count] = 0;
                    if (count + 1 == i)
                    {
                        stateReprestentation[x + count] = 1;
                    }
                }
                x += 6;
            }
        }

        public void ninetyCW(int f)//f is the face number that is being rotated
        {

            int x = 0;

            // RED    1
            // GREEN  2
            // ORANGE 3
            // BLUE   4
            // WHITE  5
            // YELLOW 6

            //if f > 6 this corresponds to an anticlockwise rotation and the program does this by repeating a clockwise rotation 3 times
            int repeat = 1;
            if (f > 6)
            {
                repeat = 3;
                f -= 6;
            }

            for (int i = 0; i < repeat; i++)
            {

                //changes the positions of corner and edge tiles on the face that is being rotated in a clockwise motion
                fourPieceChange(cubeRepresentation, 1, 3, 9, 7, f, f, f, f);
                fourPieceChange(cubeRepresentation, 2, 6, 8, 4, f, f, f, f);

                //changes the positions of corner and edge tiles adjacent to the face that is being rotated in a clockwise motion
                //different config needed for each face so case statement for each face
                switch (f)
                {
                    case 1://f
                        fourPieceChange(cubeRepresentation, 1, 3, 9, 7, 2, 5, 4, 6);
                        fourPieceChange(cubeRepresentation, 4, 2, 6, 8, 2, 5, 4, 6);
                        fourPieceChange(cubeRepresentation, 7, 1, 3, 9, 2, 5, 4, 6);
                        break;

                    case 2://r
                        fourPieceChange(cubeRepresentation, 1, 9, 9, 9, 3, 5, 1, 6);
                        fourPieceChange(cubeRepresentation, 4, 6, 6, 6, 3, 5, 1, 6);
                        fourPieceChange(cubeRepresentation, 7, 3, 3, 3, 3, 5, 1, 6);
                        break;

                    case 3://b
                        fourPieceChange(cubeRepresentation, 1, 7, 9, 3, 4, 5, 2, 6);
                        fourPieceChange(cubeRepresentation, 4, 8, 6, 2, 4, 5, 2, 6);
                        fourPieceChange(cubeRepresentation, 7, 9, 3, 1, 4, 5, 2, 6);
                        break;

                    case 4://l
                        fourPieceChange(cubeRepresentation, 1, 1, 9, 1, 1, 5, 3, 6);
                        fourPieceChange(cubeRepresentation, 4, 4, 6, 4, 1, 5, 3, 6);
                        fourPieceChange(cubeRepresentation, 7, 7, 3, 7, 1, 5, 3, 6);
                        break;

                    case 5://d
                        fourPieceChange(cubeRepresentation, 7, 7, 7, 7, 2, 3, 4, 1);
                        fourPieceChange(cubeRepresentation, 8, 8, 8, 8, 2, 3, 4, 1);
                        fourPieceChange(cubeRepresentation, 9, 9, 9, 9, 2, 3, 4, 1);
                        break;

                    case 6://u
                        fourPieceChange(cubeRepresentation, 3, 3, 3, 3, 2, 1, 4, 3);
                        fourPieceChange(cubeRepresentation, 2, 2, 2, 2, 2, 1, 4, 3);
                        fourPieceChange(cubeRepresentation, 1, 1, 1, 1, 2, 1, 4, 3);
                        break;
                    default:
                        break;
                }
            }


            //converts the cube array to a 323 long 1d array with one hot encoded values
            // each tile is represented by 6 digits
            // RED    100000
            // GREEN  010000
            // ORANGE 001000
            // BLUE   000100
            // WHITE  000010
            // YELLOW 000001
            
            x = 0;
            foreach (int i in cubeRepresentation)
            {
                for (int count = 0; count < 6; count++)
                {
                    stateReprestentation[x + count] = 0;
                    if (count + 1 == i)
                    {
                        stateReprestentation[x + count] = 1;
                    }
                }
                x += 6;
            }
        }


        //when a face is rotated each tile replaces another in a cycle of length 4
        //this method takes the index's and face's of 4 tiles and pushes each one into the slot of the next and the last one into the slot of the first
        public static void fourPieceChange(int[,] cubeRepresentation, int index1, int index2, int index3, int index4, int face1, int face2, int face3, int face4)
        {
            //indexing is from 0 so index and face must be decreased by 1 
            index1 -= 1;
            index2 -= 1;
            index3 -= 1;
            index4 -= 1;

            face1 -= 1;
            face2 -= 1;
            face3 -= 1;
            face4 -= 1;

            int temp = cubeRepresentation[face1, index1];
            cubeRepresentation[face1, index1] = cubeRepresentation[face4, index4];
            cubeRepresentation[face4, index4] = cubeRepresentation[face3, index3];
            cubeRepresentation[face3, index3] = cubeRepresentation[face2, index2];
            cubeRepresentation[face2, index2] = temp;
        }


        // useful for debugging
        //prints cube array
        public void printCubeRepresentation()
        {
            string str = "";
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    str += cubeRepresentation[i, j].ToString();
                }
                Console.WriteLine(str);
                str = "";
            }
        }

        //prints stateRepresentation array
        public void printStateRepresentation()
        {
            int b = 0;
            foreach (float i in stateReprestentation)
            {
                //adds a space to the print for every tile (6 digits)
                if (b % 6 == 0)
                    Console.WriteLine("\n");
                b += 1;
                Console.WriteLine(i.ToString());
            }
        }
    }
}
