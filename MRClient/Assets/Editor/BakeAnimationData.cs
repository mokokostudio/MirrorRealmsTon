using MR.Battle.Timeline;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TrueSync;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Animations;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using static CharacterAnimationDataClip;

public class BakeAnimationData : OdinEditorWindow {
    [MenuItem("Tools/BakeAnimationData")]
    public static void Open() => GetWindow<BakeAnimationData>().Focus();

    private static List<string> s_RunClips = new List<string> {
        "Run_B",
        "Run_BL",
        "Run_BR",
        "Run_F",
        "Run_FL",
        "Run_FR",
        "Run_L",
        "Run_R",
        "RunAim1_B",
        "RunAim1_BL",
        "RunAim1_BR",
        "RunAim1_F",
        "RunAim1_FL",
        "RunAim1_FR",
        "RunAim1_L",
        "RunAim1_R",
        "Sprint" };

    [BoxGroup("Source")]
    [FolderPath]
    [LabelText("Path")]
    [ValidateInput("InputPathExist")]
    [ListDrawerSettings(Expanded = true)]
    public List<string> inputPaths;

    [BoxGroup("Source")]
    [AssetsOnly]
    [Required]
    public GameObject posturePrefab;

    [BoxGroup("Output")]
    [FolderPath]
    [LabelText("Path")]
    [ValidateInput("OutputPathExist")]
    public string outputPath;

    [Button("Bake")]
    [EnableIf("CanBake")]
    public void Bake() {
        if (!posturePrefab.GetComponent<Animator>()) {
            Debug.LogError("<Animator> not found on the prefab.");
            return;
        }
        var go = Instantiate(posturePrefab);
        go.hideFlags = HideFlags.DontSave;
        var animator = go.GetComponent<Animator>();
        var pd = go.AddComponent<PlayableDirector>();
        var step = 1 / 60f;

        foreach (var inputPath in inputPaths) {
            var fns = inputPath.Split("/");
            var outDir = $"{outputPath}/{fns[fns.Length - 2]}_{fns[fns.Length - 1]}";

            AssetDatabase.DeleteAsset(outDir);
            Directory.CreateDirectory(outDir);

            var group = CreateInstance<CharacterAnimationDataGroup>();

            var clips = new Dictionary<string, AnimResource>();
            FillAnimations(inputPath, clips);
            foreach (var data in clips) {
                var name = data.Key;
                var clip = data.Value.clip;
                var timeline = data.Value.timeline;
                var dataClip = CreateInstance<CharacterAnimationDataClip>();
                if (clip != null) {
                    AnimationPlayableUtilities.PlayClip(animator, clip, out var graph);
                    go.transform.position = Vector3.zero;
                    if (s_RunClips.Contains(name)) {
                        graph.Evaluate(clip.length);
                        dataClip.length = clip.length;
                        dataClip.clip = clip;
                        dataClip.offset = go.transform.position.magnitude;
                    } else {
                        var rp = TSVector.zero;
                        var offsets = new List<TSVector>();
                        var frames = (int)(clip.length * 60);
                        for (int i = 0; i < frames; i += 1) {
                            graph.Evaluate(step);
                            var np = go.transform.position.ToTSVector();
                            offsets.Add(np - rp);
                            rp = np;
                        }
                        dataClip.length = clip.length;
                        dataClip.clip = clip;
                        dataClip.moveData = offsets.ConvertAll(m => new TSVector2(m.x, m.z)).ToArray();
                    }
                }
                if (timeline != null) {
                    pd.time = 0;
                    pd.Play(timeline);

                    foreach (var track in timeline.GetOutputTracks())
                        switch (track) {
                            case AnimationTrack at:
                                pd.SetGenericBinding(at, go);
                                dataClip.length = at.duration;
                                var customAnims = new List<CustomAnim>();
                                foreach (var tlc in track.GetClips()) {
                                    var asset = tlc.asset as AnimationPlayableAsset;
                                    var config = new CustomAnim();
                                    config.clip = asset.clip;
                                    config.start = (float)tlc.start;
                                    config.end = (float)(tlc.start + tlc.duration);
                                    config.animStart = (float)tlc.clipIn;
                                    config.animSpeed = (float)tlc.timeScale;
                                    customAnims.Add(config);
                                }
                                foreach (var anim in customAnims) {
                                    var startTarget = customAnims.Find(m => m.start < anim.start && m.end > anim.start);
                                    if (startTarget != null)
                                        anim.startBlend = startTarget.end;
                                    else
                                        anim.startBlend = anim.start;
                                    var endTarget = customAnims.Find(m => m.end > anim.end && m.start < anim.end);
                                    if (endTarget != null)
                                        anim.endBlend = endTarget.start;
                                    else
                                        anim.endBlend = anim.end;
                                }
                                dataClip.customAnims = customAnims.ToArray();

                                go.transform.position = Vector3.zero;
                                var rp = TSVector.zero;
                                var offsets = new List<TSVector>();
                                var frames = (int)(dataClip.length * 60);
                                for (int i = 0; i < frames; i += 1) {
                                    pd.time += step;
                                    pd.Evaluate();
                                    var np = go.transform.position.ToTSVector();
                                    offsets.Add(np - rp);
                                    rp = np;
                                }
                                dataClip.moveData = offsets.ConvertAll(m => new TSVector2(m.x, m.z)).ToArray();
                                break;
                            case EquipTrack t:
                                dataClip.weaponPoint = t.Export();
                                break;
                            case LimitSkillTrack t:
                                dataClip.skillPoint = t.Export();
                                break;
                            case LimitMoveTrack t:
                                dataClip.movePoint = t.Export();
                                break;
                            case ComboSkillTrack t:
                                dataClip.comboSkillPoints = t.Export();
                                break;
                            case AttackBoxTrack t:
                                dataClip.attackBoxConfigs = t.Export();
                                break;
                            case BulletTrack t:
                                dataClip.bulletConfigs = t.Export();
                                break;
                            case EndureTrack t:
                                dataClip.endureConfigs = t.Export();
                                break;
                            case LimitDodgeTrack t:
                                dataClip.dodgePoint = t.Export();
                                break;
                            case MoveTrack t:
                                t.Export(dataClip.moveData);
                                break;
                            case FaceTrack t:
                                dataClip.faceConfigs = t.Export();
                                break;
                            case DefenseTrack t:
                                dataClip.defenseConfigs = t.Export();
                                break;
                            case RunModeTrack t:
                                dataClip.runModeConfigs = t.Export();
                                break;
                            case LookTargetTrack t:
                                dataClip.lookTargetConfigs = t.Export();
                                break;
                            case ConfigTrack t:
                                dataClip.moveable = t.moveable;
                                break;
                            case SPTrack t:
                                dataClip.spConfigs = t.Export();
                                break;
                            case ImpulseTrack t:
                                dataClip.impulseConfigs = t.Export();
                                break;
                            case EffectTrack t:
                                if (dataClip.effectConfigs == null)
                                    dataClip.effectConfigs = t.Export();
                                else {
                                    var list = new List<EffectConfig>(dataClip.effectConfigs);
                                    list.AddRange(t.Export());
                                    dataClip.effectConfigs = list.ToArray();
                                }
                                break;
                        }
                }
                var p = $"{outDir}/{name}.asset";
                AssetDatabase.CreateAsset(dataClip, p);
                var ar = new AssetReference(AssetDatabase.AssetPathToGUID(p));
                group.dataClips.Add(new AssetNameReference(name, ar));
            }

            AssetDatabase.CreateAsset(group, $"{outDir}/_Group.asset");
        }

        DestroyImmediate(go);
        AssetDatabase.Refresh();
    }

    public bool InputPathExist => !inputPaths.Exists(m => !Directory.Exists(m));
    public bool OutputPathExist => Directory.Exists(outputPath);
    public bool CanBake => InputPathExist && OutputPathExist && posturePrefab != null;

    private void FillAnimations(string path, Dictionary<string, AnimResource> data, string[] dirs = null) {
        if (dirs == null)
            dirs = path.Split('/', '\\');
        foreach (var file in Directory.GetFiles(path)) {
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(file);
            if (clip != null) {
                var name = clip.name;
                name = name.Replace(dirs[dirs.Length - 1] + "_", "");
                name = name.Replace(dirs[dirs.Length - 2] + "_", "");
                data[name] = new AnimResource { clip = clip };
                continue;
            }
            var timeline = AssetDatabase.LoadAssetAtPath<TimelineAsset>(file);
            if (timeline != null) {
                var name = timeline.name;
                name = name.Replace(dirs[dirs.Length - 1] + "_", "");
                name = name.Replace(dirs[dirs.Length - 2] + "_", "");
                data[name] = new AnimResource { timeline = timeline };
                continue;
            }
        }
        foreach (var dir in Directory.GetDirectories(path))
            FillAnimations(dir, data, dirs);
    }

    private struct AnimResource {
        public AnimationClip clip;
        public TimelineAsset timeline;
    }
}
