using MR.Battle;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

public class CharacterAnimationDataClip : SerializedScriptableObject {
    public AnimationClip clip;
    public CustomAnim[] customAnims;
    public FP length;
    public TSVector2[] moveData;
    public FP offset;
    public FP weaponPoint = -1;
    public FP skillPoint;
    public FP movePoint;
    public FP dodgePoint;
    public ComboSkillPoint[] comboSkillPoints;
    public AttackBoxConfig[] attackBoxConfigs;
    public BulletConfig[] bulletConfigs;
    public EndureConfig[] endureConfigs;
    public FaceConfig[] faceConfigs;
    public SimpleConfig[] defenseConfigs;
    public RunModeConfig[] runModeConfigs;
    public SimpleConfig[] lookTargetConfigs;
    public SPConfig[] spConfigs;
    public ImpulseConfig[] impulseConfigs;
    public EffectConfig[] effectConfigs;
    public bool moveable;

    [Serializable]
    public class ComboSkillPoint {
        public FP startPoint;
        public FP endPoint;
        public bool release;
        public string skill;
    }

    [Serializable]
    public class AttackBoxConfig {
        public FP startPoint;
        public FP endPoint;
        public TSVector position;
        public TSVector rotation;
        public TSVector scale;
        public int atkLv;
        public HitType hitType;
        public int sp;
        public int hp;
        public int paush;
    }

    [Serializable]
    public class BulletConfig {
        public FP startPoint;
        public TSVector position;
        public TSVector rotation;
        public int atkLv;
        public HitType hitType;
        public int sp;
        public int hp;

        public bool faceTarget;
        public FP speed;
        public FP trackDeg;
        public FP trackTime;
    }

    [Serializable]
    public class EndureConfig {
        public FP startPoint;
        public FP endPoint;
        public int defLV;
    }

    [Serializable]
    public class FaceConfig {
        public FP point;
        public bool lockTarget;
        public FP maxAngle;
    }

    [Serializable]
    public class SimpleConfig {
        public FP startPoint;
        public FP endPoint;
    }

    [Serializable]
    public class RunModeConfig {
        public FP startPoint;
        public FP endPoint;
        public string mode;
    }

    [Serializable]
    public class SPConfig {
        public FP startPoint;
        public int sp;
    }

    [Serializable]
    public class ImpulseConfig {
        public FP startPoint;
        public bool onlySelf;
        public TSVector power;
    }

    [Serializable]
    public class EffectConfig {
        public FP startPoint;
        public FP endPoint;
        public GameObject target;
        public bool follow;
        public float speed;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }
}
