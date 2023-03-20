
namespace BusinessLayer
{
    public class CustomerManager
    {
        private ICustomerRepository repo;

        public CustomerManager(ICustomerRepository repo)
        {
            this.repo = repo;
        }

        public void AddCustomer(CustomerInfo customerInfo)
        {
            try
            {
                if (customerInfo == null) throw new ManagerException("CustomerManager");
                Customer customer = DomainFactory.NewCustomer(customerInfo);
                repo.AddCustomer(customer);
                customerInfo.Id = customer.Id;
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
        public void UpdateCustomer(CustomerInfo customerInfo)
        {
            try
            {
                if (customerInfo == null) throw new ManagerException("CustomerManager");
                if (!customerInfo.Id.HasValue) throw new ManagerException("CustomerManager");
                Customer customer = repo.GetCustomer((int)customerInfo.Id);
                customer.SetAddress(customerInfo.Address);
                customer.SetName(customerInfo.Name);
                customer.SetEmail(customerInfo.Email);
                repo.UpdateCustomer(customer);
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
        public void DeleteCustomer(CustomerInfo customerInfo)
        {
            try
            {
                if (customerInfo == null) throw new ManagerException("CustomerManager");
                if (!customerInfo.Id.HasValue) throw new ManagerException("CustomerManager");
                Customer customer = repo.GetCustomer((int)customerInfo.Id);
                repo.DeleteCustomer(customer);
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
        public Customer GetCustomer(int id)
        {
            try
            {
                return repo.GetCustomer(id);
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
        public List<CustomerInfo> GetCustomersInfo()
        {
            try
            {
                return repo.GetCustomersInfo();
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
        public List<CustomerInfo> GetCustomersInfo(string name)
        {
            try
            {
                return repo.GetCustomersInfo(name);
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
        public Customer GetCustomer(string email)
        {
            try
            {
                return repo.GetCustomer(email);
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
        public void AddBike(BikeInfo bikeInfo)
        {
            try
            {
                if (bikeInfo == null) throw new ManagerException("CustomerManager");
                Customer customer = repo.GetCustomer(bikeInfo.Customer.id);
                Bike bike = DomainFactory.NewBike(bikeInfo);
                customer.AddBike(bike);
                repo.AddBike(bike);
                bikeInfo.Id = bike.Id;
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
        public void UpdateBike(BikeInfo bikeInfo)
        {
            try
            {
                if (bikeInfo == null) throw new ManagerException("CustomerManager");
                Customer customer = repo.GetCustomer(bikeInfo.Customer.id);
                Bike bike = customer.Bikes().Where(x => x.Id == bikeInfo.Id).First();
                bike.Description = bikeInfo.Description;
                bike.BikeType = bikeInfo.BikeType;
                bike.SetPurchaseCost(bikeInfo.PurchaseCost);
                //TODO check if bike changed
                repo.UpdateBike(bike);
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
        public void DeleteBike(BikeInfo bikeInfo)
        {
            try
            {
                if (bikeInfo == null) throw new ManagerException("CustomerManager");
                Customer customer = repo.GetCustomer(bikeInfo.Customer.id);
                Bike bike = customer.Bikes().Where(x => x.Id == bikeInfo.Id).First();
                //Bike bike = DomainFactory.ExistingBike(bikeInfo,customer);
                customer.RemoveBike(bike);
                repo.DeleteBike(bike);
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
        public List<BikeInfo> GetBikesInfo()
        {
            try
            {
                return repo.GetBikesInfo();
            }
            catch (Exception ex) { throw new ManagerException("CustomerManager", ex); }
        }
    }
}
