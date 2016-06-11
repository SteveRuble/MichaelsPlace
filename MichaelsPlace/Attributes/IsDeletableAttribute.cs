using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MichaelsPlace.Attributes
{
    /// <summary>
    /// Attribute which will tell the UI whether or not to display the delete button.
    /// TODO: implement UI side.
    /// </summary>
    public class IsDeletableAttribute : Attribute
    {
        public bool IsDeletable { get; }

        public IsDeletableAttribute(bool isDeletable)
        {
            IsDeletable = isDeletable;
        }
    }
}
