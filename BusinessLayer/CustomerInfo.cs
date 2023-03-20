namespace BusinessLayer
{ 
    public class CustomerInfo
    {
        public CustomerInfo(int? id, string name, string email, string address, int nrOfBikes, double totalBikeValues)
        {
            Id = id;
            Name = name;
            Email = email;
            Address = address;
            NrOfBikes = nrOfBikes;
            TotalBikeValues = totalBikeValues;
        }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int NrOfBikes { get; set; }
        public double TotalBikeValues { get; set; }
        public override string ToString()
        {
            return $"{Id},{Name},{Address},{Email}";
        }
    }
}
