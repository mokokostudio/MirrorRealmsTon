using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [APIName("分歧")]
    public class EFork : EAPIAct {
        public List<EELSEIF> @elseif = new List<EELSEIF> { new EELSEIF() };
        public EELSE @else = new EELSE();

        public override string ToString() {
            if (@else == null)
                @else = new EELSE();
            if (@elseif == null)
                @elseif = new List<EELSEIF> { new EELSEIF() };
            int num = @else.actions.Count;
            foreach (var item in elseif)
                num += item.actions.Count;
            return $"分歧 根据条件总计执行{num}项目";
        }

        public override void Check() {
            foreach (var elif in elseif) 
                elif.Check();
            @else.Check();
        }

        public class EELSEIF {
            [LabelText("条件")]
            public EAPIReturn<bool> cond;
            [LabelText("行为列表")]
            public List<EAPIAct> actions = new List<EAPIAct>();
            public override string ToString() => $"{GetName(cond)}成立,执行{actions.Count}项目";

            public void Check() {
                if (cond == null) 
                    throw new Exception("分歧 条件未配置");
                cond.Check();

                foreach (var ac in actions)
                    ac.Check();
            }
        }

        public class EELSE {
            [LabelText("行为列表")]
            public List<EAPIAct> actions = new List<EAPIAct>();

            public override string ToString() => $"执行{actions.Count}项目";

            public void Check() {
                foreach (var ac in actions) 
                    ac.Check();
            }
        }
    }
}
