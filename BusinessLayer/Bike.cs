namespace BusinessLayer
{
    public class Bike
    {
        internal Bike(int? id, BikeType bikeType, double purchaseCost, string? description)
        {
            if (id.HasValue) SetId((int)id);
            BikeType = bikeType;
            SetPurchaseCost(purchaseCost);
            Description = description;
        }
        public int? Id { get; private set; }
        public BikeType BikeType { get; set; }
        public double PurchaseCost { get; private set; }
        public string? Description { get; set; }
        public Customer Customer { get; private set; }
        public void SetId(int id)
        {
            if (id <= 0)
                throw new DomainException("Bike");
            Id = id;
        }
        public void SetPurchaseCost(double purchaseCost)
        {
            if (purchaseCost < 0) throw new DomainException("Bike");
            PurchaseCost = purchaseCost;
        }
        public void SetCustomer(Customer customer)
        {
            if (customer == null) throw new DomainException("Bike");
            if (!customer.Bikes().Contains(this)) customer.AddBike(this);
            Customer = customer;
        }
        public override string ToString()
        {
            return $"{Id},{BikeType},{PurchaseCost},{Description}";
        }
        public override bool Equals(object? obj)
        {
            return obj is Bike bike &&
                   Description == bike.Description &&
                   PurchaseCost == bike.PurchaseCost &&
                   BikeType == bike.BikeType;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Description, PurchaseCost, BikeType);
        }
    }
}
