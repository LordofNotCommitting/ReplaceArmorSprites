This mod allows you to change sprite of existing armor to another armor.
(i.e transmog/layering armor/etc)
This should be applied to player only.

Did you got your armor from your favourite corporation and realized that, it looks ugly. Or perhaps even fugly. Dear god, I spent 2788 faction chips on this armor set upgrade and it looks like ♥♥♥♥.

This mod allows you to change appearence of armor into some other armor.
After starting the game with mod installed,
Change
C:\Users\[username]\AppData\LocalLow\Magnum Scriptum Ltd\Quasimorph_ModConfigs\ReplaceArmorSprites\ArmorSpriteConversionList.tsv
(ask where the Appdata is to AI if you don't know where it is)
put new record into this .tsv file. (editable with text editor)

"[from armor ID]  [to armor ID]"
It is TAB(  ).

like below:
FromArmorID	ToArmorID
ddr_power_armor_1	rwa_power_armor_1
ddr_power_boots_1	rwa_power_boots_1
ddr_power_helmet_1	common_glasses_1

Right next to ArmorSpriteConversionList.tsv. there is also example_ArmorSpriteConversionList.tsv. This contains example file format.

To see armor ID, you can use console command mod and use command "itemscan".
Beware as using console command will invalidate achievement progress on said save. Either do NOT save after using console command or use console command on separate save. Or use wiki to get armor ID once it gets updated to 1.0.
