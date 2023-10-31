using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class AuAnimator {

    private PlayableGraph m_Graph;
    private AnimationLayerMixerPlayable m_Root;
    private AnimationClip m_DefaultClip;
    private Func<float> m_MoveSpeedGet;
    private Func<float> m_FaceGet;

    public AuAnimator(string name, Animator animator, AnimationClip defaultClip, Func<float> moveSpeedGet, Func<float> faceGet) {
        m_DefaultClip = defaultClip;
        m_MoveSpeedGet = moveSpeedGet;
        m_FaceGet = faceGet;
        m_Graph = PlayableGraph.Create(name);
        m_Root = AnimationLayerMixerPlayable.Create(m_Graph, 2);
        var output = AnimationPlayableOutput.Create(m_Graph, "AnimatorOutput", animator);
        output.SetSourcePlayable(m_Root);
        m_Root.ConnectInput(0, GetDefualt(), 0, 1);

        var halfMask = new AvatarMask();
        halfMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.Root, false);
        halfMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.Body, false);
        halfMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.Head, false);
        halfMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftLeg, false);
        halfMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.RightLeg, false);
        halfMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.LeftFootIK, false);
        halfMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.RightFootIK, false);
        m_Root.SetLayerMaskFromAvatarMask(1, halfMask);
    }
    private Playable GetDefualt() => GetPlayable(m_DefaultClip);

    public void Evaluate(float time) => m_Graph.Evaluate(time);

    public void SetDefault(AnimationClip defaultClip) => m_DefaultClip = defaultClip;

    public void PlaySingle(AnimationClip clip) {
        var p = SinglePB.Create(m_Graph, GetPlayable(clip), GetDefualt);
        //同步过渡，进行时过渡，暂停帧过渡
        TransPB.Create(m_Root, p, .2f);
    }

    public void PlaySingle(CustomAnim[] anims) {
        var p = SinglePB.Create(m_Graph, ListPB.Create(m_Graph, anims), GetDefualt);
        TransPB.Create(m_Root, p, .2f);
    }

    public void PlaySingleWithMove(AnimationClip clip) {
        var p = GetPlayable(clip);
        TransLayerPB.Create(m_Root, p, .2f);
    }

    public void PlaySingleWithMove(CustomAnim[] anims) {
        var p = ListPB.Create(m_Graph, anims);
        TransLayerPB.Create(m_Root, p, .2f);
    }

    public void PlayBlendMove(AnimationClip[] clips, float[] moveDistance) {
        var p = BlendMovePB.Create(m_Graph, clips, moveDistance, m_MoveSpeedGet, m_FaceGet);
        TransPB.Create(m_Root, p, .2f);
    }

    private Playable GetPlayable(AnimationClip clip) {
        var p = AnimationClipPlayable.Create(m_Graph, clip);
        if (!clip.isLooping)
            p.SetDuration(clip.length);
        return p;
    }

    public void Dispose() {
        m_Graph.Destroy();
    }

    private class SinglePB : PlayableBehaviour {
        private Func<Playable> m_GetIdle;
        private Playable m_Input;

        public static Playable Create(PlayableGraph graph, Playable input, Func<Playable> defaultGet) {
            if (input.GetDuration() == 0)
                return input;
            else {
                var p = ScriptPlayable<SinglePB>.Create(graph, 1);
                var b = p.GetBehaviour();
                b.m_Input = input;
                b.m_GetIdle = defaultGet;
                p.ConnectInput(0, input, 0, 1);
                return p;
            }
        }

        public override void PrepareFrame(Playable playable, FrameData info) {
            var a = playable.GetTime();
            var b = playable.GetDuration();
            if (m_Input.GetTime() >= m_Input.GetDuration()) {
                var parent = playable.GetOutput(0);
                parent.DisconnectInput(0);
                parent.ConnectInput(0, m_GetIdle(), 0, 1);
            }
        }
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

        public static Playable Create(Playable parent, Playable target, float duration) {
            var graph = parent.GetGraph();
            var p = ScriptPlayable<TransPB>.Create(graph, 1);
            var b = p.GetBehaviour();
            b.m_Target = target;
            p.SetDuration(duration);
            var org = parent.GetInput(0);
            parent.DisconnectInput(0);
            parent.ConnectInput(0, p, 0, 1);
            var m = b.m_Mixer = AnimationMixerPlayable.Create(graph, 2);
            p.ConnectInput(0, m, 0, 1);
            var bg = AnimationMixerPlayable.Create(graph, 1);
            bg.ConnectInput(0, org, 0, 1);
            m.ConnectInput(0, bg, 0, 1);
            m.ConnectInput(1, target, 0, 0);

            if (org.GetPlayableType() == typeof(BlendMovePB) && target.GetPlayableType() == typeof(BlendMovePB)) {
                target.GetInput(0).SetTime(org.GetInput(0).GetTime());
            } else
                org.SetSpeed(0);
            return p;
        }

        public override void PrepareFrame(Playable playable, FrameData info) {
            var t = playable.GetTime();
            var d = playable.GetDuration();
            if (t < d) {
                var a = (float)(t / d);
                m_Mixer.SetInputWeight(0, 1 - a);
                m_Mixer.SetInputWeight(1, a);
            } else {
                var parent = playable.GetOutput(0);
                m_Mixer.DisconnectInput(1);
                parent.DisconnectInput(0);
                parent.ConnectInput(0, m_Target, 0, 1);
            }
        }
    }

    private class TransLayerPB : PlayableBehaviour {
        private AnimationLayerMixerPlayable m_Parent;
        private Playable m_Target;

        private AnimationMixerPlayable m_Mixer;
        private float m_TransDuration;
        public static Playable Create(AnimationLayerMixerPlayable parent, Playable target, float duration) {
            var graph = parent.GetGraph();
            var p = ScriptPlayable<TransLayerPB>.Create(graph, 1);
            var b = p.GetBehaviour();
            b.m_Parent = parent;
            b.m_Target = target;
            b.m_TransDuration = duration;
            p.ConnectInput(0, target, 0, 1);
            var dur = target.GetDuration();
            p.SetDuration(dur);
            var org = parent.GetInput(1);
            if (org.IsNull()) {
                parent.ConnectInput(1, p, 0, 1);
            } else {
            }
            return p;
        }

        public override void PrepareFrame(Playable playable, FrameData info) {
            var t = playable.GetTime();
            var d = playable.GetDuration();
            if (t < d) {
                var a = Mathf.Clamp((float)Math.Min(t, d - t) / m_TransDuration, 0, 1);
                m_Parent.SetInputWeight(1, a);
            } else {
                m_Parent.DisconnectInput(1);
            }
        }
    }

    private class ListPB : PlayableBehaviour {
        private CustomAnim[] m_Anims;
        private AnimationMixerPlayable m_Mixer;
        private AnimationClipPlayable[] m_Clips;
        public static Playable Create(PlayableGraph graph, CustomAnim[] anims) {
            var p = ScriptPlayable<ListPB>.Create(graph, 1);
            var b = p.GetBehaviour();
            b.m_Anims = anims;
            b.m_Clips = new AnimationClipPlayable[anims.Length];
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
                    m_Mixer.SetInputWeight(i, Mathf.Lerp(0, 1, (anim.startBlend - t) / (anim.startBlend - anim.start)));
                else if (t < anim.endBlend)
                    m_Mixer.SetInputWeight(i, 1);
                else if (t < anim.end)
                    m_Mixer.SetInputWeight(i, Mathf.Lerp(1, 0, (anim.end - t) / (anim.end - anim.endBlend)));
                else
                    m_Mixer.SetInputWeight(i, 0);
                clip.SetTime((t - anim.start) * anim.animSpeed + anim.animStart);
            }
        }
    }
}