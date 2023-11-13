# Sound-of-Cities-Demo

The inspiration for "Sound of Cities" comes from the construction system that many games have. The main objective is to make a program that combines sandbox construction and interactive audio, or it can be said to create an "audio sandbox". In this demo, a city simulation design is used. Each building corresponds to a timbre. Every building is a sound emitter.

There is a metronome in the UI below. Players can adjust the pitch of each building themselves in Edit mode, and also change the timing of each sound. While players are building cities, they are also creating music simultaneously. There is a character as the listener. The volume of each building decreases with distance, so the music can be changed interactively by moving the character.

Collaboration with a game designer is needed for further development of this project. This game probably needs to incorporate some traditional city simulation game mechanics, rather than just free music creation, otherwise many players may not find it interesting. At least the game needs to have some goals for the player to achieve. 

For example, not all pitches are unlocked when you first enter the game. You can only use a simple pentatonic scale (C, D, E, G, A) at the beginning so that even if the player does not much about music theory, they can still build a nice piece by randomly selecting from these pitches. Maybe rhythms aren't all unlocked at the start, either. In this demo, there are a total of 32 "grids" in each bar, and only 1, 9, 17, and 25 may be unlocked at the beginning, which are four down beats. As the game progresses, other target grids are gradually unlocked.

It doesn’t necessarily have to be a city simulation. It can be placing pieces of furniture in an indoor scene, or it can be placing some flowers, plants, and trees in a natural scene.

灵感来源于很多游戏都有的家园系统或者建造系统，主要就是想做一个把沙盒建造和互动音频结合起来的作品，或者也可以说是创造一个音频层面的沙盒。
在这个demo中是使用了城市模拟的设计，每种建筑都对应一个音色，同类型的建筑可以有相似的音色。每个建筑都是一个发声体。

下方UI有一个节拍器。玩家可以在Edit模式下自己调整每个建筑的音高，也能改变每个声音出现的时机。玩家在建造城市的同时自己也在创造音乐。
城市中有一个小人作为listener。玩家可以移动这个小人。每个建筑的音量随距离衰减，所以通过移动小人就可以让音乐产生变化。

这个游戏恐怕也需要结合一些传统的城市模拟游戏机制，而不能是单纯的自由音乐创作，否则大多数玩家可能不会觉得有趣。至少游戏需要有一些明确的目标让玩家达成。
刚进入游戏时，不会解锁所有的音高。可以在一开始只解锁一个简单的五声音阶（C,D,E,G,A），这样玩家即使完全不懂音乐，随机选择音高也有不错的听觉效果。
节奏也不会在一开始就全部解锁。在这个demo里每个小节一共有32个“grid”，而一开始可能只解锁1,9,17,25，也就是四个正拍。随着游戏的进行逐渐解锁别的target grid。

也未必做成城市模拟，可以是在室内摆放家具，也可以在自然场景里摆一些花草树木之类。
