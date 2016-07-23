using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelsPlace.Models.Persistence;

namespace MichaelsPlace.Models
{
    /// <summary>
    /// Represents a situation in an easily serialized/deserialized form.
    /// Serializes to a dash delimited list of dot-delimited lists of tag IDs.
    /// The order is <see cref="Contexts"/>-<see cref="Losses"/>-<see cref="Relationships"/>
    /// </summary>
    [TypeConverter(typeof(SituationTypeConverter))]
    public class SituationModel
    {
        public List<int> Contexts { get; set; } = new List<int>();
        public List<int> Losses { get; set; } = new List<int>();
        public List<int> Relationships { get; set; } = new List<int>();

        public override string ToString() =>
            string.Join("-", string.Join(".", Contexts), string.Join(".", Losses), string.Join(".", Relationships));

        public SituationModel() { }

        public SituationModel(string fromString)
        {
            var segs = fromString.Split('-');
            if(segs.Length != 3) { throw new ArgumentException("Invalid Situation: " + fromString, "fromString");}
            Contexts = segs[0].Split('.').Select(int.Parse).ToList();
            Losses = segs[1].Split('.').Select(int.Parse).ToList();
            Relationships = segs[2].Split('.').Select(int.Parse).ToList();

        }

    }

    public class SituationTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;
            return stringValue != null ? new SituationModel(stringValue) : base.ConvertFrom(context, culture, value);
        }
    }
}
