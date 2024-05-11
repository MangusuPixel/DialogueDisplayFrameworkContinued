# Dialogue Display Framework Continued API

## Usage

Dialogue Display Framework Continued uses Content Patcher to load a dictionary from a fake path. Your content pack should be a pack for Content Patcher and target the following path:

"**aedenthorn.DialogueDisplayFramework/dictionary**"

Dictionary keys generally consist of the name of the speaker in the dialogue. See more details below.

So, an example CP shell would look like:

    {
        "Format": "1.23.0",
        "Changes": [
            {
                "Action": "EditData",
                "Target": "aedenthorn.DialogueDisplayFramework/dictionary",
                "Entries": {
                    "Emily": {
                        (your data goes here)
                    }
                }
            }
        ]
    }

When testing out your pack, you can use `patch reload <yourModID>` in the SMAPI console to reload all registered entries, so you can make edits and see them reflected in-game in real-time.

## Dictionary Keys

Dictionary keys can take many forms:

| Key                                  | Description |
| ------------------------------------ | ----------- |
| `<CharacterNameID>_<LocationNameID>` | ***Legacy support:** Edit NPC appearance data instead.*<br>Apply changes to a character listed in a location's `UniquePortrait` property
| `<CharacterNameID>_<AppearanceID>`   | Apply changes to a character using a specified appearance ID. Might not match their current texture if they were manually overriden elsewhere
| `<CharacterNameID>_Beach`            | Apply changes to a characters in beach attire
| `<CharacterNameID>`                  | Apply changes to a specific character
| `default`                            | Fallback option if no other data is specified or for a global dialogue setup

It's important to note that patches will be overridden in the order given above, not in the order they appear. For example, beach keys will take precedence over those with standard keys but will be replaced by appearance keys and location keys.

Entry keys can also contain a list of keys separated by a comma and space (e.g. "Emily, Abigail_Beach, etc"), in which case, each element in the list will count as keys, sharing the same patch data.

## Dictionary Objects

Dictionary values are objects with the following keys:

| Key        | Type    | Description |
| ---------- | ------- | ----------- |
| `packName` | string  | Manifest ID of the content pack containing this entry, used for logging.
| `copyFrom` | string  | Key to a data entry to copy data from. Any following data will override the copied data.
| `xOffset`  | integer | X offset of the dialogue box relative to its normal position on the screen.
| `yOffset`  | integer | Y offset of the dialogue box relative to its normal position on the screen.
| `width`    | integer | Width of the dialogue box, default 1200.
| `height`   | integer | Height of the dialogue box, default 384.
| `dialogue` | [DialogueData](#dialogue-data)       | Customizes the dialogue text display.
| `portrait` | [PortraitData](#portrait-data)       | Customizes the character's portrait image display.<br>Doesn't include the portrait frame background.
| `name`     | [TextData](#text-data)               | Customizes the name display which normally appears under the portrait frame.
| `jewel`    | [BaseData](#base-data)               | Customizes the friendship jewel display.
| `button`   | [BaseData](#base-data)               | Customizes the action button display.
| `sprite`   | [SpriteData](#sprite-data)           | **(Disabled)** Customizes a character sprite display.
| `gifts`    | [GiftsData](#gifts-data)             | Customizes a custom gift display.
| `hearts`   | [HeartsData](#hearts-data)           | Customizes a friendship hearts display.
| `images`   | List of [ImageData](#image-data)     | Custom images to draw. Assigning a value of `null` will erase pre-existing entries, otherwise it will merge the lists.
| `texts`    | List of [TextData](#text-data)       | Custom texts to draw. Assigning a value of `null` will erase pre-existing entries, otherwise it will merge the lists.
| `dividers` | List of [DividerData](#divider-data) | Custom dividers to draw. Assigning a value of `null` will erase pre-existing entries, otherwise it will merge the lists.
| `disabled` | boolean | Disable this entry entirely, default false.<br>Ignored if the `copyFrom` entry is disabled.

If any field is missing in an NPC entry, the field from the "default" entry will be used instead.

## Base Data

For all of the above entries that are objects (or lists of objects), the objects have the following common keys available (though they may not all use them):

| Key          | Type    | Description |
| ------------ | ------- | ----------- |
| `xOffset`    | integer | X offset relative to the box, default 0.
| `yOffset`    | integer | Y offset relative to the box, default 0.
| `right`      | boolean | Whether the x offset should be calculated from the right side of the box, default false.
| `bottom`     | boolean | Whether the y offset should be calculated from the bottom of the box, default false.
| `width`      | integer | Width of elements that need it.
| `height`     | integer | Height of elements that need it.
| `alpha`      | decimal | Opacity, default 1 (full opacity).
| `scale`      | decimal | Size scale, default 4 (most things in the game are displayed at 4x).
| `layerDepth` | decimal | Z-index of the element, default 0.88.
| `disabled`   | boolean | Whether to disable this element, i.e. if this is for an NPC for which you don't want a default element added, default false.


## Dialogue Data

Along the [base data](#base-data) keys, dialogue data has the following keys available:

| Key         | Type    | Description |
| ----------- | ------- | ----------- |
| `color`     | string | Supports color name, hex and RGB formats.
| `alignment` | enum   | Text alignment: 0 = left, 1 = center, 2 = right.


## Portrait Data

Along the [base data](#base-data) keys, portrait data has the following keys available:

| Key           | Type    | Description |
| ------------- | ------- | ----------- |
| `texturePath` | string  | The fake or real game path relative to the Content folder of the texture file used to draw (if omitted, use the character's default portrait sheet).
| `x`           | integer | X position in the source texture file, default -1 (disabled).
| `y`           | integer | Y position in the source texture file, default -1 (disabled).
| `w`           | integer | Width in the source texture file, default 64.
| `h`           | integer | Height in the source texture file, default 64.


## Sprite Data

**Sprite data is currently unavailable:** PR or code fixes are greatly welcomed.
~~Along the [base data](#base-data) keys, sprite data has the following keys available:~~

| Key          | Type    | Description |
| ------------ | ------- | ----------- |
| `background` | boolean | Whether to show the day / night background behind the sprite.
| `frame`      | integer | Which frame on the character sprite sheet to show. Set to -1 to animate the sprite instead.


## Hearts Data

Along the [base data](#base-data) keys, hearts data has the following keys available:

| Key                 | Type    | Description |
| ------------------- | ------- | ----------- |
| `heartsPerRow`      | integer | Number of hearts per row, default 14.
| `showEmptyHearts`   | boolean | Display empty hearts, default true.
| `showPartialHearts` | boolean | Display partial hearts, default true.
| `centered`          | boolean | If true, `xOffset` will point to the center of the row of hearts.


## Gift Data

Along the [base data](#base-data) keys, gift data has the following keys available:

| Key            | Type    | Description |
| -------------- | ------- | ----------- |
| `showGiftIcon` | boolean | Show the gift icon, default true.
| `inline`       | boolean | Show the check boxes to the right of the icon, default false.


## Image Data

The `images` field is an array of Image Data objects. Along the [base data](#base-data) keys, image data has the following keys available:

| Key           | Type    | Description |
| ------------- | ------- | ----------- |
| `id`          | string  | A [unique string ID](https://stardewvalleywiki.com/Modding:Common_data_field_types#Unique_string_ID) for inter-mod support, `image.unnamed` by default.
| `texturePath` | string  | The fake or real game path relative to the Content folder of the texture file used to draw.
| `x`           | integer | X position in the source texture file.
| `y`           | integer | Y position in the source texture file.
| `w`           | integer | Width in the source texture file.
| `h`           | integer | Height in the source texture file.


## Text Data

The `texts` field is an array of Text Data objects. Along the [base data](#base-data) keys, text data has the following keys available:

| Key               | Type    | Description |
| ----------------- | ------- | ----------- |
| `id`              | string  | A [unique string ID](https://stardewvalleywiki.com/Modding:Common_data_field_types#Unique_string_ID) for inter-mod support, `text.unnamed` by default<br>Unused with the `name` field.
| `text`            | string  | The text to display. Unused with the `name` field.
| `placeholderText` | string  | If using scroll background, this affects the size of the scroll while keeping it centered.
| `color`           | string  | Supports color name, hex and RGB formats.
| `scrollType`      | integer | Possible values:<br>`-1` = No scroll (default),<br>`0` = Sizeable scroll,<br>`1` = Speech bubble,<br>`2` = ???,<br>`3` = ???
| `junimo`          | boolean | Whether the name should be displayed in Junimo characters, because why not.
| `alignment`       | enum    | Possible values:<br>`0` = Left,<br>`1` = Center (default),<br>`2` = Right
| `centered`        | boolean | **Legacy support:** Use `alignment: 1`.<br>Whether to center the text at the given position.


## Divider Data

The `dividers` field is an array of Divider Data objects. Along the [base data](#base-data) keys, divider data has the following keys available:

| Key          | Type    | Description |
| ------------ | ------- | ----------- |
| `id`         | string  | A [unique string ID](https://stardewvalleywiki.com/Modding:Common_data_field_types#Unique_string_ID) for inter-mod support, `divider.unnamed` by default.
| `horizontal` | boolean | Horizontal, default false (i.e. vertical).
| `connectors` | [ConnectorData](#connector-data)  | Divider connector data (see below).
| `color`      | string  | Supports color name, hex and RGB formats<br>If specified, `Maps\MenuTilesUncolored` will be used instead of `Maps\MenuTiles`.

## Connector Data

The `connectors` field refers to an object with the following keys available:

| Key      | Type    | Description |
| ---------| ------- | ----------- |
| `top`    | boolean | Whether or not to display the top connector, default true.
| `bottom` | boolean | Whether or not to display the bottom connector, default true.
