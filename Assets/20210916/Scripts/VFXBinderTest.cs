using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
using com.rfilkov.kinect;

[VFXBinder("Jphoooo/Rendertexture")]
public class VFXBinderTest : VFXBinderBase
{
    public BackgroundRemovalManager bgrmManager;
    public override bool IsValid(VisualEffect component)
    {
        return bgrmManager != null;
    }

    public override void UpdateBinding(VisualEffect component)
    {
        component.SetTexture("texture", bgrmManager.GetAlphaTex());
        component.SetVector2("res", bgrmManager.textureRes);
    }
}
