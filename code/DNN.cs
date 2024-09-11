using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube
{
    class DNN
    {
        int[] layersInfo;
        Layer[] layers;


        //the constructor takes in a layersInfo and a learning rate
        //layers info refers to an array of set size which matches the number of layers and values...
        //...that represent the number of neurons in each layer
        public DNN(int[] layersInfo, float learningRate)
        {
            this.layersInfo = layersInfo;
            layers = new Layer[layersInfo.Length - 1];

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new Layer(layersInfo[i], layersInfo[i + 1], learningRate);
            }

        }

        //train is responsible for sending the expected and observed value and running the algorighm that updates the weights accordingly
        public float train(float[] inputs, float[] expectedOutput)
        {
            float cost = 0;
            float[] recievedOutput = new float[1];
            recievedOutput = FeedForward(inputs);
            cost = exampleError(recievedOutput, expectedOutput);
            BackPropogation(expectedOutput);
            return cost;
        }


        //example error computes (observed - expected)^2 and was useful in understanding the network but is not used in the final program
        public float exampleError(float[] observedOutput, float[] expectedOutput)
        {
            float addcost = 0;
            for (int i = 0; i < observedOutput.Length; i++)
            {
                addcost += (observedOutput[i] - expectedOutput[i]) * (observedOutput[i] - expectedOutput[i]);
            }
            return addcost;
        }

        //takes an input and feeds it forward one layer
        //then takes that output as a new input and feeds it through the next layer
        //repeating until the output from the final layer is found and then returns it
        public float[] FeedForward(float[] inputs)
        {
            layers[0].FeedForwardLayer(inputs);
            for (int i = 1; i < layers.Length; i++)
            {
                layers[i].FeedForwardLayer(layers[i - 1].outputs);
            }

            return layers[layers.Length - 1].outputs;
        }


        //iterates backwards through the layers running the method for backpropogation until the first layer is reached
        //runs updateWeights method for each layer
        public void BackPropogation(float[] expectedOutput)
        {
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                if (i == layers.Length - 1)
                {
                    layers[i].BackPropOutput(expectedOutput);
                }
                else
                {
                    layers[i].BackPropHidden(layers[i + 1].gamma, layers[i + 1].weights);
                }
            }

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].UpdateWeights();
            }
        }

        //Where the hard computation of the neural network algortithm happens
        class Layer
        {

            float learningRate;

            int numOfInputs;
            int numOfOutputs;

            public float[] inputs;
            public float[] beforeActivation;
            public float[] outputs;
            

            public float[,] weights;
            public float[,] weightsDelta;

            public float[] gamma;
            public float[] error;

            public static Random rnd = new Random();

            public Layer(int numOfInputs, int numOfOutputs, float learningRate)
            {
                this.learningRate = learningRate;
                this.numOfInputs = numOfInputs;
                this.numOfOutputs = numOfOutputs;

                inputs = new float[numOfInputs];
                beforeActivation = new float[numOfOutputs];
                outputs = new float[numOfOutputs];

                weights = new float[numOfOutputs, numOfInputs];
                weightsDelta = new float[numOfOutputs, numOfInputs];

                gamma = new float[numOfOutputs];
                error = new float[numOfOutputs];

                initialiseWeights();

                //testRelu();//test 2

            }

            //populates each value of the weights matrix with a random double between 0 and 1
            public void initialiseWeights()
            {
                for (int i = 0; i < numOfOutputs; i++)
                {
                    for (int j = 0; j < numOfInputs; j++)
                    {
                        weights[i, j] = (float)rnd.NextDouble();

                    }
                }
            }

            
            public float[] FeedForwardLayer(float[] inputs)
            {
                this.inputs = inputs;
                for (int i = 0; i < numOfOutputs; i++)
                {
                    beforeActivation[i] = 0;
                    for (int j = 0; j < numOfInputs; j++)
                    {
                        beforeActivation[i] += (inputs[j]) * weights[i, j];
                    }

                    outputs[i] = activation(beforeActivation[i]);
                }
                
                return outputs;
            }

            public float activation(float val)
            {
                //tanh activation function commented out below
                //return (float)Math.Tanh(val);
                //return (float)(1.0 / (1.0 + Math.Exp(-(val-0.5))));


                //relu activation function
                if (val < 0)
                    return 0;
                else
                    return val;
            }
            public float activationDer(float val)
            {
                //derivative of tanh activation function commented out below
                //return (float)(1 - (Math.Tanh(val) * Math.Tanh(val)));
                //return (activation(val) * (1 - activation(val)));

                //derivative of relu function
                if (val < 0)
                    return 0;
                else
                    return 1;
            }



            public void BackPropOutput(float[] expected)
            {
                //calculates gamma as the rate of change of the mean squared error with respect to before activation
                for (int i = 0; i < numOfOutputs; i++)
                {
                    error[i] = 2 / numOfOutputs * (outputs[i] - expected[i]);
                    gamma[i] = error[i] * activationDer(beforeActivation[i]);
                }

                //calculates each value of weights delta to become the rate of change of the mse with respect to...
                //...the correspornding value of the weights matrix
                for (int i = 0; i < numOfOutputs; i++)
                {
                    for (int j = 0; j < numOfInputs; j++)
                    {
                        weightsDelta[i, j] = gamma[i] * inputs[j];
                    }
                }
            }

            //there is no mse calculated in the feed forward of the hidden layers so a second simpler method can be used
            public void BackPropHidden(float[] gammaForward, float[,] weightsForward)
            {
                for (int i = 0; i < numOfOutputs; i++)
                {
                    gamma[i] = 0;

                    //calculates gamma as the rate of change of the mse with respect to the output of that layer before activation
                    for (int j = 0; j < gammaForward.Length; j++)
                    {
                        gamma[i] += gammaForward[j] * weightsForward[j, i];
                    }
                    gamma[i] *= activationDer(beforeActivation[i]);


                    //calculates each value of weights delta to become the rate of change of the mse with respect to...
                    //...the correspornding value of the weights matrix
                    for (int j = 0; j < numOfInputs; j++)
                    {
                        weightsDelta[i, j] = gamma[i] * inputs[j];
                    }
                }


            }

            //each value in the weights matrix is updated by taking off its corresponding value in the...
            //...in the weights delta matrix multiplied by the small learning rate
            public void UpdateWeights()
            {
                for (int i = 0; i < numOfOutputs; i++)
                {
                    for (int j = 0; j < numOfInputs; j++)
                    {
                        weights[i, j] = weights[i, j];//test 5
                        weights[i, j] -= weightsDelta[i, j] * learningRate;
                    }
                }
            }

        }
    }
}






