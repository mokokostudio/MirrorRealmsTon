namespace GameEditor.Editors.Domain {
    [APIName("数组")]
    public class EArrayPicker<T> : EAPIReturnArray<T> {
        public EArray<T> array;
        public override string ToString() => "数组" + GetName(array);
    }
}
