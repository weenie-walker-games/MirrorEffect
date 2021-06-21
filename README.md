# MirrorEffect
A mirror effect for the Adventure Creator plug-in for Unity (in progress)

## Install Instructions
1. Import the custom Unity package into your project. Make sure Adventure Creator is already installed or you will get errors about missing references within the script.
2. Copy the updated version of the MirrorEffect.cs script and replace the version that was just installed via the package

## To Use Mirror System
1. An example mirror is included in the Unity package. Bring the MirrorParent prefab into your scene.
2. Adjust the BoxCollider2D to match where you want the mirror effect to activate.
3. In the MirrorEffect script in your scene, adjust the scale factor to match the height of your character when it is near the mirror. (This uses AC's scaling so depending on the size of your scene, this might need to be adjusted quite a bit.)
4. Make sure your player is tagged "Player" so the collider will recognize it.
