using BusinessLayer;
using DataLayer;
using System.Data.SqlClient;
using Xunit.Priority;

namespace TestProjectIntegrationTest
{
    public class DatabaseFixture : IDisposable
    {
        private string connString = @"Data Source=NB21-6CDPYD3\SQLEXPRESS;Initial Catalog=BikeRepairShopTestDB;Integrated Security=True";

        private void CleanDB()
        {
            string clearDB = "DELETE FROM Bike;DBCC CHECKIDENT ('Bike', RESEED, 0); DELETE FROM Customer; DBCC CHECKIDENT ('Customer', RESEED, 0)";
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand command = conn.CreateCommand())
            {
                conn.Open();
                command.CommandText = clearDB;
                command.ExecuteNonQuery();
            }
        }
        public DatabaseFixture()
        {            
            CleanDB();
            customerRepository = new CustomerRepositoryADO(connString);
            // ... initialize data in the test database ...
        }
        public void Dispose()
        {
            // ... clean up test data from the database ...
        }
        public ICustomerRepository customerRepository { get; private set; }
    }
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class TestCustomerRepository : IClassFixture<DatabaseFixture>
    {
        DatabaseFixture fixture;
        CustomerManager customerManager;

        public TestCustomerRepository(DatabaseFixture fixture)
        {
            this.fixture = fixture;
            customerManager = new CustomerManager(fixture.customerRepository);
        }
        [Fact, Priority(2)]
        public void ZTest()
        {
            //Test();
            string description = "green bike";
            BikeType bikeType = BikeType.racingBike;
            int customerId = 1;
            string customerDescription = "jos (jos@gmail)";
            double purchaseCost = 175;
            BikeInfo bikeInfo = new BikeInfo(null, description, bikeType, customerId, customerDescription, purchaseCost);

            customerManager.AddBike(bikeInfo);
            var bDB = customerManager.GetBikesInfo();

            Assert.NotNull(bDB);
            Assert.Contains(bikeInfo, bDB);
        }
        [Fact, Priority(0)]
        public void Test()
        {
            string name = "jos";
            string email = "jos@gmail";
            string address = "9000 Gent";
            CustomerInfo customerInfo = new CustomerInfo(null,name,email,address,0,0);
            customerManager.AddCustomer(customerInfo);
            Customer cDB=customerManager.GetCustomer(1);
            Assert.NotNull(cDB);
            Assert.Equal(email, cDB.Email);
            Assert.Equal(name, cDB.Name);
            Assert.Equal(address, cDB.Address);
        }
        [Fact, Priority(1)]
        public void bTest()
        {
            string description = "blue bike";
            BikeType bikeType = BikeType.racingBike;
            int customerId = 1;
            string customerDescription = "jos (jos@gmail)";
            double purchaseCost = 175;
            BikeInfo bikeInfo = new BikeInfo(null, description, bikeType, customerId, customerDescription, purchaseCost);

            customerManager.AddBike(bikeInfo);
            var bDB = customerManager.GetBikesInfo();

            Assert.NotNull(bDB);
            Assert.Contains(bikeInfo, bDB);
        }
    }
}