using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Serialization;

namespace ChatSocket
{
    [Serializable]
    public class Contatto : IEquatable<Contatto>
    {
        public String Nome { get; set; }
        public String IP { get; set; }
        public String Port { get; set; }

        public bool Equals(Contatto other)
        {
            return IP.Equals(other.IP);
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
