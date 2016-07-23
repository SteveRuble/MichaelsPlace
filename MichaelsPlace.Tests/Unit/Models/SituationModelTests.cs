using System;
using System.ComponentModel;
using FluentAssertions;
using MichaelsPlace.Models;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Unit.Models
{
    [TestFixture]
    public class SituationModelTests
    {
        [Test]
        public void can_round_trip()
        {
            var expected = new SituationModel()
                         {
                             Contexts = {1, 2, 3},
                             Losses = {4, 5, 6},
                             Relationships = {7, 8, 9}
                         };
            var serialized = expected.ToString();
            Console.WriteLine(serialized);
            var actual = new SituationModel(serialized);

            actual.ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void can_type_convert_from_string()
        {
            var expected = new SituationModel()
                         {
                             Contexts = {1, 2, 3},
                             Losses = {4, 5, 6},
                             Relationships = {7, 8, 9}
                         };
            var serialized = expected.ToString();
            var converter = TypeDescriptor.GetConverter(typeof(SituationModel));
            var actual = converter.ConvertFrom(serialized);
            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
