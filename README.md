#AdPlayer

##Installation 

Use following link to download unitypackage - [Link](https://placeholder.com)

> Make sure you import all of the files included with package

The CrossPromo player requires the following -
1. Having the CrossPromo prefab in scene - located in /CrossPromo/Prefabs/CrossPromo
2. The CrossPromo.Scripts namespace
3. A reference to the CrossPromoPlayer so that you could call it's functions and subscribe to it's events

> The player will begin playing on Start and each time it's re-enabled!

##Usage

Package has four public base API's 

-  `public void Next();` - Will play the next video if next video if previous video exists
-  `public void Previous();` - Will play the previous video if previous video exists
-  `public void Pause();` - Will Pause ad if ad is Playing
-  `public void Resume();` - Will Resume ad if ad is Paused

** Player will begin playing one prefab is instansiated and will loop over all videos **
** If the Ad is clicked the corresponsing ad will open a webpage with the displayed video**

*** Prefab sits under this path - `Assets/CrossPromo/Prefabs/CrossPromo` ***

