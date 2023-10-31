namespace ExcelExport {
    public class EnumOption {
        private string m_Address;
        public string Name { get; private set; }
        private EnumType m_Parent;
        public int Value { get; set; }

        public string Location => m_Parent.Location + ">" + m_Address;

        public EnumOption(EnumType enumType, string address, string name) {
            m_Parent = enumType;
            m_Address = address;
            Name = name;
        }
    }
}