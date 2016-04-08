﻿using System;
using System.Diagnostics;
using NPoco;
using NPoco.Tests.Common;
using NUnit.Framework;

namespace NPoco.Tests
{
    public class NewMapperTests : BaseDBDecoratedTest
    {
        [Test]
        public void Test1()
        {
            var result = Database.Fetch<TestNewMapper>(@"
select 4 Id, 'Will' Name, 23.0 Money__Value, 'AUD' Money__Code 
union all
select 8 Id, 'John' Name, 87.0 Money__Value, 'USD' Money__Code");

          

            var result2 = Database.FetchMultiple<TestNewMapper, Money>(@"
select 4 Id, 'Will' Name, 23.0 Value, 'AUD' Code 
union all
select 8 Id, 'John' Name, 87.0 Value, 'USD' Code");

            var sw1 = Stopwatch.StartNew();

            for (int i = 0; i < 500; i++)
            {
                var result1 = Database.FetchMultiple<TestNewMapper, Money>(@"
select 4 Id, 'Will' Name, 23.0 Value, 'AUD' Code 
union all
select 8 Id, 'John' Name, 87.0 Value, 'USD' Code");
            }

            sw1.Stop();

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 500; i++)
            {
                var result1 = Database.Fetch<TestNewMapper>(@"
select 4 Id, 'Will' Name, 23.0 Money__Value, 'AUD' Money__Code 
union all
select 8 Id, 'John' Name, 87.0 Money__Value, 'USD' Money__Code");
            }

            sw.Stop();

            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(sw1.ElapsedMilliseconds);
        }

        [Test]
        public void GetOnlyProperties()
        {
            var result = Database.Single<GetzOnly>("select 'aaa' Name1, 'bbb' Name2");
            Assert.AreEqual("aaa", result.Name1);
            Assert.AreEqual("bbb", result.Name2);
        }
    }

    public class GetzOnly
    {
        public string Name1 { get; }
        public string Name2 { get; } = "Default";
    }

    public class TestNewMapper
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Money Money { get; set; }
    }

    public class Money
    {
        public decimal Value { get; set; }
        public string Code { get; set; }
    }
}
