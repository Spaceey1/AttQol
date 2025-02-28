# A township tale mod that aims to add a couple qol fixes/features
## How to install
1. Download and install the 64 bit version of [BepInEx 5](https://github.com/BepInEx/BepInEx) to your att install
2. Run the game once to generate the necessary files and directories
3. Put the mod .dll file into BepInEx/Plugins folder inside your game directory
## Building from source
1. ``git clone https://github.com/BepInEx/BepInEx.git``
2. Change the directory in attqol.csproj to point towards your game directory
3. ``dotnet build``
4. The mod dll should go into your game's ``Plugins`` folder but if it didn't it should also be present in bin/Debug/net472
## Current features
- Jump under teleport button (to be changed), can be toggled in the quick access menu
- More to come