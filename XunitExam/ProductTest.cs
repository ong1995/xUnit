using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Xunit;
using AutoFixture;

namespace XunitExam
{
    public class ProductTest
    {
        [Fact]
        public void Products_ShouldReturnNullExceptionIfTheValueIsNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Products().AddNew(null)
                );
        }

        [Fact]
        public void Products_ShouldReturnTrueIfTheNewProductWasAdded()
        {
            var fixture = new Fixture();
            
            var product = fixture.Build<Product>().With(x=>x.IsSold, false).Create();
            var products = new Products();
            products.AddNew(product);
            Assert.Contains<Product>(product, products.Items);
        }

        [Fact]
        public void Products_ShouldReturnExceptionIfTheProductNameIsNull()
        {
            var fixture = new Fixture();

            var product = fixture.Build<Product>()
                .With(x => x.IsSold, false)
                //.With(x => x.Name, null)
                .Create();

            product.Name = null;
            Assert.Throws<NameRequiredException>(
                () => new Products().AddNew(product)
                );
        }
    }

    internal class Products
    {
        private readonly List<Product> _products = new List<Product>();

        public IEnumerable<Product> Items => _products.Where(t => !t.IsSold);

        public void AddNew(Product product)
        {
            product = product ??
                throw new ArgumentNullException();
            product.Validate();
            _products.Add(product);
        }

        public void Sold(Product product)
        {
            product.IsSold = true;
        }

    }

    internal class Product
    {
        public bool IsSold { get; set; }
        public string Name { get; set; }

        internal void Validate()
        {
            Name = Name ??
                throw new NameRequiredException();
        }

    }

    [Serializable]
    internal class NameRequiredException : Exception
    {
        public NameRequiredException() { /* ... */ }

        public NameRequiredException(string message) : base(message) { /* ... */ }

        public NameRequiredException(string message, Exception innerException) : base(message, innerException) { /* ... */ }

        protected NameRequiredException(SerializationInfo info, StreamingContext context) : base(info, context) { /* ... */ }
    }
}
