using BusinessLayer;
using DataLayer;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProjectUTDomain
{
    public class UnitTestCustomerManager
    {
        private CustomerManager customerManager;
        private Mock<ICustomerRepository> customerRepository;

        public UnitTestCustomerManager()
        {
            customerRepository = new Mock<ICustomerRepository>();
            customerManager = new CustomerManager(customerRepository.Object);
        }

        [Fact]
        public void Test_GetBikesInfo_InValid()
        {
            customerRepository.Setup(r => r.GetBikesInfo()).Throws(new CustomerRepositoryException("GetBikesInfo"));
            var ex=Assert.Throws<ManagerException>(()=>customerManager.GetBikesInfo());
            Assert.IsType<CustomerRepositoryException>(ex.InnerException);
        }
        [Fact]
        public void Test_UpdateBike_Valid()
        {
            BikeInfo biOld = new BikeInfo(5, "blue bike", BikeType.childBike, 10, "jos(jos@gmail)", 120);
            BikeInfo biNew = new BikeInfo(5, "green bike", BikeType.racingBike, 10, "jos(jos@gmail)", 180);
            Bike bikeOld=DomainFactory.NewBike(biOld);
            Bike bikeNew = DomainFactory.NewBike(biNew);
            Customer c = DomainFactory.ExistingCustomer(10, "jos", "jos@gmail", "Gent", new List<Bike>() { bikeOld });
            customerRepository.Setup(r => r.GetCustomer(biOld.Customer.id))
                .Returns(c);
            customerManager.UpdateBike(biNew);
            Assert.Equal(bikeNew, c.Bikes().Where(b => b.Id == 5).First());
        }
        [Fact]
        public void Test_UpdateBike_InValidBikeInfo()
        {
            //bikeinfo is null
            Assert.Throws<ManagerException>(() => customerManager.UpdateBike(null));      
        }
        [Fact]
        public void Test_UpdateBike_InValidCustomer()
        {
            //get customer fails
            BikeInfo bi = new BikeInfo(5, "blue bike", BikeType.childBike, 10, "jos(jos@gmail)", 120);
            customerRepository.Setup(r => r.GetCustomer(10)).Throws(new CustomerRepositoryException("GetCustomer"));
            var ex = Assert.Throws<ManagerException>(() => customerManager.UpdateBike(bi));
            Assert.IsType<CustomerRepositoryException>(ex.InnerException);
        }
        [Fact]
        public void Test_UpdateBike_InValidUpdate()
        {
            BikeInfo biOld = new BikeInfo(5, "blue bike", BikeType.childBike, 10, "jos(jos@gmail)", 120);
            BikeInfo biNew = new BikeInfo(5, "green bike", BikeType.racingBike, 10, "jos(jos@gmail)", 180);
            Bike bikeOld = DomainFactory.NewBike(biOld);
            Bike bikeNew = DomainFactory.NewBike(biNew);
            Customer c = DomainFactory.ExistingCustomer(10, "jos", "jos@gmail", "Gent", new List<Bike>() { bikeOld });
            customerRepository.Setup(r => r.GetCustomer(biOld.Customer.id))
                .Returns(c);
            customerRepository.Setup(r=>r.UpdateBike(bikeNew)).Throws(new CustomerRepositoryException("UpdateBike"));
            var ex = Assert.Throws<ManagerException>(() => customerManager.UpdateBike(biNew));
            Assert.IsType<CustomerRepositoryException>(ex.InnerException);
        }
        [Fact]
        public void Test_UpdateBike_InValidPurchaseCost()
        {
            BikeInfo biOld = new BikeInfo(5, "blue bike", BikeType.childBike, 10, "jos(jos@gmail)", 120);
            BikeInfo biNew = new BikeInfo(5, "green bike", BikeType.racingBike, 10, "jos(jos@gmail)", -180);
            Bike bikeOld = DomainFactory.NewBike(biOld);
            Customer c = DomainFactory.ExistingCustomer(10, "jos", "jos@gmail", "Gent", new List<Bike>() { bikeOld });
            customerRepository.Setup(r => r.GetCustomer(biOld.Customer.id))
                .Returns(c);
            var ex = Assert.Throws<ManagerException>(() => customerManager.UpdateBike(biNew));
            Assert.IsType<DomainException>(ex.InnerException);
        }
    }
}
