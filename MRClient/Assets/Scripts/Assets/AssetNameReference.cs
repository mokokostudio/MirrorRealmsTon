using System;
using UnityEngine.AddressableAssets;

[Serializable]
public class AssetNameReference {
    public string name;
    public AssetReference reference;
    public AssetNameReference(string name, AssetReference reference) {
        this.name = name;
        this.reference = reference;
    }
}
