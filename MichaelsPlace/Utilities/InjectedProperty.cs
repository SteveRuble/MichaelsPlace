using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MichaelsPlace.Utilities
{
    /// <summary>
    /// Used as a backing field for properties which should be injected.
    /// Throws informative exception if the value has not been set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Injected<T> where T : class
    {
        private T _value;

        public T Value
        {
            get
            {
                if (_value == null) throw new InvalidOperationException($"Value should have been set to an instance of {typeof(T)} via injection (or manually during a unit test).");
                return _value;
            }
            set { _value = value; }
        }

    }
}
