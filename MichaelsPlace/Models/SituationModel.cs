using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MichaelsPlace.Models
{
    /// <summary>
    /// Represents a situation in an easily serialized/deserialized form.
    /// Serializes to a semi-colon delimited list of comma-delimited lists of tag IDs.
    /// The order is <see cref="Demographics"/>-<see cref="Losses"/>-<see cref="Mourners"/>
    /// </summary>
    [TypeConverter(typeof(SituationTypeConverter))]
    public class SituationModel
    {
        public List<int> Demographics { get; set; } = new List<int>();
        public List<int> Losses { get; set; } = new List<int>();
        public List<int> Mourners { get; set; } = new List<int>();

        public override string ToString() =>
            string.Join("-", string.Join(".", Demographics), string.Join(".", Losses), string.Join(".", Mourners));

        public SituationModel() { }

        public SituationModel(string fromString)
        {
            var segs = fromString.Split('-');
            if(segs.Length != 3) { throw new ArgumentException("Invalid Situation: " + fromString, "fromString");}
            Demographics = segs[0].Split('.').Select(int.Parse).ToList();
            Losses = segs[1].Split('.').Select(int.Parse).ToList();
            Mourners = segs[2].Split('.').Select(int.Parse).ToList();

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
