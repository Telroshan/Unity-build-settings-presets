# Unity build settings presets

This asset lets you save build settings presets in Unity and switch easily between them.

On a project, I needed 2 different standalone builds (a client build, and a server build), with different settings, different scene lists and different script compilation defines. It was pretty boring to manually change those settings each time I'd want to build my game, so here came the Unity build settings presets system.

## Install

Simply download this repo and place it in your Unity project's Assets folder.

You should now have a `Build presets` menu in the menu toolbar.

![image](https://user-images.githubusercontent.com/19146183/64428975-46620e00-d0b5-11e9-8752-e544654a8ff4.png)

## How to use
### Create preset
Click on `+ New (from current settings)` to create a new preset.

![image](https://user-images.githubusercontent.com/19146183/64429643-b91fb900-d0b6-11e9-84ad-6bb1d3f42ed9.png)

_Note : this creates a preset based on your current build settings._

_Note2 : you can also duplicate an existing preset and start working with it_

Presets are stored as assets in the `Unity-build-settings-presets/Presets folder`. You'll find a `Default` preset there, which just contains the basic build settings for standalone, you can remove it safely if you don't need it.

### Edit preset
Select a preset to view its properties in the inspector.

![image](https://user-images.githubusercontent.com/19146183/64432085-1b2eed00-d0bc-11e9-8139-aeac76a125bd.png)

You can edit the properties directly from there.

The preset's properties match the [EditorUserBuildSettings](https://docs.unity3d.com/ScriptReference/EditorUserBuildSettings.html)'s properties, except the `scene list` and `script compilation defines` that are in the [PlayerSettings](https://docs.unity3d.com/ScriptReference/PlayerSettings.html).

You can also click on the "Overwrite with current build settings" button to replace the preset's properties with the current build settings.

![image](https://user-images.githubusercontent.com/19146183/64432161-3ef23300-d0bc-11e9-8018-eb8c11b91f80.png)

### Apply preset
#### Via the menu bar
You can apply a preset (thus replacing your current build settings by the preset's properties) by clicking on its name in the menu bar

![image](https://user-images.githubusercontent.com/19146183/64430739-32201000-d0b9-11e9-9d00-e1d5afdd965b.png)

#### Via the preset inspector
In the preset inspector, you can also click the `Apply` button to get the same result.

![image](https://user-images.githubusercontent.com/19146183/64432238-6fd26800-d0bc-11e9-927f-8cc24ffc6a71.png)


## License (MIT)
Copyright (c) 2019 Telroshan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
