using Cysharp.Threading.Tasks;
using ProtoBuf.Serializers;
using Unity.VectorGraphics;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class AOTReference
{
    public void DoNotCall() {
        RepeatedSerializer.CreateVector<int>();
        RepeatedSerializer.CreateVector<uint>();
        RepeatedSerializer.CreateVector<long>();
        RepeatedSerializer.CreateVector<ulong>();
        RepeatedSerializer.CreateVector<string>(); 
    }
}
