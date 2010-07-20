using System;

namespace Gooeycms.Data
{
    public struct EncryptedValue
    {
        public EncryptedValue(String value) : this()
        {
            this.Value = value;
        }

        public String Value { get; set; }
        public static EncryptedValue New(String value)
        {
            return new EncryptedValue(value);
        }
    }
}
