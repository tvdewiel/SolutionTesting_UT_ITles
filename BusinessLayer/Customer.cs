namespace BusinessLayer
{
    public class Customer
    {
        public int? Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }
        private List<Bike> bikes = new List<Bike>();

        internal Customer(string name, string email, string address)
        {
            SetName(name);
            SetEmail(email);
            SetAddress(address);
        }
        internal Customer(int id, string name, string email, string address, List<Bike> bikes) : this(name, email, address)
        {
            SetId(id);
            this.bikes = bikes;
            foreach (Bike bike in bikes) { bike.SetCustomer(this); }
        }

        public IReadOnlyList<Bike> Bikes() { return bikes.AsReadOnly(); }
        public void AddBike(Bike bike)
        {
            if (bike == null) throw new DomainException("Customer");
            //if (bike.Customer!=null) if (bike.Customer!=this) throw new DomainException("Customer"); //je kan een fiets niet toekennen aan een nieuwe klant
            if (bikes.Contains(bike)) throw new DomainException("Customer");
            bikes.Add(bike);
            if (bike.Customer != this) bike.SetCustomer(this); 
        }
        public void RemoveBike(Bike bike)
        {
            if (bike == null) throw new DomainException("Customer");
            //if (bike.Customer != this) throw new DomainException("Customer");
            if (!bikes.Contains(bike)) throw new DomainException("Customer");
            bikes.Remove(bike);
        }
        public void SetId(int id)
        {
            if (id <= 0) throw new DomainException("Customer");
            Id = id;
        }
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Customer");
            Name = name;
        }
        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Customer");
            Email = email;
        }
        public void SetAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new DomainException("Customer");
            Address = address;
        }
    }
}