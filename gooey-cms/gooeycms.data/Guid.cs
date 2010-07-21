using System;

namespace Gooeycms.Data
{
    public struct Guid
    {
        public Guid(String value) : this()
        {
            this.Value = value;
        }

        public String Value { get; set; }

        public Boolean IsEmpty()
        {
            return (String.IsNullOrEmpty(this.Value));
        }

        public static Guid Create()
        {
            String guid = System.Guid.NewGuid().ToString();
            Guid temp = new Guid();
            temp.Value = guid;

            return temp;
        }

        public static Guid New(String guid)
        {
            return new Guid(guid);
        }

        public override String ToString()
        {
            return this.Value;
        }
    }
}
