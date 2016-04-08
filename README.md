# MonoGame-3D-Game-Test
A sample of a game that should be created with RPG Paper Maker (http://rpgpapermaker.com/). This is a cross-platform game runnable on Windows, Linux, and Mac. See http://www.monogame.net/ for more informations about what MonoGame is.

## Run the game on Windows

Go to the **Game** directory and find the application **Test.exe**. Run it. 

## Run the game on Linux

You have to install MonoGame packages to launch the game. Install those ones :

> sudo apt-get install libopenal-dev mono-runtime gtk-sharp3

If all the installation is ok (if not, there will probably be a conflict), go to the **Game** directory and run the game with this command :

> mono Test.exe

## Source code

You can find all the source code into the **Test** directory (*.cs files). The solution ("Test.sln") has been created with Visual Studio Community 2015 on Windows 7 64bits, MonoGame 3.5, and a MonoGame Cross Platform Desktop Project.

## How to play

### Move Lucas

Use WASD keys to move the character.

### Turn the camera

Press left and right button to turn at 90° around Lucas.