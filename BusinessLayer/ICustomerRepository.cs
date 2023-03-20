namespace BusinessLayer
{
    public interface ICustomerRepository
    {
        void AddBike(Bike bike);
        void AddCustomer(Customer customer);
        void DeleteBike(Bike bike);
        void DeleteCustomer(Customer customer);
        List<BikeInfo> GetBikesInfo();
        Customer GetCustomer(int id);
        Customer GetCustomer(string email);
        List<CustomerInfo> GetCustomersInfo(string? name=null);
        void UpdateBike(Bike bike);
        void UpdateCustomer(Customer customer);
    }
}
