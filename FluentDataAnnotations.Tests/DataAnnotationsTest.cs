/*  Copyright (C) 2009 Jos van der Til
    
    This file is part of the Fluent DataAnnotations Library.

    The Fluent DataAnnotations Library is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    The Fluent Metadata Library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with the Fluent DataAnnotations Library.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FluentDataAnnotations.Tests
{
    [TestClass]
    public class DataAnnotationsTest
    {
        [TestMethod]
        public void AddRawAttribute()
        {
            var annotations = new DataAnnotations<TestEntity>();

            annotations.AnnotateProperty(x => x.Name).AddRawAttribute(new NonSerializedAttribute());

            Assert.AreEqual(1, annotations.Annotations.Count);
        }

        [TestMethod]
        public void TestGetPropertyDescriptor1M()
        {
            var provider = new DataAnnotationsTypeDescriptionProvider(TypeDescriptor.GetProvider(typeof (TestEntity)));

            var start = DateTime.Now;
            for(int i = 0; i < 1000000; i++)
            {
                provider.GetTypeDescriptor(typeof (TestEntity), null);
            }
            var end = DateTime.Now;

            Console.WriteLine(end - start);
        }
    }

    public class TestEntity
    {
        public Guid Id { get; set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
