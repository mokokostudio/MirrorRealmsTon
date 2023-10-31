public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	// System.Core.dll
	// System.dll
	// UniTask.Addressables.dll
	// UniTask.Linq.dll
	// UniTask.dll
	// Unity.Addressables.dll
	// Unity.InputSystem.dll
	// Unity.ResourceManager.dll
	// UnityEngine.CoreModule.dll
	// mscorlib.dll
	// protobuf-net.Core.dll
	// protobuf-net.dll
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Cysharp.Threading.Tasks.AsyncReactiveProperty<int>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<int>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>
	// Cysharp.Threading.Tasks.Linq.IAsyncWriter<System.ValueTuple<UnityEngine.Vector3,float,float>>
	// Cysharp.Threading.Tasks.UniTask<object>
	// Cysharp.Threading.Tasks.UniTask<float>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask<int>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,int>>
	// Cysharp.Threading.Tasks.UniTask<byte>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<int>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<float>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,int>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<object>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<byte>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTaskCompletionSource<object>
	// System.Action<float>
	// System.Action<byte>
	// System.Action<System.ValueTuple<UnityEngine.Vector3,float,float>>
	// System.Action<UnityEngine.InputSystem.InputAction.CallbackContext>
	// System.Action<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>>
	// System.Action<object>
	// System.Action<Cysharp.Threading.Tasks.AsyncUnit,int>
	// System.Action<int,object>
	// System.Action<int,int>
	// System.Collections.Concurrent.ConcurrentDictionary<object,int>
	// System.Collections.Concurrent.ConcurrentDictionary<int,object>
	// System.Collections.Generic.Dictionary<byte,object>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.Dictionary<object,byte>
	// System.Collections.Generic.Dictionary<MR.Battle.FX,object>
	// System.Collections.Generic.Dictionary<ulong,object>
	// System.Collections.Generic.Dictionary<object,int>
	// System.Collections.Generic.Dictionary<object,UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<byte,MR.Battle.BattleGroundScoreCD.ScoreData>
	// System.Collections.Generic.Dictionary.Enumerator<ulong,object>
	// System.Collections.Generic.Dictionary.Enumerator<byte,MR.Battle.BattleGroundScoreCD.ScoreData>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<MR.Battle.FX,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,int>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,int>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<ulong,object>
	// System.Collections.Generic.KeyValuePair<byte,MR.Battle.BattleGroundScoreCD.ScoreData>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.KeyValuePair<MR.Battle.FX,object>
	// System.Collections.Generic.LinkedList<object>
	// System.Collections.Generic.LinkedListNode<object>
	// System.Collections.Generic.List<MR.Battle.HitEffect>
	// System.Collections.Generic.List<Config.Equips.WeaponCardType>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.List<TrueSync.FP>
	// System.Collections.Generic.List<byte>
	// System.Collections.Generic.List<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<MR.Battle.FX>
	// System.Collections.Generic.List<ulong>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List.Enumerator<TrueSync.FP>
	// System.Collections.Generic.Queue<int>
	// System.Collections.Generic.Queue<object>
	// System.Comparison<object>
	// System.Converter<object,object>
	// System.Func<float>
	// System.Func<byte>
	// System.Func<int>
	// System.Func<Cysharp.Threading.Tasks.UniTaskVoid>
	// System.Func<Config.Equips.WeaponCardType>
	// System.Func<object>
	// System.Func<ulong>
	// System.Func<object,float>
	// System.Func<Cysharp.Threading.Tasks.AsyncUnit,Cysharp.Threading.Tasks.UniTask>
	// System.Func<object,int>
	// System.Func<object,object>
	// System.Func<object,ulong>
	// System.Func<object,byte>
	// System.Func<int,byte>
	// System.Func<int,Cysharp.Threading.Tasks.UniTaskVoid>
	// System.Func<object,System.Threading.CancellationToken,Cysharp.Threading.Tasks.UniTask>
	// System.IEquatable<MR.Battle.FX>
	// System.IEquatable<object>
	// System.Predicate<object>
	// System.Predicate<ulong>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<int>
	// System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Runtime.CompilerServices.TaskAwaiter<int>
	// System.Runtime.CompilerServices.ValueTaskAwaiter<object>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.Task<int>
	// System.Threading.Tasks.ValueTask<object>
	// System.ValueTuple<byte,int>
	// System.ValueTuple<byte,object>
	// System.ValueTuple<UnityEngine.Vector3,float,float>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<int>
	// UnityEngine.Events.UnityEvent<int>
	// UnityEngine.Events.UnityEvent<byte>
	// UnityEngine.InputSystem.Utilities.ReadOnlyArray<object>
	// UnityEngine.InputSystem.Utilities.ReadOnlyArray.Enumerator<object>
	// UnityEngine.Playables.ScriptPlayable<object>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>
	// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>
	// }}

	public void RefMethods()
	{
		// Cysharp.Threading.Tasks.UniTask.Awaiter<object> Cysharp.Threading.Tasks.AddressablesAsyncExtensions.GetAwaiter<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>)
		// Cysharp.Threading.Tasks.UniTask.Awaiter<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> Cysharp.Threading.Tasks.AddressablesAsyncExtensions.GetAwaiter<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>)
		// Cysharp.Threading.Tasks.UniTask<object> Cysharp.Threading.Tasks.AddressablesAsyncExtensions.WithCancellation<object>(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object>,System.Threading.CancellationToken)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsingNext.Scripts.CallbackSample.<FallTarget>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsingNext.Scripts.CallbackSample.<FallTarget>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,TestBattle.<LoadBattle>d__54>(Cysharp.Threading.Tasks.UniTask.Awaiter&,TestBattle.<LoadBattle>d__54&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,TestBattle.<LoadBattle>d__54>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,TestBattle.<LoadBattle>d__54&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,ScreenView_Battle.<LoadBattle>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,ScreenView_Battle.<LoadBattle>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Window_Main.<PlayAnimationItor2>d__30>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Window_Main.<PlayAnimationItor2>d__30&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<byte>,UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<SyncSlider>d__21>(Cysharp.Threading.Tasks.UniTask.Awaiter<byte>&,UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<SyncSlider>d__21&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,UniTaskTutorial.Advance.Scripts.PlayerControl.<<CheckPlayerInput>b__6_0>d>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,UniTaskTutorial.Advance.Scripts.PlayerControl.<<CheckPlayerInput>b__6_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.Advance.Scripts.PlayerControl.<<CheckPlayerInput>b__6_0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.Advance.Scripts.PlayerControl.<<CheckPlayerInput>b__6_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,PerformanceTest.<EmptyUnitTask>d__8>(Cysharp.Threading.Tasks.UniTask.Awaiter&,PerformanceTest.<EmptyUnitTask>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.Test.TaskExecuter.<RunDelayTask>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.Test.TaskExecuter.<RunDelayTask>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>,BDFramework.UFlux.UFluxUtils.<AsyncLoadScene>d__12>(Cysharp.Threading.Tasks.UniTask.Awaiter<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>&,BDFramework.UFlux.UFluxUtils.<AsyncLoadScene>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>,BDFramework.UFlux.UFluxUtils.<AsyncUnLoadkScene>d__13>(Cysharp.Threading.Tasks.UniTask.Awaiter<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance>&,BDFramework.UFlux.UFluxUtils.<AsyncUnLoadkScene>d__13&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UIEventsSample.<>c__DisplayClass10_0.<<CheckCooldownClickButton>b__0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UIEventsSample.<>c__DisplayClass10_0.<<CheckCooldownClickButton>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<RunSomeOne>d__12>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<RunSomeOne>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<UniTaskTutorial.Advance.Scripts.PlayerControl.<<CheckPlayerInput>b__6_0>d>(UniTaskTutorial.Advance.Scripts.PlayerControl.<<CheckPlayerInput>b__6_0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<SyncSlider>d__21>(UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<SyncSlider>d__21&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<Window_Main.<PlayAnimationItor2>d__30>(Window_Main.<PlayAnimationItor2>d__30&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<RunSomeOne>d__12>(UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<RunSomeOne>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<UIEventsSample.<>c__DisplayClass10_0.<<CheckCooldownClickButton>b__0>d>(UIEventsSample.<>c__DisplayClass10_0.<<CheckCooldownClickButton>b__0>d&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<TestBattle.<LoadBattle>d__54>(TestBattle.<LoadBattle>d__54&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<UniTaskTutorial.Test.TaskExecuter.<RunDelayTask>d__7>(UniTaskTutorial.Test.TaskExecuter.<RunDelayTask>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<ScreenView_Battle.<LoadBattle>d__10>(ScreenView_Battle.<LoadBattle>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<PerformanceTest.<EmptyUnitTask>d__8>(PerformanceTest.<EmptyUnitTask>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<BDFramework.UFlux.UFluxUtils.<AsyncLoadScene>d__12>(BDFramework.UFlux.UFluxUtils.<AsyncLoadScene>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<UniTaskTutorial.BaseUsingNext.Scripts.CallbackSample.<FallTarget>d__5>(UniTaskTutorial.BaseUsingNext.Scripts.CallbackSample.<FallTarget>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<BDFramework.UFlux.UFluxUtils.<AsyncUnLoadkScene>d__13>(BDFramework.UFlux.UFluxUtils.<AsyncUnLoadkScene>d__13&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<RunSomeOne>d__19>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<RunSomeOne>d__19&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitEndOfFrame>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitEndOfFrame>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitYield>d__0>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitYield>d__0&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitNextFrame>d__1>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitNextFrame>d__1&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,object>>,UniTaskTutorial.BaseUsingNext.Scripts.TimeoutTest.<GetRequest>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,object>>&,UniTaskTutorial.BaseUsingNext.Scripts.TimeoutTest.<GetRequest>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<UniTaskTutorial.BaseUsingNext.Scripts.TimeoutTest.<GetRequest>d__5>(UniTaskTutorial.BaseUsingNext.Scripts.TimeoutTest.<GetRequest>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<int>.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitYield>d__0>(UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitYield>d__0&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<int>.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitNextFrame>d__1>(UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitNextFrame>d__1&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<int>.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitEndOfFrame>d__2>(UniTaskTutorial.BaseUsing.Scripts.UniTaskAsyncSample_Wait.<WaitEndOfFrame>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<int>.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<RunSomeOne>d__19>(UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<RunSomeOne>d__19&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<BDFramework.UFlux.UFluxUtils.<LoadAssetUniTask>d__11<object>>(BDFramework.UFlux.UFluxUtils.<LoadAssetUniTask>d__11<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<float>,FireBulletSample.<CheckScoreChange>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<float>&,FireBulletSample.<CheckScoreChange>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,Window_Loding.<ProgressTask>d__4>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,Window_Loding.<ProgressTask>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Window_BattleReady.<ShowTips>d__39>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Window_BattleReady.<ShowTips>d__39&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,Window_BattleReady.<ReadyAll>d__25>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,Window_BattleReady.<ReadyAll>d__25&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<int>,UIEventsSample.<CheckDoubleClickButton>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<int>&,UIEventsSample.<CheckDoubleClickButton>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UIEventsSample.<CheckCooldownClickButton>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UIEventsSample.<CheckCooldownClickButton>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,Window_BattleReady.<ReadyAll>d__25>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,Window_BattleReady.<ReadyAll>d__25&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UIEventsSample.<SphereTweenScale>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UIEventsSample.<SphereTweenScale>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UIEventsSample.<CheckSphereClick>d__8>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UIEventsSample.<CheckSphereClick>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,FireBulletSample.<FlyBullet>d__12>(Cysharp.Threading.Tasks.UniTask.Awaiter&,FireBulletSample.<FlyBullet>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<int>,FireBulletSample.<OnClickFire>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<int>&,FireBulletSample.<OnClickFire>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,BattleResources.<AsPreLoadAnimation>d__12>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,BattleResources.<AsPreLoadAnimation>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,BattleResources.<AsPreLoadAvatar>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,BattleResources.<AsPreLoadAvatar>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UIEventsSample.<CheckDoubleClickButton>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UIEventsSample.<CheckDoubleClickButton>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,BattleResources.<AsPreLoadWeapon>d__13>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,BattleResources.<AsPreLoadWeapon>d__13&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,Window_Main.<PlayAnimationItor>d__28>(Cysharp.Threading.Tasks.UniTask.Awaiter&,Window_Main.<PlayAnimationItor>d__28&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickYieldRun>d__4>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickYieldRun>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<OnHpChange>d__20>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<OnHpChange>d__20&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<int>,UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<CheckFirstLowHp>d__19>(Cysharp.Threading.Tasks.UniTask.Awaiter<int>&,UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<CheckFirstLowHp>d__19&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<CheckHpChange>d__15>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<CheckHpChange>d__15&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.SwitchToThreadPoolAwaitable.Awaiter,UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickYieldRun>d__4>(Cysharp.Threading.Tasks.SwitchToThreadPoolAwaitable.Awaiter&,UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickYieldRun>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,UniTaskTutorial.BaseUsingNext.Scripts.TimeoutTest.<OnClickTest>d__6>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,UniTaskTutorial.BaseUsingNext.Scripts.TimeoutTest.<OnClickTest>d__6&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.SwitchToMainThreadAwaitable.Awaiter,UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickStandardRun>d__5>(Cysharp.Threading.Tasks.SwitchToMainThreadAwaitable.Awaiter&,UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickStandardRun>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickStandardRun>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickStandardRun>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickYieldRun>d__4>(System.Runtime.CompilerServices.TaskAwaiter<object>&,UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickYieldRun>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsingNext.Scripts.ForgetSample.<FallTarget>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsingNext.Scripts.ForgetSample.<FallTarget>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsingNext.Scripts.CallbackSample.<OnClickCallback>d__6>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsingNext.Scripts.CallbackSample.<OnClickCallback>d__6&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Window_BattleReady.<ReadyAll>d__25>(Window_BattleReady.<ReadyAll>d__25&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UIEventsSample.<CheckDoubleClickButton>d__11>(UIEventsSample.<CheckDoubleClickButton>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<CheckFirstLowHp>d__19>(UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<CheckFirstLowHp>d__19&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UIEventsSample.<CheckCooldownClickButton>d__10>(UIEventsSample.<CheckCooldownClickButton>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Window_BattleReady.<ShowTips>d__39>(Window_BattleReady.<ShowTips>d__39&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleResources.<AsPreLoadWeapon>d__13>(BattleResources.<AsPreLoadWeapon>d__13&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleResources.<AsPreLoadAnimation>d__12>(BattleResources.<AsPreLoadAnimation>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<BattleResources.<AsPreLoadAvatar>d__11>(BattleResources.<AsPreLoadAvatar>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<OnHpChange>d__20>(UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<OnHpChange>d__20&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Window_Loding.<ProgressTask>d__4>(Window_Loding.<ProgressTask>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<CheckHpChange>d__15>(UniTaskTutorial.Advance.Scripts.AsyncReactivePropertySample.<CheckHpChange>d__15&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<FireBulletSample.<CheckScoreChange>d__10>(FireBulletSample.<CheckScoreChange>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<FireBulletSample.<OnClickFire>d__11>(FireBulletSample.<OnClickFire>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickYieldRun>d__4>(UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickYieldRun>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<FireBulletSample.<FlyBullet>d__12>(FireBulletSample.<FlyBullet>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UniTaskTutorial.BaseUsingNext.Scripts.CallbackSample.<OnClickCallback>d__6>(UniTaskTutorial.BaseUsingNext.Scripts.CallbackSample.<OnClickCallback>d__6&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UniTaskTutorial.BaseUsingNext.Scripts.TimeoutTest.<OnClickTest>d__6>(UniTaskTutorial.BaseUsingNext.Scripts.TimeoutTest.<OnClickTest>d__6&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UniTaskTutorial.BaseUsingNext.Scripts.ForgetSample.<FallTarget>d__7>(UniTaskTutorial.BaseUsingNext.Scripts.ForgetSample.<FallTarget>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<Window_Main.<PlayAnimationItor>d__28>(Window_Main.<PlayAnimationItor>d__28&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickStandardRun>d__5>(UniTaskTutorial.BaseUsingNext.Scripts.ThreadSample.<OnClickStandardRun>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UIEventsSample.<SphereTweenScale>d__9>(UIEventsSample.<SphereTweenScale>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<UIEventsSample.<CheckSphereClick>d__8>(UIEventsSample.<CheckSphereClick>d__8&)
		// Cysharp.Threading.Tasks.UniTask.Awaiter Cysharp.Threading.Tasks.EnumeratorAsyncExtensions.GetAwaiter<object>(object)
		// Cysharp.Threading.Tasks.IUniTaskAsyncEnumerable<System.ValueTuple<UnityEngine.Vector3,float,float>> Cysharp.Threading.Tasks.Linq.UniTaskAsyncEnumerable.Create<System.ValueTuple<UnityEngine.Vector3,float,float>>(System.Func<Cysharp.Threading.Tasks.Linq.IAsyncWriter<System.ValueTuple<UnityEngine.Vector3,float,float>>,System.Threading.CancellationToken,Cysharp.Threading.Tasks.UniTask>)
		// Cysharp.Threading.Tasks.UniTask<int> Cysharp.Threading.Tasks.Linq.UniTaskAsyncEnumerable.FirstAsync<int>(Cysharp.Threading.Tasks.IUniTaskAsyncEnumerable<int>,System.Func<int,bool>,System.Threading.CancellationToken)
		// Cysharp.Threading.Tasks.UniTask Cysharp.Threading.Tasks.Linq.UniTaskAsyncEnumerable.ForEachAsync<object>(Cysharp.Threading.Tasks.IUniTaskAsyncEnumerable<object>,System.Action<object>,System.Threading.CancellationToken)
		// Cysharp.Threading.Tasks.UniTask Cysharp.Threading.Tasks.Linq.UniTaskAsyncEnumerable.ForEachAsync<int>(Cysharp.Threading.Tasks.IUniTaskAsyncEnumerable<int>,System.Action<int,int>,System.Threading.CancellationToken)
		// Cysharp.Threading.Tasks.UniTask Cysharp.Threading.Tasks.Linq.UniTaskAsyncEnumerable.ForEachAsync<Cysharp.Threading.Tasks.AsyncUnit>(Cysharp.Threading.Tasks.IUniTaskAsyncEnumerable<Cysharp.Threading.Tasks.AsyncUnit>,System.Action<Cysharp.Threading.Tasks.AsyncUnit,int>,System.Threading.CancellationToken)
		// Cysharp.Threading.Tasks.UniTask Cysharp.Threading.Tasks.Linq.UniTaskAsyncEnumerable.ForEachAsync<System.ValueTuple<UnityEngine.Vector3,float,float>>(Cysharp.Threading.Tasks.IUniTaskAsyncEnumerable<System.ValueTuple<UnityEngine.Vector3,float,float>>,System.Action<System.ValueTuple<UnityEngine.Vector3,float,float>>,System.Threading.CancellationToken)
		// Cysharp.Threading.Tasks.UniTask Cysharp.Threading.Tasks.Linq.UniTaskAsyncEnumerable.ForEachAwaitAsync<Cysharp.Threading.Tasks.AsyncUnit>(Cysharp.Threading.Tasks.IUniTaskAsyncEnumerable<Cysharp.Threading.Tasks.AsyncUnit>,System.Func<Cysharp.Threading.Tasks.AsyncUnit,Cysharp.Threading.Tasks.UniTask>,System.Threading.CancellationToken)
		// System.IDisposable Cysharp.Threading.Tasks.Linq.UniTaskAsyncEnumerable.Subscribe<int>(Cysharp.Threading.Tasks.IUniTaskAsyncEnumerable<int>,System.Func<int,Cysharp.Threading.Tasks.UniTaskVoid>)
		// Cysharp.Threading.Tasks.IUniTaskAsyncEnumerable<Cysharp.Threading.Tasks.AsyncUnit> Cysharp.Threading.Tasks.Linq.UniTaskAsyncEnumerable.Take<Cysharp.Threading.Tasks.AsyncUnit>(Cysharp.Threading.Tasks.IUniTaskAsyncEnumerable<Cysharp.Threading.Tasks.AsyncUnit>,int)
		// System.IProgress<float> Cysharp.Threading.Tasks.Progress.Create<float>(System.Action<float>)
		// Cysharp.Threading.Tasks.UniTask<float> Cysharp.Threading.Tasks.UniTask.WaitUntilValueChanged<object,float>(object,System.Func<object,float>,Cysharp.Threading.Tasks.PlayerLoopTiming,System.Collections.Generic.IEqualityComparer<float>,System.Threading.CancellationToken)
		// Cysharp.Threading.Tasks.UniTask<object[]> Cysharp.Threading.Tasks.UniTask.WhenAll<object>(Cysharp.Threading.Tasks.UniTask<object>[])
		// System.Void Cysharp.Threading.Tasks.UniTaskExtensions.Forget<byte>(Cysharp.Threading.Tasks.UniTask<byte>)
		// System.Void Cysharp.Threading.Tasks.UnityBindingExtensions.BindTo<int>(Cysharp.Threading.Tasks.AsyncReactiveProperty<int>,UnityEngine.UI.Text,bool)
		// System.Void ProtoBuf.Serializer.SerializeWithLengthPrefix<object>(System.IO.Stream,object,ProtoBuf.PrefixStyle)
		// ProtoBuf.Serializers.RepeatedSerializer<long[],long> ProtoBuf.Serializers.RepeatedSerializer.CreateVector<long>()
		// ProtoBuf.Serializers.RepeatedSerializer<int[],int> ProtoBuf.Serializers.RepeatedSerializer.CreateVector<int>()
		// ProtoBuf.Serializers.RepeatedSerializer<uint[],uint> ProtoBuf.Serializers.RepeatedSerializer.CreateVector<uint>()
		// ProtoBuf.Serializers.RepeatedSerializer<object[],object> ProtoBuf.Serializers.RepeatedSerializer.CreateVector<object>()
		// ProtoBuf.Serializers.RepeatedSerializer<ulong[],ulong> ProtoBuf.Serializers.RepeatedSerializer.CreateVector<ulong>()
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// System.Collections.Generic.List<object> System.Collections.Generic.List<object>.ConvertAll<object>(System.Converter<object,object>)
		// object System.Linq.Enumerable.FirstOrDefault<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// UnityEngine.RuntimePlatform[] System.Linq.Enumerable.ToArray<UnityEngine.RuntimePlatform>(System.Collections.Generic.IEnumerable<UnityEngine.RuntimePlatform>)
		// System.Collections.Generic.List<ulong> System.Linq.Enumerable.ToList<ulong>(System.Collections.Generic.IEnumerable<ulong>)
		// System.Collections.Generic.List<int> System.Linq.Enumerable.ToList<int>(System.Collections.Generic.IEnumerable<int>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Where<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// object System.Reflection.CustomAttributeExtensions.GetCustomAttribute<object>(System.Reflection.MemberInfo)
		// object System.Reflection.CustomAttributeExtensions.GetCustomAttribute<object>(System.Reflection.MemberInfo,bool)
		// System.Collections.Generic.IEnumerable<object> System.Reflection.CustomAttributeExtensions.GetCustomAttributes<object>(System.Reflection.MemberInfo)
		// System.Collections.Generic.IEnumerable<object> System.Reflection.CustomAttributeExtensions.GetCustomAttributes<object>(System.Reflection.MemberInfo,bool)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,MR.Net.Proto.TcpProtoHandle.<ReadFromStream>d__19>(System.Runtime.CompilerServices.TaskAwaiter&,MR.Net.Proto.TcpProtoHandle.<ReadFromStream>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,MR.Net.Proto.TcpProtoHandle.<ReadFromStream>d__19>(System.Runtime.CompilerServices.TaskAwaiter<object>&,MR.Net.Proto.TcpProtoHandle.<ReadFromStream>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<int>,MR.Net.Proto.TcpProtoHandle.<ReadFromStream>d__19>(System.Runtime.CompilerServices.TaskAwaiter<int>&,MR.Net.Proto.TcpProtoHandle.<ReadFromStream>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<int>,MR.Net.Proto.TcpProtoHandle.<ReadSkip>d__26>(System.Runtime.CompilerServices.TaskAwaiter<int>&,MR.Net.Proto.TcpProtoHandle.<ReadSkip>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<MR.Net.Proto.TcpProtoHandle.<ReadSkip>d__26>(MR.Net.Proto.TcpProtoHandle.<ReadSkip>d__26&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder.Start<MR.Net.Proto.TcpProtoHandle.<ReadFromStream>d__19>(MR.Net.Proto.TcpProtoHandle.<ReadFromStream>d__19&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<int>,MR.Net.Proto.TcpProtoHandle.<Deserialize>d__20>(System.Runtime.CompilerServices.TaskAwaiter<int>&,MR.Net.Proto.TcpProtoHandle.<Deserialize>d__20&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<int>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<int>,MR.Net.Proto.TcpProtoHandle.<ReadNumber>d__24>(System.Runtime.CompilerServices.TaskAwaiter<int>&,MR.Net.Proto.TcpProtoHandle.<ReadNumber>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<int>.Start<MR.Net.Proto.TcpProtoHandle.<ReadNumber>d__24>(MR.Net.Proto.TcpProtoHandle.<ReadNumber>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncTaskMethodBuilder<object>.Start<MR.Net.Proto.TcpProtoHandle.<Deserialize>d__20>(MR.Net.Proto.TcpProtoHandle.<Deserialize>d__20&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestDelay>d__13>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestDelay>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<int>,UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestEndOfFrame>d__22>(Cysharp.Threading.Tasks.UniTask.Awaiter<int>&,UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestEndOfFrame>d__22&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestDelayFrame>d__12>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestDelayFrame>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,int>>,UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<OnClickSecondRun>d__18>(Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,int>>&,UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<OnClickSecondRun>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<int>,UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<OnClickFirstRun>d__17>(Cysharp.Threading.Tasks.UniTask.Awaiter<int>&,UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<OnClickFirstRun>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<int>,UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestNextFrame>d__23>(Cysharp.Threading.Tasks.UniTask.Awaiter<int>&,UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestNextFrame>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<int>,UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestYield>d__24>(Cysharp.Threading.Tasks.UniTask.Awaiter<int>&,UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestYield>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.ResourceRequestAwaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickLoadText>d__10>(Cysharp.Threading.Tasks.UnityAsyncExtensions.ResourceRequestAwaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickLoadText>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickFirstRun>d__13>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickFirstRun>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickLoadScene>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickLoadScene>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickWhenAll>d__15>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickWhenAll>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<int>,UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickWhenAny>d__16>(Cysharp.Threading.Tasks.UniTask.Awaiter<int>&,UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickWhenAny>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickWebRequest>d__8>(Cysharp.Threading.Tasks.UnityAsyncExtensions.UnityWebRequestAsyncOperationAwaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickWebRequest>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,PerformanceTest.<OnClickUnitTaskTest>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter&,PerformanceTest.<OnClickUnitTaskTest>d__7&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter,MR.Net.Proto.TcpProtoHandle.<BeginRead>d__13>(System.Runtime.CompilerServices.TaskAwaiter&,MR.Net.Proto.TcpProtoHandle.<BeginRead>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.ValueTaskAwaiter<object>,MR.Net.Frame.KcpInstance.<Read>d__5>(System.Runtime.CompilerServices.ValueTaskAwaiter<object>&,MR.Net.Frame.KcpInstance.<Read>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,BDFramework.UFlux.CBA_Image.<SetProp_Sprite>d__1>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,BDFramework.UFlux.CBA_Image.<SetProp_Sprite>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,BDFramework.UFlux.CBA_Image.<SetProp_OverrideSprite>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,BDFramework.UFlux.CBA_Image.<SetProp_OverrideSprite>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickSecondRun>d__14>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickSecondRun>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickWebRequest>d__8>(Cysharp.Threading.Tasks.UniTask.Awaiter&,UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickWebRequest>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<MR.Net.Proto.TcpProtoHandle.<BeginRead>d__13>(MR.Net.Proto.TcpProtoHandle.<BeginRead>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<MR.Net.Frame.KcpInstance.<Read>d__5>(MR.Net.Frame.KcpInstance.<Read>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<BDFramework.UFlux.CBA_Image.<SetProp_OverrideSprite>d__2>(BDFramework.UFlux.CBA_Image.<SetProp_OverrideSprite>d__2&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<BDFramework.UFlux.CBA_Image.<SetProp_Sprite>d__1>(BDFramework.UFlux.CBA_Image.<SetProp_Sprite>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickLoadScene>d__9>(UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickLoadScene>d__9&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickWhenAny>d__16>(UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickWhenAny>d__16&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickWhenAll>d__15>(UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickWhenAll>d__15&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickSecondRun>d__14>(UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickSecondRun>d__14&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickFirstRun>d__13>(UniTaskTutorial.BaseUsing.Scripts.UniTaskWhenTest.<OnClickFirstRun>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestYield>d__24>(UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestYield>d__24&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestNextFrame>d__23>(UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestNextFrame>d__23&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestEndOfFrame>d__22>(UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestEndOfFrame>d__22&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestDelay>d__13>(UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestDelay>d__13&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestDelayFrame>d__12>(UniTaskTutorial.BaseUsing.Scripts.UniTaskWaitTest.<OnClickTestDelayFrame>d__12&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<OnClickSecondRun>d__18>(UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<OnClickSecondRun>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<OnClickFirstRun>d__17>(UniTaskTutorial.BaseUsing.Scripts.UniTaskCancelTest.<OnClickFirstRun>d__17&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickLoadText>d__10>(UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickLoadText>d__10&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickWebRequest>d__8>(UniTaskTutorial.BaseUsing.Scripts.UniTaskBaseTest.<OnClickWebRequest>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<PerformanceTest.<OnClickUnitTaskTest>d__7>(PerformanceTest.<OnClickUnitTaskTest>d__7&)
		// object System.Threading.Interlocked.CompareExchange<object>(object&,object,object)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<object>(object)
		// UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<object> UnityEngine.AddressableAssets.AssetReference.LoadAssetAsync<object>()
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object UnityEngine.GameObject.GetComponentInChildren<object>()
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// UnityEngine.Vector2 UnityEngine.InputSystem.InputAction.ReadValue<UnityEngine.Vector2>()
		// UnityEngine.Vector2 UnityEngine.InputSystem.InputAction.CallbackContext.ReadValue<UnityEngine.Vector2>()
		// System.Void UnityEngine.InputSystem.OnScreen.OnScreenControl.SendValueToControl<float>(float)
		// System.Void UnityEngine.InputSystem.OnScreen.OnScreenControl.SendValueToControl<UnityEngine.Vector2>(UnityEngine.Vector2)
		// object UnityEngine.Object.FindObjectOfType<object>()
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion)
		// object UnityEngine.Object.Instantiate<object>(object)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// double UnityEngine.Playables.PlayableExtensions.GetTime<UnityEngine.Playables.Playable>(UnityEngine.Playables.Playable)
		// UnityEngine.ResourceRequest UnityEngine.Resources.LoadAsync<object>(string)
	}
}