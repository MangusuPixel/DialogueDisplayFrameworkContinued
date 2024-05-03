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

It's important to note that patches will be overridden in the order given above, not in the order they appear. For example, patches with beach keys will take precedence over those with standard keys but will be replaced by location keys.

Entry keys can also contain a list of keys separated by a comma and space (e.g. "Emily, Abigail_Beach, etc"), in which case, each element in the list will count as keys, sharing the same patch data.

## Dictionary Objects

Dictionary values are objects with the following keys:

| Key        | Type    | Description |
| ---------- | ------- | ----------- |
| `packName` | string  | Manifest ID of the content pack containing this entry, used for reloading the data in-game.
| `xOffset`  | integer | X offset of the dialogue box relative to its normal position on the screen.
| `yOffset`  | integer | Y offset of the dialogue box relative to its normal position on the screen.
| `width`    | integer | Width of the dialogue box (omit to use normal width, 1200)
| `height`   | integer | Height of the dialogue box (omit to use normal height)
| `dialogue` | object  | To customize the dialogue display (see below)
| `portrait` | object  | To customize the portrait display (see below)
| `name`     | object  | To customize the name display (see below)
| `jewel`    | object  | To customize the friendship jewel display (see below)
| `button`   | object  | To customize the action button display (see below)
| `sprite`   | object  | (disabled) To customize a custom character sprite (see below)
| `gifts`    | object  | To customize a custom gift display (see below)
| `hearts`   | object  | To customize a hearts display (see below)
| `images`   | object array | List of custom images (see below)
| `texts`    | object array | List of custom texts (see below)
| `dividers` | object array | List of custom dividers (see below)
| `disabled` | boolean | Disable this entry and use the game's default dialogue box setup

If any field is missing in an NPC entry, the field from the "default" entry will be used instead.

## Base Data

For all of the above entries that are objects (or arrays of objects), the objects have the following common keys available (though they may not all use them):

| Key          | Type    | Description |
| ------------ | ------- | ----------- |
| `xOffset`    | integer | X offset relative to the box, default 0
| `yOffset`    | integer | Y offset relative to the box, default 0
| `right`      | boolean | Whether the x offset should be calculated from the right side of the box, default false
| `bottom`     | boolean | Whether the y offset should be calculated from the bottom of the box, default false
| `width`      | integer | Width of elements that need it
| `height`     | integer | Height of elements that need it
| `alpha`      | decimal | Opacity, default 1 (full opacity)
| `scale`      | decimal | Size scale, default 4 (most things in the game are displayed at 4x)
| `layerDepth` | decimal | Z-index of the element, default 0.88
| `variable`   | boolean | Whether the size of the element is variable, tells the mod to calculate size based on the center of the element, default false
| `disabled`   | boolean | Whether to disable this element, i.e. if this is for an NPC for which you don't want a default element added, default false


## Name Data

Name data has the following additional keys available:

| Key               | Type    | Description |
| ----------------- | ------- | ----------- |
| `color`           | string  | Supports color name, hex and RGB formats
| `scroll`          | boolean | Whether to draw a scroll behind the text
| `placeholderText` | string  | If using scroll background, this affects the size of the scroll
| `centered`        | boolean | Whether to center the text on the scroll
| `scrollType`      | integer | n/a
| `junimo`          | boolean | Whether the name should be displayed in Junimo characters, because why not


## Dialogue Data

Dialogue data has the following additional keys available:

| Key         | Type    | Description |
| ----------- | ------- | ----------- |
| `color`     | string | Supports color name, hex and RGB formats
| `alignment` | enum   | Text alignment: 0 = left, 1 = center, 2 = right


## Portrait Data

Portrait data has the following additional keys available:

| Key           | Type    | Description |
| ------------- | ------- | ----------- |
| `texturePath` | string  | The fake or real game path relative to the Content folder of the texture file used to draw (if omitted, use the character's default portrait sheet)
| `x`           | integer | X position in the source texture file (if tileSheet is false)
| `y`           | integer | Y position in the source texture file (if tileSheet is false)
| `w`           | integer | Width in the source texture file, default 64
| `h`           | integer | Height in the source texture file, default 64
| `tileSheet`   | boolean | Whether the source texture  default true


## Sprite Data

**Sprite data is currently unavailable:** PR or code fixes are greatly welcomed.
~~Sprite data has the following additional keys available:~~

| Key          | Type    | Description |
| ------------ | ------- | ----------- |
| `background` | boolean | Whether to show the day / night background behind the sprite
| `frame`      | integer | Which frame on the character sprite sheet to show. Set to -1 to animate the sprite instead

## Jewel Data

Jewel data has no additional keys.


## Button Data

Button data has no additional keys.


## Hearts Data

Hearts data has the following additional keys available:

| Key               | Type    | Description |
| ----------------- | ------- | ----------- |
| `heartsPerRow`    | integer | Number of hearts per row, default 14
| `showEmptyHearts` | boolean | Include empty hearts, default true
| `centered`        | boolean | If true, xOffset will point to the center of the row of hearts


## Gift Data

Gift data has the following additional keys available:

| Key            | Type    | Description |
| -------------- | ------- | ----------- |
| `showGiftIcon` | boolean | Show the gift icon, default true
| `inline`       | boolean | Show the check boxes to the right of the icon, default false


## Image Data

The images field is an array of Image Data objects. Image data has the following additional keys available:

| Key           | Type    | Description |
| ------------- | ------- | ----------- |
| `id`          | string  | A [unique string ID](https://stardewvalleywiki.com/Modding:Common_data_field_types#Unique_string_ID) for inter-mod support, `unnamed.image` by default
| `texturePath` | string  | The fake or real game path relative to the Content folder of the texture file used to draw 
| `x`           | integer | X position in the source texture file
| `y`           | integer | Y position in the source texture file
| `w`           | integer | Width in the source texture file
| `h`           | integer | Height in the source texture file


## Text Data

The texts field is an array of Text Data objects. Text data has the following additional keys available:

| Key               | Type    | Description |
| ----------------- | ------- | ----------- |
| `id`              | string  | A [unique string ID](https://stardewvalleywiki.com/Modding:Common_data_field_types#Unique_string_ID) for inter-mod support, `unnamed.text` by default
| `color`           | string  | Supports color name, hex and RGB formats
| `text`            | string  | The text to display
| `scroll`          | boolean | Whether to draw a scroll behind the text
| `placeholderText` | string  | If using scroll background, this affects the size of the scroll
| `centered`        | boolean | Whether to center the text on the scroll
| `scrollType`      | integer | n/a
| `junimo`          | boolean | Whether the name should be displayed in Junimo characters, because why not


## Divider Data

The dividers field is an array of Divider Data objects. Divider data has the following additional keys available:

| Key          | Type    | Description |
| ------------ | ------- | ----------- |
| `id`         | string  | A [unique string ID](https://stardewvalleywiki.com/Modding:Common_data_field_types#Unique_string_ID) for inter-mod support, `unnamed.divider` by default
| `horizontal` | boolean | Horizontal, default false (i.e. vertical)
| `small`      | boolean | Show teeny divider, default false
| `connectors` | object  | Divider connector data (see below)
| `color`      | string  | Supports color name, hex and RGB formats<br>If specified, `Maps\MenuTilesUncolored` will be used instead of `Maps\MenuTiles`

## Divider Connector Data

The divider's bolts field is an object with the following keys available:

| Key      | Type    | Description |
| ---------| ------- | ----------- |
| `top`    | boolean | Whether or not to display the top connector, default true
| `bottom` | boolean | Whether or not to display the bottom connector, default true
