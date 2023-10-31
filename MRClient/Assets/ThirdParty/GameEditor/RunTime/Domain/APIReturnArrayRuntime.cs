using System.Collections.Generic;
namespace GameEditor {
    public abstract class APIReturnArrayRuntime<T, U> {
        public abstract List<U> Exe(T handle);
    }
}
