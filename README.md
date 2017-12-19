# Stage Mechanic

## Scope of project

We are creating an application that allows one to practice portions of games such as Catherine, Robo5, and Pushmo outside of their respective games. Initial implementation will allow the user to test different solves for correctness but will not implement pressure elements such as time limits, score, move limits, etc. We are explicitly NOT designing a game (even though some game elements may be included). This is a tool for practicing a game outside of the game.

An important aspect of this project will is a stage designer allowing the user to create the sections of stages they wish to practice.

We will likely (TBD) have some degree of backstory/mythology associated with this project as well, though it is not intended to be story-based game (or any kind of game for that matter).

We may include some original music to build/practice by as well. (TBD)

## Target audences

We will target both the amateur/casual player of Catherine, Robo5, and Pushmo looking for a more convenient way to practice individual parts of the game as well as professional/aspiring competitive and speedrun players looking to test times/results of different solves without the overhead associated with a full game.

## Current state

This project is just recently getting started! Our official project start date is September 12th, "National Video Game Day" 2017.

[Screenshots](https://github.com/TheRealCatherine/StageMechanic/wiki/Screenshots) [Videos](https://www.youtube.com/channel/UCO7dcRrjPb5eCst-9uAXXPg/videos)

### What works:
* Easily and quickly create levels using our custom-made level editor mode. (level editor)
* Placing all Catherine 1-style blocks anywhere in 3d space (level editor)
* Catherine-1 style gravity and EDGE mechanics (play mode)
* Grid-based player movement and block climbing (play mode)
* Pushing/pulling blocks and rows of blocks (play mode)
* Immobile block mechanic (play mode)
* Windows, OS X, and Linux builds
* Keyboard input
* XBox 360 controller input (Windows, edit mode)
* Editing and playing through levels based on Catherine 1 levels: 1 and 2-1 (play mode)

### Currently in progress
* Sidling (hanging onto block edges) (play mode)
* Player movement animantion (play mode)
* Special block functions (ie bombs, cracked blocks, etc)
* XBox 360 controller input (Windows, play mode)

[Development Builds](https://github.com/TheRealCatherine/StageMechanic/wiki/Downloads)

## Licensing

This project is being developed as an Open Source project entirely by volunteers. The resulting application will be made freely available to users and all source code made free available to developers/artists/etc.

We are using the BSD 3-clause license. This allows maximum freedom while protecting contributors. Essentially anyone is allowed to do whatever they want to with anything published as part of this project. However it is worth noting that portions of the project are being developed using Unity Personal and this may add certain restrictions on those wishing to profit commercially from certain parts of the project. If you are wanting to use a portion or all of this project in a commercial application or bundle please consult with proper legal authorities regarding mixing the BSD license with the Unity Personal license.

Additionally note that items under the StageMechanic/Assets/3rdParty folder may be under other licenses such as the Unity Store license, check the LICENSES-EXCEPTIONS file for specifics.

## Contribting

This project is being developed entirely by volunteers and is managed by an industry veteran in her spare time. If there is anything you would like to work on please join our Discord chat and let us know. Pretty much no matter what it is we can find a way to fit it in. Our project manager often says "do what you want, when you want, for as long as you want" and applies this not only to life but to project management.

### Tools/technologies used

* Game Engine: Unity Personal 2017.3.0f3
* Languages: C# 6.0 for primary development, Lua (via MoonSharp) for custom scripting
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

&nbsp;&nbsp;&nbsp;&nbsp;https://desktop.github.com/

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

Below are resources for those new to game design and devleopment as well as those simply new to this project. If you would like to contribute but have never written a line or code or even heard of Blender thats OK! Join our Discord chat and let us know what sparks your interest and we will help you find appropriate resources to get you up to speed enough to contribute the thing you want to do.

### General

* [CGCookie](https://cgcookie.com/) - $21/mo, professionally developed courses on 3D modeling and game development using Blender 3D and Unity - the two main technologies we are using. Highly recommended.

### Character and story development

#### Mythological, Historical, Linguistic, and Religious Source Material

##### Websites:

* [Ancient Mesopotamian Gods and Goddesses](http://oracc.museum.upenn.edu/amgg/)

##### Books:

* [Myths from Mesopotamia: Creation, the Flood, Gilgamesh, and Others](https://www.amazon.com/Myths-Mesopotamia-Creation-Gilgamesh-Classics/dp/019953836)
* [Inanna, Queen of Heaven and Earth: Her Stories and Hymns from Sumer](https://www.amazon.com/Inanna-Queen-Heaven-Earth-Stories/dp/0060908548)
* [Into the Great Below: A Devotional for Inanna and Ereshkigal](https://www.amazon.com/Into-Great-Below-Devotional-Ereshkigal/dp/0982579837)
* [Queen of the Great Below: An Anthology in Honor of Ereshkigal](https://www.amazon.com/Queen-Great-Below-Anthology-Ereshkigal/dp/1453878963)
* [The Egyptian Book of the Dead: The Book of Going Forth by Day: The Complete Papyrus of Ani](https://www.amazon.com/Egyptian-Book-Dead-Integrated-Full-Color/dp/1452144389)
* [Leaves from the Garden of Eden](https://www.amazon.com/Leaves-Garden-Eden-Howard-Schwartz/dp/0199754381)
* [The Legend of St. Catherine](https://www.amazon.com/Legend-Katherine-Alexandria-Classic-Reprint/dp/B008J2SQ3O)
* [The Golden Legend](https://www.amazon.com/Golden-Legend-Readings-Saints/dp/0691154074)
* [Sumerian Grammar](https://www.amazon.com/Sumerian-Grammar-Handbook-Oriental-Studies/dp/1589832523)
* [Complete Babylonian](https://www.amazon.com/Complete-Babylonian-Yourself-Martin-Worthington/dp/0340983884)


#### Catherine Resources

* [Catherine Game Movie](https://www.youtube.com/watch?v=QX_ImJmCbSs)
* [Catherine: The Novel](http://fftranslations.atspace.co.uk/catherine/)
* [Catherine: The Mysterious Tale of Rapunzel](http://fftranslations.atspace.co.uk/rapunzel/index.html)
* [Another Take on Catherine](https://www.youtube.com/watch?v=f8k8dG27pB8&t=77s)
* [Catherine Game Reddit](http://reddit.com/r/catherinegame)
* [Catherine Discord Chat](https://discord.gg/nG5rN8B)

#### Robo5 Resources

* [Launch Trailer](https://www.youtube.com/watch?v=2S96oOPK1Ck)

##### Pushmo Resources

* [Pushmo World Launch Trailer](https://www.youtube.com/watch?v=Vm7qYpklhNg)

### Level design

#### Catherine Resources

* [Catherine walktrhough](https://www.youtube.com/watch?v=dGdVFn8KbBc&list=PLCF294B7A87DAB31C) this is from a new player rather than ideal solves
* [Rapunzel walktrhough](https://www.youtube.com/watch?v=NLRzLdbnpLU&list=PL5775F9FDCBD08849) ideal or near-ideal solves for each stage

#### Robo 5 Resources

* [Chapter 1 walkthrough](https://www.youtube.com/watch?v=WNSG7Qq8NeI)

#### Pushmo Resources

* [Pushmo World walktrhough part 1](https://www.youtube.com/watch?v=oUEa2FHGAAw)

### Asset design

When using artwork from other sources it _MUST_ be either CC0, CC-BY, BSD, or other license that at most requires attribution. CC0 or other public domain licenses are strongly preferred. CC-SA, GPL, and other licenses that impose restrictions on commercial usage are not allowed in this project, however you may make a separate asset pack that is compatible with this project using assets with CC-SA, GPL, or other such licenses.

* [Wikimedia commons](https://commons.wikimedia.org/wiki/Main_Page)
* [Clipart today](http://www.clipartoday.com)
* [Open Game Art](https://opengameart.org/textures/all?field_art_licenses_tid[0]=17981&field_art_licenses_tid[1]=2&field_art_licenses_tid[2]=10310&field_art_licenses_tid[3]=4)
* [Unity Asset Store](https://www.assetstore.unity3d.com/en/)

### Development, coding

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
