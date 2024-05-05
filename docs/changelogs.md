# Changelogs

## 0.6.0
* Added support for patches needing to be applied only with specific appearances/locations.
* Added `ID` fields to image, text and divider objects for inter-mod support.
* Divider changes:
	* Default height now sets to the dialogue box's height.
    * Added a `connectors` field to include the connection image at the ends of dividers.
    * Replaced the `red`, `green` and `blue` fields with a `color` field which supports more color formats.
    * Fixed `right` and `bottom` fields not working as intended.
* Removed `tilesheet` field from portrait data. Instead, directly assign a value to `x` and `y`.
* Changed the way the default appearance is loaded. Entries will complete missing fields to fit the vanilla look, including a default divider and portrait image.
* Optimized asset caching for better performance.
* Fixed an issue with array objects not being edited properly by Content Patcher.
* Fixed potential crash during dialogues with NPCs not yet spawned.
* Removed api docs from release files. Mod authors can always read them online.

## 0.5.2
* Patch reloading no longer requires re-opening the current dialogue to see changes.

## 0.5.1
* Fixed dialogue crashing on custom NPCs.

## 0.5.0
* Initial release based on [Dialogue Display Framework](https://github.com/aedenthorn/StardewValleyMods/tree/master/DialogueDisplayFramework).
* Added support for listed key.
* Updated support for colors to use HEX, RGB and named colors.
* Fixed incompatibility with the 1.6 SDV update.
* Disabled support for sprite data.
* Disabled support for Mod Updater.