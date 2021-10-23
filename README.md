# AdPlayer :movie_camera:

## Installation :+1:

Use following link to download unitypackage - [Link](https://drive.google.com/file/d/1bpSCHgOHDbxlm5yu7alzFx4ksbdL-vLu/view?usp=sharing)

A link for the demo apk file [Link](https://drive.google.com/file/d/1dgatjN1wZT-ASRo_pD4l9aubg9KpBDmh/view?usp=sharing)

> Make sure you import all of the files included with package 

The CrossPromo player requires the following -
1. Having the CrossPromo prefab in scene - located in /CrossPromo/Prefabs/CrossPromo
2. The CrossPromo.Scripts namespace
3. A reference to the CrossPromoPlayer so that you could call it's functions and subscribe to it's events

> The player will begin playing on Start and each time it's re-enabled!

## Usage :bulb:

Package has four public base API's 

-  `public void Next();` - Will play the next video if next video if previous video exists :fast_forward:
-  `public void Previous();` - Will play the previous video if previous video exists :rewind:
-  `public void Pause();` - Will Pause ad if ad is Playing :arrow_forward:
-  `public void Resume();` - Will Resume ad if ad is Paused :pause_button:

** Player will begin playing one prefab is instansiated and will loop over all videos **
** If the Ad is clicked the corresponsing ad will open a webpage with the displayed video**

*** Prefab sits under this path - `Assets/CrossPromo/Prefabs/CrossPromo` ***

