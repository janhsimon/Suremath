# Suremath

An eLearning application to support math courses made in Unity. Visualization and examination platform for mathemtical graphs and the corresponding formulas, such as line segments and parabolas. Designed to be used both on desktop and mobile platforms by teachers in the classroom and by students at home.

# Overview

The project was developed with Unity (Version 5.6.1f1), so you need a compatible Unity Editor to open it. It was written to be used and tested with a focus group of local German students, thus all the text content in the application is German, but that can easily be translated.

# Working with the project

Suremath is set up in the editor to be as easy to work with as possible. It only uses a single scene currently (Main.scene). It only has three root-level game objects: the main camera, the canvas (in which the vast majority of game code is located) and the event system, that corresponds to the canvas. Input is automatically handled by Unity's interface/GUI system and has been tested on Windows PCs with keyboard and mouse, as well as on touch devices.

The project folders are sorted by content, rather then by asset type where it makes sense. There is no "Scripts" folder for example, you can find the code in the folders with the other assets that belong to it. The "Math View" folder holds the scripts, material and render texture for the square graph area in the application, on the right-hand side.
