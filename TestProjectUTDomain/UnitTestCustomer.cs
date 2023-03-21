using BusinessLayer;
using System.ComponentModel.DataAnnotations;

namespace TestProjectUTDomain
{
    public class UnitTestCustomer
    {
        private Customer customerValid;

        public UnitTestCustomer()
        {
            customerValid = new Customer("jos", "jos@gmail", "9000 Gent");
        }

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
        [InlineData("","jos@gmail","gent")]
        [InlineData(" ", "jos@gmail", "gent")]
        [InlineData(null, "jos@gmail", "gent")]
        [InlineData("jos", "", "gent")]
        [InlineData("jos", "  ", "gent")]
        [InlineData("jos", null, "gent")]
        [InlineData("jos", "jos@gmail", "")]
        [InlineData("jos", "jos@gmail", "   ")]
        [InlineData("jos", "jos@gmail", null)]
        public void Test_ctor_InValid(string name,string email,string address)
        {          
            Assert.Throws<DomainException>(()=>new Customer(name,email,address));
        }
        [Fact]
        public void Test_AddBike_Valid()
        {
            Bike bike1 = DomainFactory.NewBike(new BikeInfo(null, "blue bike", BikeType.regularBike, 10, "jos (jos@gmail)", 250));
            Bike bike2 = DomainFactory.NewBike(new BikeInfo(null, "green bike", BikeType.regularBike, 10, "jos (jos@gmail)", 275));
            //Customer c = new Customer("jos", "jos@gmail", "9000 Gent");

            Assert.Empty(customerValid.Bikes());
            customerValid.AddBike(bike1);
            Assert.Contains(bike1, customerValid.Bikes());
            Assert.Single(customerValid.Bikes());
            Assert.Equal(customerValid, bike1.Customer);

            customerValid.AddBike(bike2);
            Assert.Contains(bike1, customerValid.Bikes());
            Assert.Contains(bike2, customerValid.Bikes());
            Assert.True(customerValid.Bikes().Count == 2);
            Assert.Equal(customerValid, bike1.Customer);
            Assert.Equal(customerValid, bike2.Customer);
        }
        [Fact]
        public void Test_AddBike_null_InValid()
        {
            Assert.Throws<DomainException>(() => customerValid.AddBike(null));
        }
        [Fact]
        public void Test_AddBike_DuplicateBike_InValid()
        {
            Bike bike1 = DomainFactory.NewBike(new BikeInfo(null, "blue bike", BikeType.regularBike, 10, "jos (jos@gmail)", 250));
            Bike bike2 = DomainFactory.NewBike(new BikeInfo(null, "blue bike", BikeType.regularBike, 10, "jos (jos@gmail)", 250));
            Assert.Empty(customerValid.Bikes());
            customerValid.AddBike(bike1);
            Assert.Throws<DomainException>(() => customerValid.AddBike(bike2));
            Assert.Single(customerValid.Bikes());
            Assert.Contains(bike2, customerValid.Bikes());
        }
    }
}