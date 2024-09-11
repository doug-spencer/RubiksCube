<h1 align="center">
  <br>
  <img src="https://github.com/doug-spencer/RubiksCube/blob/main/Content/CubeGif.gif" alt="Markdownify" width="600" height="337.5"></a>
  <br>
  Rubik's Cube Solver
  <br>
</h1>

<h4 align="center">A-Level Computer Science project</h4>

## 🌟 Highlights

* Capable of interactively solving *partially* scrambled cubes
* 3d cube with animated rotations
* Click and drag navigation around faces of cube
* Space bar timer for timing your own solves

## ℹ️ Overview

As part of a wider application, this project implements a Rubik's Cube solver inspired by the algorithms and methodologies from the 2019 paper, Solving the Rubik's Cube with Deep Reinforcement Learning and Search (Agostinelli et al., 2019).

Motivated by a desire to more deeply understand the mechanics of training deep neural networks and applying reinforcement learning techniques, I opted to build the neural network and backpropagation trainer from scratch. While this approach sacrifices efficiency, it showcases a reinforcement algorithm built from the ground up.

Currently, the solver is capable of solving Rubik's Cubes scrambled up to 6 moves, with plenty of room for further optimization and development. Despite its rudimentary implementation, the project showcases the power of the Deep Approximate Value Iteration algorithm in an engaging, interactive way.

## ⬇️ Setup

Windows + [Monogame 3.8.2](https://docs.monogame.net/articles/whats_new.html) + [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet)

## 🚀Usage

* r, o, b, g, y, w keys rotate the red, orange, ... and white centered faces respectively
* Hold down then release space bar to start timer
* Tap space bar to end timer
* Partially scramble and solve through navigation bar

## References

1. Agostinelli, F., McAleer, S., Shmakov, A., & Baldi, P. (2019). Solving the Rubik’s cube with deep reinforcement learning and search. *Nature Machine Intelligence*, 1(8), 356–363. Springer Science and Business Media LLC. DOI: [10.1038/s42256-019-0070-z](https://doi.org/10.1038/s42256-019-0070-z)
[![DOI:10.1038/s42256-019-0070-z](https://zenodo.org/badge/DOI/10.1038/s42256-019-0070-z.svg)](https://www.nature.com/articles/s42256-019-0070-z)

> Email [douglasgspencer@gmail.com](douglasgspencer@gmail.com) &nbsp;&middot;&nbsp;
> GitHub [@doug-spencer](https://github.com/doug-spencer) &nbsp;&middot;&nbsp;
