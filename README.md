# Unity Assets Advanced Editor
Unity .assets and AssetBundle editor

UAAE is an advanced editor for Unity .assets and AssetBundle files. It is based on DerPopo's UABE tool, but improves its functions.
UAAE isn't so `advanced` because it is still missing a lot of features and may have some bugs, so unless your game is unity 2019.3+, please use the original [UABE](https://github.com/DerPopo/UABE) for now. But it's better than nothing at all. Feel free to contribute.

#### Supported unity versions: 5 - 2020.2

## Features
* Open assets/bundle files
* Add/Remove assets
* Export/Import/Remove/Info options (for assets files in Menu)
* Bundle file compression/decompression
* Data view
* Dependency loading
* Monobehaviour deserialization in data view
* Export/Import assets to dump/raw format (only TXT and one asset import)
* Save modified assets files and bundle file
## Todo
This list may be incomplete!
* Advanced error checking (with logging)
* Dependency data view
  * Add dependency option
  * Edit dependency option
  * Remove dependency option
  * Save map to file
* [view asset] option in data view
* Tabs in data view
  * Dump view tab
  * Raw view tab
  * Preview tab
* Container data view
  * Add container option
  * Edit container option
  * Remove container option
* ~~Monobehaviour deserialization in data view~~ (including il2cpp technology)
* Asset list searching
  * By name
  * Binary content
  * Monobehaviour
  * Transform
* Go to asset option
* Asset preview option
* Dump export/import in
  * JSON format
  * XML format
* Plugin support
* Plugins
  * Texture plugin
  * TextAsset plugin
  * Audio plugin (including Wwise)
  * Mesh plugin
  * MovieTexture plugin
  * Prefab/Scene plugin **(custom)** *
  * Sprite editor plugin **(custom)** *
* ModInstaller and ModMaker
* Cldb/Tpk editor
* Undo/Redo option
* Edit button
  * Dump editor tab
  * Raw editor tab
  * Asset editor (this is where the list of plugins will be)
* ~~Bundle compression support~~
* Batch import/export assets
* Command line support
* Blank assets file/bundle creation
* Blank ResS/Resource file creation
* Blank MonoScript creation
* Progress bar form when opening multiple assets files
* Settings
* MacOS, Linux and Mobile support
* Implement the features that are going to be in new [UABE](https://community.7daystodie.com/topic/1871-unity-assets-bundle-extractor/?do=findComment&comment=357397)

## Build
* Visual Studio 2019 or newer

## Credits
* [AssetsTools.NET](https://github.com/nesrak1/AssetsTools.NET)

## Disclaimer
**None of the repo, the tool, nor the repo owner is affiliated with, or sponsored or authorized by, Unity Technologies or its affiliates.**

## License
This software is distributed under the MIT License