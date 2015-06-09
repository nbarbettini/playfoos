using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core
{
    // Borrowed from Şafak Gür (http://stackoverflow.com/a/18613926/3191599)
    // Helper extension that allows hashcodes to be calcualated fluenty
    public struct HashCode
    {
        private readonly int hashCode;

        public HashCode(int hashCode)
        {
            this.hashCode = hashCode;
        }

        public static HashCode Start
        {
            get { return new HashCode(17); }
        }

        public static implicit operator int(HashCode hashCode)
        {
            return hashCode.GetHashCode();
        }

        public HashCode Include(int existingHash)
        {
            unchecked { existingHash += this.hashCode * 31; }
            return new HashCode(existingHash);
        }

        public HashCode Hash<T>(T obj)
        {
            var h = obj != null ? obj.GetHashCode() : 0;
            unchecked { h += this.hashCode * 31; }
            return new HashCode(h);
        }

        public override int GetHashCode()
        {
            return this.hashCode;
        }
    }
}
