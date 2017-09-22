# Stage Mechanic

## Scope of project

We are creating an application that allows one to practice portions of games such as Catherine, Robo5, and Pushmo outside of their respective games. Initial implementation will allow the user to test different solves for correctness but will not implement pressure elements such as time limits, score, move limits, etc. We are explicitly NOT designing a game (even though some game elements may be included). This is a tool for practicing a game outside of the game.

Another aspect of this project will be a stage designer allowing the user to create the sections of stages they wish to practice.

We will likely (TBD) have some degree of backstory/mythology associated with this project as well, though it is not intended to be story-based game (or any kind of game for that matter).

We may include some original music to build/practice by as well. (TBD)

## Target audences

We will target both the amateur/casual player of Catherine, Robo5, and Pushmo looking for a more convenient way to practice individual parts of the game as well as professional/aspiring competitive and speedrun players looking to test times/results of different solves without the overhead associated with a full game.

## Current state

This project is just getting started! Our official project start date is September 12th, "National Video Game Day" 2017. So far we have focused on the Level Editor implementation and it is currently possible to easily place blocks in 3D space. Block properties (trap, player movement, bombs, etc) have been defined (though not yet implemented) and textures are in the works. Once the Level Editor as has reached a usable state, development focus will shift to the Play Mode while level designers create the default map set.

## Licensing

This project is being developed as an Open Source project entirely by volunteers. The resulting application will be made freely available to users and all source code made free available to developers/artists/etc.

We are using the BSD 3-clause license. This allows maximum freedom while protecting contributors. Essentially anyone is allowed to do whatever they want to with anything published as part of this project. However it is worth noting that portions of the project are being developed using Unity Personal and this may add certain restrictions on those wishing to profit commercially from certain parts of the project. If you are wanting to use a portion or all of this project in a commercial application or bundle please consult with proper legal authorities regarding mixing the BSD license with the Unity Personal license.

## For Developers/Artists/Musicians

### Tools/technologies used

* Game Engine: Unity Personal 2017.1.1f1
* Languages: C# (primarily) and Javascript
* 3D Modeling: Blender 3D
* 2D Painting: .PSD file format (Photoshop/Krita/gimp/CSP)
* Audio/Music: TBD, one or more of: Audacity, Sony Acid Pro, Fruity Loops, Presonus Studio One

### Getting started

Step 0: Play the video game [Catherine](http://catherine.wikia.com/wiki/Catherine_Wiki), published by Atlus, at least enough to get the basic mechanics down. Optionally play Robo5 and pushmo as well.

Step 1: (Optional) Read [The Rapunzel Novel](http://fftranslations.atspace.co.uk/rapunzel/)

Step 2: Join our Discord Chat: https://discord.gg/w7y8u5g

Step 3: Download and install Unity Personal:

&nbsp;&nbsp;&nbsp;&nbsp;https://store.unity.com/download?ref=personal

Step 4: Install git

&nbsp;&nbsp;Windows, Mac:

&nbsp;&nbsp;&nbsp;&nbsp;https://git-scm.com/downloads

&nbsp;&nbsp;Fedora 20:

&nbsp;&nbsp;&nbsp;&nbsp;dnf -y update && dnf -y install git

&nbsp;&nbsp;CentOS 7:

&nbsp;&nbsp;&nbsp;&nbsp;yum -y update && yum -y install git

&nbsp;&nbsp;Ubuntu/Kubuntu/Xubuntu:

&nbsp;&nbsp;&nbsp;&nbsp;sudo apt-get update && sudo apt-get install git

Step 5: Make sure your video card drivers are up to date

&nbsp;&nbsp;&nbsp;&nbsp;http://www.nvidia.com/Download/index.aspx

Step 6: Check out the source code

&nbsp;&nbsp;Using Linux, Mac command line, or git bash on Windows

&nbsp;&nbsp;&nbsp;&nbsp;git clone https://github.com/TheRealCatherine/StageMechanic.git

## Resources

If you are completely new to game development, graphic design, music production, etc. but are eager to contribute here are some resources to help you get the skills needed to contribute.

### General

* [CGCookie](https://cgcookie.com/) - $21/mo, professionally developed courses on 3D modeling and game development using Blender 3D and Unity - the two main technologies we are using. Highly recommended.

### Development, coding, and level design

* [Official Unity tutorials](https://unity3d.com/learn/tutorials) - Free tutorials developed by the makers of the game engine we are using.
* [Brackeys](https://www.youtube.com/user/Brackeys) - Fast-paced tutorial videos on getting started with Unity
* [C# 6.0 Pocket Reference](https://www.amazon.com/6-0-Pocket-Reference-Instant-Programmers/dp/1491927410) - Pocket size reference book that gives an overview of the primary development language we are using.
* [Effective C#](https://www.amazon.com/Effective-Covers-Content-Update-Program/dp/0672337878) - Prefer 3rd edition but 2nd edition is ok.
* [Unit Testing in Unity](https://blogs.unity3d.com/2014/07/28/unit-testing-at-the-speed-of-light-with-unity-test-tools/)

### Music composition, recording, and production

* [Rocksmith 2014](https://rocksmith.ubisoft.com/rocksmith/en-us/home/) - The fastest way to learn guitar (including bass guitar)
* [Catherine CDLC for Rocksmith 2014](http://customsforge.com/page/customsforge_rs_2014_cdlc.html/_/pc-enabled-rs-2014-cdlc/lost-r26278) - also search in "Ignition" on that site for Shoji Meguro, many Persona songs are available.
* [Yousician](http://yousician.com/) - Teaches guitar, bass, piano, and ukulele
* [Studio One Prime](https://shop.presonus.com/products/studio-one-prods/Studio-One-3-Prime) - Free version of Presonus' excellent digital audio workstation/music production software.
* [Audacity](http://www.audacityteam.org/download/) - Free and simple audio recording/mixing tool.
* [FL Studio](https://www.image-line.com/flstudio/) - Popular digital audio workstation
* [Sony Acid Pro](http://www.sonycreativesoftware.com/products/product.asp?pid=383&pageid=119) - Full featured digitial audio workstation/music production software
