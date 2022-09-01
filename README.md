## 🔥 kinect VFX Testbed 🔥

⚠️ <b>This repository is based on the [Azure Kinect Examples for Unity](https://assetstore.unity.com/packages/tools/integration/azure-kinect-examples-for-unity-149700?locale=zh-CN#content) plugin, so you must install this plugin to run this project</b>

Visit my blog for more details: [Jphoooo](https://jphoooo.github.io/posts/Unity-Kinect.html)

Unity version : `2020.2.6 f1` or `later`

### 🧊 20210916

![20210916-preview](https://github.com/JpHoooo/unity-kinect-testbed/blob/master/Recordings/20210916-preview.gif)

#### Issue

When opening the project, you will find the following bug:

![20210916-bug](https://github.com/JpHoooo/unity-kinect-testbed/blob/master/Recordings/20210916-bug.jpg)

#### Reason

`BackgroundRemovalManager.cs` sets the access permission of the `textureRes` parameter to private, so we cannot call it directly

#### Solution

find the `BackgroundRemovalManager.cs` and modify it like the following code

```diff 
  //render texture resolution
- private Vector2Int textureRes;
+ public Vector2Int textureRes;
```

### 🛰️ 20210621

![20210621-preview](https://github.com/JpHoooo/unity-kinect-testbed/blob/master/Recordings/20210621-preview.gif)




