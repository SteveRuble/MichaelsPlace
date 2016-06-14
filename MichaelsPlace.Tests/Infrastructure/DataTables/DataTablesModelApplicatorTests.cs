using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTables.AspNet.Core;
using DataTables.AspNet.Mvc5;
using FluentAssertions;
using MichaelsPlace.Extensions;
using Moq;
using NUnit.Framework;

namespace MichaelsPlace.Tests.Infrastructure
{
    public class DataTablesItem
    {
        public int Integer { get; set; }
        public bool Boolean { get; set; }
        public string StringA { get; set; }
        public string StringB { get; set; }

        public DataTablesItem(bool boolean, int integer, string stringA, string stringB)
        {
            Boolean = boolean;
            Integer = integer;
            StringA = stringA;
            StringB = stringB;
        }

        public override string ToString()
        {
            return $"{{ Integer: {Integer}, Boolean: {Boolean}, StringA: {StringA}, StringB: {StringB} }}";
        }
    }

    [TestFixture]
    public class DataTablesModelApplicatorTests
    {
        public readonly List<DataTablesItem> Items = new List<DataTablesItem>()
                                                     {
                                                         new DataTablesItem(true, 1, "abc", "bcd"),
                                                         new DataTablesItem(false, 2, "_abc", "bcd"),
                                                         new DataTablesItem(true, 3, "abc", "_bcd"),
                                                         new DataTablesItem(false, 3, "_abc", "True")
                                                     };


        [Test]
        public void sort()
        {
            var columns = new List<IColumn>()
                      {
                          new Column(nameof(DataTablesItem.Integer), nameof(DataTablesItem.Integer), true, true, null),
                          new Column(nameof(DataTablesItem.Boolean), nameof(DataTablesItem.Boolean), true, true, null),
                          new Column(nameof(DataTablesItem.StringA), nameof(DataTablesItem.StringA), true, true, null),
                          new Column(nameof(DataTablesItem.StringB), nameof(DataTablesItem.StringB), true, true, null),
                      };
            columns[0].SetSort(0, "asc");
            columns[1].SetSort(1, "asc");
            columns[2].SetSort(2, "desc");
            columns[3].SetSort(3, "desc");
            var expected = Items.OrderBy(i => i.Integer).ThenBy(i => i.Boolean).ThenByDescending(i => i.StringA).ThenByDescending(i => i.StringB)
                .ToList();
            var request = new TestDataTablesRequest(1, 0, 100, new Search(), columns);
            var actualObject = request.ApplyTo(Items.AsQueryable());
            var actual = ((List<DataTablesItem>) actualObject.Data);
            actual.Should().Equal(expected);
        }

        [Test]
        public void limit()
        {
            var expected = Items.Skip(2).Take(1).ToList();
            var request = new TestDataTablesRequest(1, 2, 1, new Search(), Enumerable.Empty<Column>());
            var actualObject = request.ApplyTo(Items.AsQueryable());
            var actual = ((List<DataTablesItem>) actualObject.Data);
            actual.Should().Equal(expected);
        }

        [Test]
        public void search()
        {
            var expected = Items.Where(i => i.Boolean == true || i.StringA.Contains("True") || i.StringB.Contains("True")).ToList();

            var columns = new List<IColumn>()
                      {
                          new Column(nameof(DataTablesItem.Integer), nameof(DataTablesItem.Integer), true, true, null),
                          new Column(nameof(DataTablesItem.Boolean), nameof(DataTablesItem.Boolean), true, true, null),
                          new Column(nameof(DataTablesItem.StringA), nameof(DataTablesItem.StringA), true, true, null),
                          new Column(nameof(DataTablesItem.StringB), nameof(DataTablesItem.StringB), true, true, null),
                      };

            var request = new TestDataTablesRequest(1, 0, 100, new Search("True", false), columns);
            var actualObject = request.ApplyTo(Items.AsQueryable());
            var actual = ((List<DataTablesItem>) actualObject.Data);
            actual.Should().Equal(expected);
        }

        [Test]
        public void filter()
        {
            var expected = Items.Where(i => i.Boolean == true && i.StringA.Contains("a")).ToList();

            var columns = new List<IColumn>()
                          {
                              new Column(nameof(DataTablesItem.Integer), nameof(DataTablesItem.Integer), true, true, null),
                              new Column(nameof(DataTablesItem.Boolean), nameof(DataTablesItem.Boolean), true, true, new Search("True", false)),
                              new Column(nameof(DataTablesItem.StringA), nameof(DataTablesItem.StringA), true, true, new Search("a", false)),
                              new Column(nameof(DataTablesItem.StringB), nameof(DataTablesItem.StringB), true, true, null),
                          };

            var request = new TestDataTablesRequest(1, 0, 100, null, columns);
            var actualObject = request.ApplyTo(Items.AsQueryable());
            var actual = ((List<DataTablesItem>) actualObject.Data);
            actual.Should().Equal(expected);
        }


    }

    internal class TestDataTablesRequest : IDataTablesRequest
    {
        public IDictionary<string, object> AdditionalParameters { get; private set; }

        public IEnumerable<IColumn> Columns { get; private set; }

        public int Draw { get; private set; }

        public int Length { get; private set; }

        public ISearch Search { get; private set; }

        public int Start { get; private set; }

        public TestDataTablesRequest(int draw, int start, int length, ISearch search, IEnumerable<IColumn> columns)
            : this(draw, start, length, search, columns, (IDictionary<string, object>) null)
        {
        }

        public TestDataTablesRequest(int draw, int start, int length, ISearch search, IEnumerable<IColumn> columns, IDictionary<string, object> additionalParameters)
        {
            this.Draw = draw;
            this.Start = start;
            this.Length = length;
            this.Search = search;
            this.Columns = columns;
            this.AdditionalParameters = additionalParameters;
        }
    }
}
    