using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEditor.Editors.Domain {
    [APIName("遍历数组")]
    public class EArrayForeach<T> : EAPIAct {
        public EArrayForeach() { }
        public EArrayForeach(EArray<T> v) => array = v;
        [LabelText("数组")]
        public EArray<T> array;
        public EVariable<T> item = new EVariable<T>();
        [LabelText("遍历执行")]
        public List<EAPIAct> actions = new List<EAPIAct>();
        public override string ToString() => $"遍历{GetName(array)}，执行{actions.Count}项";
        public override void Check() {
            if (array == null) 
                throw new Exception("遍历数组 数组未配置");
            array.Check();
            foreach (var ac in actions) 
                ac.Check();
        }
    }
}
