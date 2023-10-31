using ProtoBuf.WellKnownTypes;
using System;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AuAnimator2 {
    private AnimationClip m_Idle;
    private AnimationClip[] m_Moves;
    private float[] m_MoveDistances;
    private Func<float> m_MoveSpeedGet;
    private Func<float> m_FaceGet;

    private PlayableGraph m_Graph;
    private AnimationScriptPlayable m_LayerMix;
    private AnimationJob m_LayerJob;
    private State m_State;

    public AuAnimator2(string name, Animator animator, AnimationClip idle, AnimationClip[] moves, float[] moveDistances, Func<float> moveSpeedGet, Func<float> faceGet) {
        m_Idle = idle;
        m_Moves = moves;
        m_MoveDistances = moveDistances;
        m_MoveSpeedGet = moveSpeedGet;
        m_FaceGet = faceGet;

        m_Graph = PlayableGraph.Create(name);
        //m_LayerMix = AnimationLayerMixerPlayable.Create(m_Graph, 2);

        //var mask = new AvatarMask();
        //mask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.Root, false);
        //mask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.Body, false);
        //mask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftLeg, false);
        //mask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.RightLeg, false);
        //mask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftFootIK, false);
        //mask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.RightFootIK, false);
        //m_LayerMix.SetLayerMaskFromAvatarMask(1, mask);

        //m_LayerMix.ConnectInput(0, CreateIdlePB(), 0, 1);

        var lastbone = (int)HumanBodyBones.LastBone;
        var handles = new NativeArray<TransformStreamHandle>(lastbone, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        var weights = new NativeArray<float>(lastbone, Allocator.Persistent);
        for (int i = 0; i < lastbone; i++) {
            Debug.Log($"{(HumanBodyBones)i}:{animator.GetBoneTransform((HumanBodyBones)i)}");
            handles[i] = animator.BindStreamTransform(animator.GetBoneTransform((HumanBodyBones)i));
            weights[i] = 1;
        }
        weights[(int)HumanBodyBones.Hips] = 0;
        weights[(int)HumanBodyBones.Spine] = 0;
        weights[(int)HumanBodyBones.Chest] = 0;
        weights[(int)HumanBodyBones.UpperChest] = 0;
        weights[(int)HumanBodyBones.LeftUpperLeg] = 0;
        weights[(int)HumanBodyBones.LeftLowerLeg] = 0;
        weights[(int)HumanBodyBones.LeftFoot] = 0;
        weights[(int)HumanBodyBones.LeftToes] = 0;
        weights[(int)HumanBodyBones.RightUpperLeg] = 0;
        weights[(int)HumanBodyBones.RightLowerLeg] = 0;
        weights[(int)HumanBodyBones.RightFoot] = 0;
        weights[(int)HumanBodyBones.RightToes] = 0;

        m_LayerJob = new AnimationJob {
            handles = handles,
            boneWeights = weights,
            weight = 0
        };
        m_LayerMix = AnimationScriptPlayable.Create(m_Graph, m_LayerJob, 2);
        m_LayerMix.ConnectInput(0, CreateIdlePB(), 0, 0);

        AnimationPlayableOutput.Create(m_Graph, "Output", animator).SetSourcePlayable(m_LayerMix);
    }

    public void Evaluate(float time) {
        var pb = m_LayerMix.GetInput(1);
        if (pb.IsValid()) {
            var w = m_LayerJob.weight;
            if (pb.GetTime() + .2f > pb.GetDuration() || pb.GetSpeed() == 0)
                w -= time * 5;
            else if (w < 1)
                w = Math.Min(1, w + time * 5);
            if (w > 0) {
                m_LayerJob.weight = w;
                m_LayerMix.SetJobData(m_LayerJob);
            } else
                m_LayerMix.DisconnectInput(1);
        }
        m_Graph.Evaluate(time);
    }

    public void SetIdle(AnimationClip idle) {
        m_Idle = idle;
    }

    public void SetMove(AnimationClip[] moves, float[] moveDistance) {
        if (m_Moves != moves) {
            m_Moves = moves;
            m_MoveDistances = moveDistance;
            if (m_State == State.Move)
                TurnMove(CreateMovePB());
        }
    }

    public void PlayIdle() {
        m_State = State.Idle;
        PlayAnim(CreateIdlePB());
    }

    public void PlayMove() {
        m_State = State.Move;
        PlayAnim(CreateMovePB());
    }

    public void PlayAnim(AnimationClip clip, bool blendMove) {
        if (blendMove) {
            if (m_State != State.Move) {
                PlayAnim(CreateAnimPB(clip, PlayIdle));
                m_State = State.Idle;
            }
            PlayBlend(CreateAnimPB(clip, null));
        } else {
            PlayAnim(CreateAnimPB(clip, PlayIdle));
            CloseBlend();
            m_State = State.Custom;
        }
    }

    public void PlayAnim(CustomAnim[] anims, bool blendMove) {
        if (blendMove) {
            if (m_State != State.Move) {
                PlayAnim(CreateAnimPB(anims, PlayIdle));
                m_State = State.Idle;
            }
            PlayBlend(CreateAnimPB(anims, null));
        } else {
            PlayAnim(CreateAnimPB(anims, PlayIdle));
            CloseBlend();
            m_State = State.Custom;
        }
    }

    private Playable CreatePlayable(AnimationClip clip) {
        var p = AnimationClipPlayable.Create(m_Graph, clip);
        if (!clip.isLooping)
            p.SetDuration(clip.length);
        return p;
    }

    private Playable CreateIdlePB() {
        return CreatePlayable(m_Idle);
    }

    private Playable CreateMovePB() {
        return BlendMovePB.Create(m_Graph, m_Moves, m_MoveDistances, m_MoveSpeedGet, m_FaceGet);
    }

    private Playable CreateAnimPB(AnimationClip clip, Action finishCall) {
        return SinglePB.Create(m_Graph, CreatePlayable(clip), finishCall);
    }

    private Playable CreateAnimPB(CustomAnim[] anims, Action finishCall) {
        return SinglePB.Create(m_Graph, ListPB.Create(m_Graph, anims), finishCall);
    }

    private void PlayAnim(Playable pb) {
        TransPB.Create(m_LayerMix, 0, pb, .2f, false);
    }

    private void TurnMove(Playable pb) {
        TransPB.Create(m_LayerMix, 0, pb, .2f, true);
    }

    private void PlayBlend(Playable pb) {
        if (m_LayerMix.GetInput(1).IsValid()) {
            TransPB.Create(m_LayerMix, 1, pb, .2f, false);
        } else
            m_LayerMix.ConnectInput(1, pb, 0, 0);
    }

    public void CloseBlend() {
        var pb = m_LayerMix.GetInput(1);
        if (pb.IsValid())
            pb.SetSpeed(0);
    }

    public void Dispose() {
        m_Graph.Destroy();
        m_LayerJob.handles.Dispose();
        m_LayerJob.boneWeights.Dispose();
    }

    private enum State {
        Idle,
        Move,
        Custom
    }

    private class BlendMovePB : PlayableBehaviour {

        private Func<float> m_SpeedGet;
        private Func<float> m_BlendGet;
        private AnimationMixerPlayable m_Mixer;
        private float[] m_MoveDistance;
        private AnimationClipPlayable[] m_Clips;
        private float[] m_MoveDuration;

        public static Playable Create(PlayableGraph graph, AnimationClip[] clips, float[] moveDistance, Func<float> speedGet, Func<float> blendGet) {
            var p = ScriptPlayable<BlendMovePB>.Create(graph, 1);
            var b = p.GetBehaviour();
            b.m_MoveDistance = moveDistance;
            b.m_SpeedGet = speedGet;
            b.m_BlendGet = blendGet;
            var m = b.m_Mixer = AnimationMixerPlayable.Create(graph, clips.Length);
            p.ConnectInput(0, m, 0, 1);
            b.m_Clips = new AnimationClipPlayable[clips.Length];
            b.m_MoveDuration = new float[clips.Length];
            for (int i = 0; i < clips.Length; i++) {
                var clip = clips[i];
                var c = b.m_Clips[i] = AnimationClipPlayable.Create(graph, clip);
                c.SetSpeed(0);
                m.ConnectInput(i, c, 0);
                b.m_MoveDuration[i] = clip.length;
            }
            b.UpdateMixer();
            return p;
        }

        public override void PrepareFrame(Playable playable, FrameData info) {
            UpdateMixer();
            var t = m_Mixer.GetTime();
            for (int i = 0; i < m_Clips.Length; i++)
                m_Clips[i].SetTime(t * m_MoveDuration[i]);
        }

        private void UpdateMixer() {
            var speed = m_SpeedGet();
            var blend = m_BlendGet();

            var step = 1f / m_MoveDistance.Length;
            var moveDistance = 0f;

            for (int i = 0; i < m_MoveDistance.Length; i++, blend -= step) {
                var a = 0f;
                var c = blend;
                if (i == 0 && blend > .5f)
                    c -= 1;
                if (c >= 0 && c < step)
                    a = 1 - c / step;
                else if (c < 0 && c > -step)
                    a = 1 + c / step;
                if (a > 0)
                    moveDistance += m_MoveDistance[i] * a;
                m_Mixer.SetInputWeight(i, a);
            }
            m_Mixer.SetSpeed(speed / moveDistance);
        }
    }

    private class TransPB : PlayableBehaviour {
        private Playable m_Target;

        private AnimationMixerPlayable m_Mixer;
        private int m_Input;
        private float m_Duration;

        public static void Create(Playable parent, int input, Playable target, float duration, bool syncProgress) {
            var graph = parent.GetGraph();
            var p = ScriptPlayable<TransPB>.Create(graph, 1);
            var b = p.GetBehaviour();
            b.m_Target = target;
            b.m_Input = input;
            b.m_Duration = duration;
            p.SetDuration(target.GetDuration());
            var org = parent.GetInput(input);
            parent.DisconnectInput(input);
            parent.ConnectInput(input, p, 0, 1);
            var m = b.m_Mixer = AnimationMixerPlayable.Create(graph, 2);
            p.ConnectInput(0, m, 0, 1);
            m.ConnectInput(0, org, 0, 1);
            m.ConnectInput(1, target, 0, 0);

            if (syncProgress) {
                target.GetInput(0).SetTime(org.GetInput(0).GetTime());
            } else
                org.SetSpeed(0);
        }

        public override void PrepareFrame(Playable playable, FrameData info) {
            var t = playable.GetTime();
            if (t < m_Duration) {
                var a = (float)(t / m_Duration);
                m_Mixer.SetInputWeight(0, 1 - a);
                m_Mixer.SetInputWeight(1, a);
            } else {
                var parent = playable.GetOutput(0);
                m_Mixer.DisconnectInput(1);
                parent.DisconnectInput(m_Input);
                parent.ConnectInput(m_Input, m_Target, 0, 1);
            }
        }
    }

    private class SinglePB : PlayableBehaviour {
        private Action m_Callback;
        private double m_Duration;
        public static Playable Create(PlayableGraph graph, Playable input, Action callback) {
            if (input.GetDuration() == 0)
                return input;
            else {
                var p = ScriptPlayable<SinglePB>.Create(graph, 1);
                var dur = input.GetDuration();
                p.SetDuration(dur);
                var b = p.GetBehaviour();
                b.m_Duration = dur;
                b.m_Callback = callback;
                p.ConnectInput(0, input, 0, 1);
                return p;
            }
        }

        public override void PrepareFrame(Playable playable, FrameData info) {
            if (playable.GetTime() >= m_Duration) {
                playable.SetTime(m_Duration);
                playable.SetSpeed(0);
                m_Callback?.Invoke();
                m_Callback = null;
            }
        }
    }

    private class ListPB : PlayableBehaviour {
        private CustomAnim[] m_Anims;
        private AnimationMixerPlayable m_Mixer;
        private Playable[] m_Clips;
        public static Playable Create(PlayableGraph graph, CustomAnim[] anims) {
            var p = ScriptPlayable<ListPB>.Create(graph, 1);
            var b = p.GetBehaviour();
            b.m_Anims = anims;
            b.m_Clips = new Playable[anims.Length];
            var m = b.m_Mixer = AnimationMixerPlayable.Create(graph, anims.Length);
            p.ConnectInput(0, m, 0, 1);
            float maxEnd = 0;
            for (int i = 0; i < anims.Length; i++) {
                var anim = anims[i];
                var c = b.m_Clips[i] = AnimationClipPlayable.Create(graph, anim.clip);
                c.SetSpeed(0);
                m.ConnectInput(i, c, 0);
                if (anim.end > maxEnd)
                    maxEnd = anim.end;
            }
            b.m_Mixer.SetDuration(maxEnd);
            p.SetDuration(maxEnd);
            return p;
        }
        public override void PrepareFrame(Playable playable, FrameData info) {
            var t = (float)m_Mixer.GetTime();
            for (int i = 0; i < m_Anims.Length; i++) {
                var anim = m_Anims[i];
                var clip = m_Clips[i];
                if (t < anim.start)
                    m_Mixer.SetInputWeight(i, 0);
                else if (t < anim.startBlend)
                    m_Mixer.SetInputWeight(i, Mathf.Lerp(0, 1, (t - anim.start) / (anim.startBlend - anim.start)));
                else if (t <= anim.endBlend)
                    m_Mixer.SetInputWeight(i, 1);
                else if (t <= anim.end)
                    m_Mixer.SetInputWeight(i, Mathf.Lerp(1, 0, (t - anim.endBlend) / (anim.end - anim.endBlend)));
                else
                    m_Mixer.SetInputWeight(i, 0);
                clip.SetTime((t - anim.start) * anim.animSpeed + anim.animStart);
            }
        }
    }

    public struct AnimationJob : IAnimationJob {


        public NativeArray<TransformStreamHandle> handles;
        public NativeArray<float> boneWeights;
        public float weight;

        public void ProcessRootMotion(AnimationStream stream) {
        }

        public void ProcessAnimation(AnimationStream stream) {
            var streamA = stream.GetInputStream(0);
            var streamB = stream.GetInputStream(1);

            var ou = stream.AsHuman();
            var oa = streamA.AsHuman();

            ou.SetGoalLocalPosition(AvatarIKGoal.LeftFoot, oa.GetGoalLocalPosition(AvatarIKGoal.LeftFoot));
            ou.SetGoalLocalPosition(AvatarIKGoal.RightFoot, oa.GetGoalLocalPosition(AvatarIKGoal.RightFoot));
            ou.SetGoalLocalRotation(AvatarIKGoal.LeftFoot, oa.GetGoalLocalRotation(AvatarIKGoal.LeftFoot));
            ou.SetGoalLocalRotation(AvatarIKGoal.RightFoot, oa.GetGoalLocalRotation(AvatarIKGoal.RightFoot));

            if (!streamB.isValid)
                return;

            var numHandles = handles.Length;
            for (var i = 0; i < numHandles; ++i) {
                var handle = handles[i];

                var posA = handle.GetLocalPosition(streamA);
                var posB = handle.GetLocalPosition(streamB);
                handle.SetLocalPosition(stream, Vector3.Lerp(posA, posB, weight * boneWeights[i]));

                var rotA = handle.GetLocalRotation(streamA);
                var rotB = handle.GetLocalRotation(streamB);
                handle.SetLocalRotation(stream, Quaternion.Slerp(rotA, rotB, weight * boneWeights[i]));
            }
        }
    }
}
