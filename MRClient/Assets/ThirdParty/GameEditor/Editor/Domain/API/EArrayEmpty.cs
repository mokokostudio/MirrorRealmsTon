namespace GameEditor.Editors.Domain {
    [APIName("空")]
    public class EArrayEmpty<T> : EAPIReturnArray<T> {
        public override string ToString() => GetName("空");
    }
}
