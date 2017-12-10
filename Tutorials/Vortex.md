# Vortex Tutorial

### Repacking Files
When you run Vortex you get prompted with an "Select "YGO_DATA" Folder" prompt, this is the folder that 
Onomatopaira creates when extracting all the game contents. Once you've choen this folder the tool will 
then use the TOC file found in the game's install directory. This is essential because it needs to know
the order of which to pack the files back into the DAT file. If it can't locate the install directory
the tool will crash, to avoid this make sure you've launched the game at least once through Steam to
ensure the first time setup has ran.

The tool is also very Disk IO and RAM heavy so make sure you lay off other tasks to ensure the
packing goes smoothly, after the packing is done you'll notice there are two new files
``YGO_DATA.DAT`` and ``YGO_DATA.TOC``, these files will need to be copied to the game's install 
location, any changes you made to the games content has now been repacked and the game is ready to load it.
