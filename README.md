<h1 align="center">
  <br>
  <img src="https://github.com/doug-spencer/RubiksCube/blob/main/Content/CubeGif.gif" alt="Markdownify" width="600" height="337.5"></a>
  <br>
  Rubik's Cube Solver
  <br>
</h1>

<h4 align="center">My A-Level Computer Science project</h4>

## 🌟 Highlights

* Capable of solving *partially* scrambled cubes
* 3d cube with animated rotations
* Click and drag navigation around faces of cube
* Space bar timer for timing your own solves

## ℹ️ Overview

The cube solver is based on algorithms and methods presented in the 2019 paper **Solving the Rubik’s cube with deep reinforcement learning and search** (Agostinelli et al., 2019).
I implemented the deep neural network and backpropogation trainer from scratch in order to get a full understanding of how they work (although this does mean they are inefficient!).
The solver can currently only solve from 6 moves scrambled, there is lots of potential to up this towards what was achieved in the paper. However the current implementation still demonstrates the algorithms power, even with a rudimentry implementation.

## ⬇️ Setup

Windows + [Monogame 3.8.2](https://docs.monogame.net/articles/whats_new.html) + [.NET 8.0](https://dotnet.microsoft.com/en-us/download/dotnet)

## 🚀Usage

* r, o, b, g, y, w keys rotate red, orange, ... centered faces respectively
* Hold down then release space bar to start timer
* Tap space bar to end timer
* Partially scramble and solve through navigation bar

## References

1. Agostinelli, F., McAleer, S., Shmakov, A., & Baldi, P. (2019). Solving the Rubik’s cube with deep reinforcement learning and search. *Nature Machine Intelligence*, 1(8), 356–363. Springer Science and Business Media LLC. DOI: [10.1038/s42256-019-0070-z](https://doi.org/10.1038/s42256-019-0070-z)
[![DOI:10.1038/s42256-019-0070-z](https://zenodo.org/badge/DOI/10.1038/s42256-019-0070-z.svg)](https://www.nature.com/articles/s42256-019-0070-z)

> Email [douglasgspencer@gmail.com](douglasgspencer@gmail.com) &nbsp;&middot;&nbsp;
> GitHub [@doug-spencer](https://github.com/doug-spencer) &nbsp;&middot;&nbsp;
