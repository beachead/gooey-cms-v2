using System;

namespace Gooeycms.Data
{
    public struct Hash
    {
        public Hash(String hash)
            : this()
        {
            this.Value = hash;
        }

        public String Value { get; set; }

        public static Hash New(String hash)
        {
            return new Hash(hash);
        }

        public override String ToString()
        {
            return this.Value;
        }
    }
}
