# Strawberry Jam Archipelago

an archipelago implementation for the popular celeste mod strawberry jam!

## Features
done:  
selective entity disabling up to grandmaster lobby  
strawberry checks  
cat checks  
gym checks  
silver checks  
archipelago connection  
chat client

todo:  
apworld  
item disabling rendering  


## Installation
for now, this archipelago remains a manual implementation with a manual enabling and disabling of entities. you can use any manual clients that include entity items. i have attahced one made by maybeGVST that will work with this mod while it develops into a full archipelago client.

anyone that wants to playtest this initial client and suggest changes/improvements:
1. download the latest zip file in releases 
2. put the zip file directly into your mods folder (no unzipping the zip :3)
your mods folder is in your steam install. by default, it is located at "C:\Program Files (x86)\Steam\steamapps\common\Celeste\Mods"
3. enable the mod using everest, turn on strawberry jam!

## About the APWorld
the apworld attached to releases is made by maybeGVST. it relies on bingosync progression for sj. there are also more items implemented than what he has done in the apworld. the following items are connected:
* Pufferfish -> Crystal bombs
* Theo crystals -> Switch crates (theyre separate items in the code, these may be combnined in the future)
* Loop blocks -> Cereal block clouds/Cereal block bumps
* Toggle Blocks -> Dash Traffic Blocks/Dash Toggle Blocks (this may be changed soon)

some items have been renamed:
* Loop blocks -> Cereal blocks (also i just realized i never tested these uh oh) 
* Grey Spring -> Dash Springs

additionally, the folloing items are enabled by default since they are not used in the apworld attached:
* Gravity triggers
* Teleport Fields

## Acknowledgements

the apworld attached in releases belongs to maybeGVST, who gave me permission to post it :3

i would like to acknoledge the many resources out there that helped me on this journey to developing this archipelago implementation. 
the archipelago for outer wilds, developed by ixrecs, helped me tremendously with the networking. 
the celeste discord helped me with interactable disabling, and the resources they shared made this possible. the base mod template that they develop is the base of this mod.
also, shoutouts to my friends who helped me with some of the more boring tasks. in particular, thanks to julie, jog, and isaac for various parts of this. and thank you to gvst for inspiring this idea.


## Disclosure Statement
no LLM assistence, AI assistence, or anything of the like was used in the making of this implementation. humans made this from start to finish.