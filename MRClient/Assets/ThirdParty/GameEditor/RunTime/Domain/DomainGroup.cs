using System.Collections.Generic;
namespace GameEditor.Domain {

    public class DomainGroup<T> where T : RunHandle {
        public virtual Domain CreateDomain() => new Domain();
        public virtual Fork CreateFork() => new Fork();
        public virtual ArrayAdd<U> CreateArrayAdd<U>() => new ArrayAdd<U>();
        public virtual ArrayForeach<U> CreateArrayForeach<U>() => new ArrayForeach<U>();
        public virtual ArrayRemove<U> CreateArrayRemove<U>() => new ArrayRemove<U>();
        public virtual GetArrayLength<U> CreateArrayLength<U>() => new GetArrayLength<U>();
        public virtual CreateArray<U> CreateCreateArray<U>() => new CreateArray<U>();
        public virtual CreateVariable<U> CreateCreateVariable<U>() => new CreateVariable<U>();
        public virtual SetVariable<U> CreateSetVariable<U>() => new SetVariable<U>();
        public virtual Static<U> CreateStatic<U>() => new Static<U>();
        public virtual VariablePicker<U> CreateVariablePicker<U>() => new VariablePicker<U>();
        public virtual And CreateAnd() => new And();
        public virtual OR CreateOR() => new OR();
        public virtual No CreateNo() => new No();
        public virtual CompareNumber CreateCompareNumber() => new CompareNumber();
        public virtual Equate<U> CreateEquate<U>() => new Equate<U>();
        public virtual ArrayEmpty<U> CreateArrayEmpty<U>() => new ArrayEmpty<U>();
        public virtual ArrayPicker<U> CreateArrayPicker<U>() => new ArrayPicker<U>();

        private static void ExeActList(List<APIActRuntime<T>> list, T handle) {
            handle.BeginVariableArea();
            for (int i = 0; i < list.Count; i++)
                list[i].Exe(handle);
            handle.EndVariableArea();
        }

        public class Domain : APIActRuntime<T> {
            public List<APIActRuntime<T>> list;
            public override void Exe(T handle) {
                ExeActList(list, handle);
            }
        }
        public class Fork : APIActRuntime<T> {
            public List<IF> ifList;
            public List<APIActRuntime<T>> acts;

            public override void Exe(T handle) {
                for (int i = 0; i < ifList.Count; i++) {
                    if (ifList[i].condition.Exe(handle)) {
                        ExeActList(ifList[i].acts, handle);
                        return;
                    }
                }
                ExeActList(acts, handle);
            }
            public class IF {
                public APIReturnRuntime<T, bool> condition;
                public List<APIActRuntime<T>> acts;
            }
        }
        public class ArrayAdd<U> : APIActRuntime<T> {
            public int id;
            public APIReturnRuntime<T, U> value;

            public override void Exe(T handle) {
                handle.GetArray<U>(id).Add(value.Exe(handle));
            }
        }
        public class ArrayForeach<U> : APIActRuntime<T> {
            public int id;
            public List<APIActRuntime<T>> acts;
            public override void Exe(T handle) {
                var list = handle.GetArray<U>(id);
                for (int i = 0; i < list.Count; i++) {
                    int vID = handle.CreateVariable(list[i]);
                    ExeActList(acts, handle);
                    handle.RemoveVarialbe(vID);
                }
            }
        }
        public class ArrayRemove<U> : APIActRuntime<T> {
            public int id;
            public APIReturnRuntime<T, U> value;

            public override void Exe(T handle) {
                handle.GetArray<U>(id).Remove(value.Exe(handle));
            }
        }

        public class GetArrayLength<U> : APIReturnRuntime<T, float> {
            public int id;
            public override float Exe(T handle) {
                return handle.GetArray<U>(id).Count;
            }
        }
        public class CreateArray<U> : APIActRuntime<T> {
            public APIReturnArrayRuntime<T, U> value;

            public override void Exe(T handle) {
                handle.CreateArray(value.Exe(handle));
            }
        }
        public class CreateVariable<U> : APIActRuntime<T> {
            public APIReturnRuntime<T, U> value;
            public override void Exe(T handle) {
                handle.CreateVariable(value.Exe(handle));
            }
        }
        public class SetVariable<U> : APIActRuntime<T> {
            public int id;
            public APIReturnRuntime<T, U> value;

            public override void Exe(T handle) {
                handle.SetVariable(id, value.Exe(handle));
            }
        }
        public class Static<U> : APIReturnRuntime<T, U> {
            public U value;
            public override U Exe(T handle) {
                return value;
            }
        }
        public class VariablePicker<U> : APIReturnRuntime<T, U> {
            public int id;

            public override U Exe(T handle) {
                return handle.GetVariable<U>(id);
            }
        }
        public class And : APIReturnRuntime<T, bool> {
            public APIReturnRuntime<T, bool> a;
            public APIReturnRuntime<T, bool> b;
            public override bool Exe(T handle) {
                return a.Exe(handle) && b.Exe(handle);
            }
        }
        public class OR : APIReturnRuntime<T, bool> {
            public APIReturnRuntime<T, bool> a;
            public APIReturnRuntime<T, bool> b;
            public override bool Exe(T handle) {
                return a.Exe(handle) || b.Exe(handle);
            }
        }
        public class No : APIReturnRuntime<T, bool> {
            public APIReturnRuntime<T, bool> a;
            public override bool Exe(T handle) {
                return !a.Exe(handle);
            }
        }
        public class CompareNumber : APIReturnRuntime<T, bool> {
            public NumberCompareType compareType;
            public APIReturnRuntime<T, float> a;
            public APIReturnRuntime<T, float> b;

            public override bool Exe(T handle) {
                float na = a.Exe(handle);
                float nb = b.Exe(handle);
                switch (compareType) {
                    case NumberCompareType.Equal:
                        return na == nb;
                    case NumberCompareType.NoEqual:
                        return na != nb;
                    case NumberCompareType.Greater:
                        return na > nb;
                    case NumberCompareType.GreaterOrEqual:
                        return na >= nb;
                    case NumberCompareType.Less:
                        return na < nb;
                    case NumberCompareType.LessOrEqual:
                    default:
                        return na <= nb;
                }
            }
        }
        public class Equate<U> : APIReturnRuntime<T, bool> {
            public APIReturnRuntime<T, U> a;
            public APIReturnRuntime<T, U> b;

            public override bool Exe(T handle) {
                return a.Exe(handle).Equals(b.Exe(handle));
            }
        }
        public class ArrayEmpty<U> : APIReturnArrayRuntime<T, U> {
            public override List<U> Exe(T handle) {
                return new List<U>();
            }
        }
        public class ArrayPicker<U> : APIReturnArrayRuntime<T, U> {
            public int id;
            public override List<U> Exe(T handle) {
                return handle.GetArray<U>(id);
            }
        }
    }
}
