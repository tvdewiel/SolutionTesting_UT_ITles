namespace BusinessLayer
{
    public class BikeInfo
    {
        public BikeInfo(int? id, string description, BikeType bikeType, int customerId, string customerDescription, double purchaseCost)
        {
            Id = id;
            Description = description;
            BikeType = bikeType;
            Customer = (customerId, customerDescription);
            PurchaseCost = purchaseCost;
        }
        public int? Id { get; set; }
        public string Description { get; set; }
        public BikeType BikeType { get; set; }
        public double PurchaseCost { get; set; }
        public (int id, string description) Customer { get; set; }//description : name (email)

        public override bool Equals(object? obj)
        {
            return obj is BikeInfo info &&
                   Id == info.Id &&
                   Description == info.Description &&
                   BikeType == info.BikeType &&
                   PurchaseCost == info.PurchaseCost &&
                   Customer.Equals(info.Customer);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Description, BikeType, PurchaseCost, Customer);
        }
    }
}
