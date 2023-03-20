using BusinessLayer;
using System.ComponentModel.DataAnnotations;

namespace TestProjectUTDomain
{
    public class UnitTestCustomer
    {
        [Fact]
        public void Test_ctor_Valid()
        {
            Customer c = new Customer("jos","jos@gmail","9000 Gent");
            Assert.NotNull(c);
            Assert.Equal("jos", c.Name);
            Assert.Equal("jos@gmail", c.Email);
            Assert.Equal("9000 Gent", c.Address);
        }
        [Theory]
        [InlineData(" ","jos@gmail","gent")]
        [InlineData("", "jos@gmail", "gent")]
        [InlineData(null, "jos@gmail", "gent")]
        [InlineData("jos", "", "gent")]
        [InlineData("jos", "  ", "gent")]
        [InlineData("jos", null, "gent")]
        [InlineData("jos", "jos@gmail", "")]
        [InlineData("jos", "jos@gmail", "   ")]
        [InlineData("jos", "jos@gmail", null)]
        public void Test_ctor_InValid(string name, string email, string address)
        {
            Assert.Throws<DomainException>(()=>new Customer(name,email,address));
        }
        [Fact]
        public void Test_AddBike_Valid()
        {
            Bike bike1 = DomainFactory.NewBike(new BikeInfo(null,"blue bike",BikeType.regularBike,10,"jos (jos@gmail)",250));
            Bike bike2 = DomainFactory.NewBike(new BikeInfo(null, "green bike", BikeType.regularBike, 10, "jos (jos@gmail)", 275));
            Customer c = new Customer("jos", "jos@gmail", "9000 Gent");
            Assert.Empty(c.Bikes());
            c.AddBike(bike1);
            Assert.Contains(bike1,c.Bikes());
            Assert.Single(c.Bikes());
            Assert.Equal(c, bike1.Customer);
            c.AddBike(bike2);
            Assert.Contains(bike1,c.Bikes());
            Assert.Equal(c, bike1.Customer);
            Assert.Contains(bike2,c.Bikes());
            Assert.Equal(c, bike2.Customer);
            Assert.True(c.Bikes().Count==2);
        }
        [Fact]
        public void Test_AddBike_InValid()
        {
            Bike bike1 = DomainFactory.NewBike(new BikeInfo(null, "blue bike", BikeType.regularBike, 10, "jos (jos@gmail)", 250));
            Bike bike2 = DomainFactory.NewBike(new BikeInfo(null, "blue bike", BikeType.regularBike, 10, "jos (jos@gmail)", 250));
            Customer c = new Customer("jos", "jos@gmail", "9000 Gent");
            
            Assert.Empty(c.Bikes());
            Assert.Throws<DomainException>(()=>c.AddBike(null));
            c.AddBike(bike1);
            Assert.Throws<DomainException>(() => c.AddBike(bike1));
            Assert.Single(c.Bikes());
            Assert.Throws<DomainException>(() => c.AddBike(bike2));
            Assert.Single(c.Bikes());           
        }
    }
}