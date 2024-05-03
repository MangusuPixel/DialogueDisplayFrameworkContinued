# Changelogs

## 0.6.0
* Added support for patches needing to be applied only with specific appearances/locations.
* Added `ID` fields for inter-mod support.
* Added `bolts` field to dividers to specify if those should include the bolts to the frame.
* Added asset caching for performance improvements.
* Fixed an issue with array objects not being edited properly by Content Patcher.
* Fixed potential crash during dialogues with NPCs not yet spawned.
* Removed api docs from release files. Mod authors can always read them online.

## 0.52
* Patch reloading no longer requires re-opening the current dialogue to see changes.

## 0.51
* Fixed dialogue crashing on custom NPCs.

## 0.50
* Initial release based on [Dialogue Display Framework](https://github.com/aedenthorn/StardewValleyMods/tree/master/DialogueDisplayFramework).
* Added support for the 1.6 update.
* Added support for listed key.
* Disabled support for sprite data.
* Disabled support for Mod Updater.