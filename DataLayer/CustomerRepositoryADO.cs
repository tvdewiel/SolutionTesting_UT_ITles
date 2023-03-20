using BusinessLayer;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class CustomerRepositoryADO : ICustomerRepository
    {
        private string connectionString;

        public CustomerRepositoryADO(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void AddBike(Bike bike)
        {
            try
            {
                string sql = "INSERT INTO bike(biketype,purchasecost,description,customerid,status)  output INSERTED.ID VALUES(@biketype,@purchasecost,@description,@customerid,@status)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@biketype", bike.BikeType.ToString());
                    command.Parameters.AddWithValue("@purchasecost", bike.PurchaseCost);
                    if (bike.Description != null) command.Parameters.AddWithValue("@description", bike.Description);
                    else command.Parameters.AddWithValue("@description", DBNull.Value);
                    command.Parameters.AddWithValue("@customerid", bike.Customer.Id);
                    command.Parameters.AddWithValue("@status", 1);
                    int bid = (int)command.ExecuteScalar();
                    bike.SetId(bid);
                }
            }
            catch (Exception ex) { throw new CustomerRepositoryException("AddBike", ex); }
        }
        public void AddCustomer(Customer customer)
        {
            try
            {
                string sqlC = "INSERT INTO customer(name,email,address,status) output INSERTED.ID VALUES(@name,@email,@address,@status)";
                string sqlB = "INSERT INTO bike(biketype,purchasecost,description,customerid,status) output INSERTED.ID VALUES(@biketype,@purchasecost,@description,@customerid,@status)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        command.Transaction = transaction;
                        command.CommandText = sqlC;
                        command.Parameters.AddWithValue("@name", customer.Name);
                        command.Parameters.AddWithValue("@email", customer.Email);
                        command.Parameters.AddWithValue("@address", customer.Address);
                        command.Parameters.AddWithValue("@status", 1);
                        int id = (int)command.ExecuteScalar();
                        customer.SetId(id);
                        command.CommandText = sqlB;
                        foreach (Bike b in customer.Bikes())
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@biketype", b.BikeType.ToString());
                            command.Parameters.AddWithValue("@purchasecost", b.PurchaseCost);
                            if (b.Description != null) command.Parameters.AddWithValue("@description", b.Description);
                            else command.Parameters.AddWithValue("@description", DBNull.Value);
                            command.Parameters.AddWithValue("@customerid", id);
                            command.Parameters.AddWithValue("@status", 1);
                            int bid = (int)command.ExecuteScalar();
                            b.SetId(bid);
                        }
                        transaction.Commit();
                    }
                    catch (Exception) { transaction.Rollback(); throw; }
                }
            }
            catch (Exception ex) { throw new CustomerRepositoryException("AddCustomer", ex); }
        }       
        public void DeleteBike(Bike bike)
        {
            try
            {
                string sql = "UPDATE bike SET status=0 WHERE id=@id and status=1";
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@id", bike.Id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new CustomerRepositoryException("DeleteCustomer", ex); }
        }
        public void DeleteCustomer(Customer customer)
        {
            try
            {
                string sql = "UPDATE customer SET status=0 WHERE id=@id and status=1";
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@id", customer.Id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new CustomerRepositoryException("DeleteCustomer", ex); }
        }
        public List<BikeInfo> GetBikesInfo()
        {
            try
            {
                string sql = "SELECT t1.*,t2.name,t2.email FROM bike t1 INNER JOIN customer t2 on t1.customerid=t2.id WHERE t1.status=1 and t2.status=1";
                List<BikeInfo> bikes = new List<BikeInfo>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = sql;
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        bikes.Add(new BikeInfo((int)reader["id"], reader.IsDBNull(reader.GetOrdinal("description")) ? null : (string)reader["description"], (BikeType)Enum.Parse(typeof(BikeType), (string)reader["biketype"], true), (int)reader["customerid"], (string)reader["name"] + " (" + (string)reader["email"] + ")", (double)reader["purchasecost"]));
                    }
                    reader.Close();
                    return bikes;
                }
            }
            catch (Exception ex) { throw new CustomerRepositoryException("GetBikesInfo", ex); }
        }
        public Customer GetCustomer(int id)
        {
            try
            {
                string sqlC = "SELECT t1.*,t2.id bikeid,t2.biketype,t2.purchasecost,t2.description FROM customer t1 left join  (select * from bike where status=1) t2 on t1.id=t2.customerid WHERE t1.id=@id AND t1.status=1";
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = sqlC;
                    command.Parameters.AddWithValue("@id", id);
                    IDataReader reader = command.ExecuteReader();
                    string name = null, email = null, address = null;
                    bool firstLine = true;
                    List<Bike> bikes = new List<Bike>();
                    while (reader.Read())
                    {
                        if (firstLine)
                        {
                            firstLine = false;
                            name = (string)reader["name"];
                            email = (string)reader["email"];
                            address = (string)reader["address"];
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("bikeid")))
                        {
                            bikes.Add(DomainFactory.ExistingBike((int)reader["bikeid"], (BikeType)Enum.Parse(typeof(BikeType), (string)reader["biketype"], true), (double)reader["purchasecost"],
                               reader.IsDBNull(reader.GetOrdinal("description")) ? null : (string)reader["description"]));
                        }
                    }
                    reader.Close();                    
                    return DomainFactory.ExistingCustomer(id, name, email, address, bikes);
                }
            }
            catch (Exception ex) { throw new CustomerRepositoryException("GetCustomer", ex); }
        }
        public Customer GetCustomer(string email)
        {
            try
            {
                string sqlC = "SELECT t1.*,t2.id bikeid,t2.biketype,t2.purchasecost,t2.description FROM customer t1 left join  (select * from bike where status=1) t2 on t1.id=t2.customerid WHERE t1.email=@email AND t1.status=1 ";
                string sqlROI = "SELECT * from repairorder WHERE customerid=@customerid and status=1";
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = sqlC;
                    command.Parameters.AddWithValue("@email", email);
                    IDataReader reader = command.ExecuteReader();
                    string name = null, address = null;
                    int customerid = 0;
                    bool firstLine = true;
                    List<Bike> bikes = new List<Bike>();
                    while (reader.Read())
                    {
                        if (firstLine)
                        {
                            firstLine = false;
                            name = (string)reader["name"];
                            address = (string)reader["address"];
                            customerid = (int)reader["id"];
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("bikeid")))
                        {
                            bikes.Add(DomainFactory.ExistingBike((int)reader["bikeid"], (BikeType)Enum.Parse(typeof(BikeType), (string)reader["biketype"], true), (double)reader["purchasecost"],
                               reader.IsDBNull(reader.GetOrdinal("description")) ? null : (string)reader["description"]));
                        }
                    }
                    reader.Close();                    
                    return DomainFactory.ExistingCustomer(customerid, name, email, address, bikes);
                }
            }
            catch (Exception ex) { throw new CustomerRepositoryException("GetCustomer", ex); }
        }
        public List<CustomerInfo> GetCustomersInfo(string? name=null)
        {
            try
            {
                string sql;
                if (name== null)
                   sql = "SELECT t1.id,t1.name,t1.email,t1.address,count(t2.id) nrofbikes,coalesce(sum(t2.purchasecost),0) totalcost "
                    + " FROM customer t1 left join (select * from bike where status=1) t2 on t1.id=t2.customerId "
                    + " WHERE t1.status=1 "
                    + " group by t1.id,t1.name,t1.email,t1.address";
                else
                    sql = "SELECT t1.id,t1.name,t1.email,t1.address,count(t2.id) nrofbikes,coalesce(sum(t2.purchasecost),0) totalcost "
                    + " FROM customer t1 left join (select * from bike where status=1) t2 on t1.id=t2.customerId "
                    + " WHERE t1.status=1 AND t1.name LIKE @namematch "
                    + " group by t1.id,t1.name,t1.email,t1.address";
                List<CustomerInfo> customers = new List<CustomerInfo>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = sql;
                    if (name != null) command.Parameters.AddWithValue("@namematch", $"%{name}%");
                    IDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        customers.Add(new CustomerInfo((int)reader["id"], (string)reader["name"], (string)reader["email"], (string)reader["address"], (int)reader["nrofbikes"], (double)reader["totalcost"]));
                    }
                    reader.Close();
                    return customers;
                }
            }
            catch (Exception ex) { throw new CustomerRepositoryException("GetCustomersInfo", ex); }
        }
        public void UpdateBike(Bike bike)
        {
            try
            {
                string sql = "UPDATE bike SET biketype=@biketype,purchasecost=@purchasecost,customerid=@customerid,description=@description WHERE id=@id and status=1";
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@biketype", bike.BikeType.ToString());
                    command.Parameters.AddWithValue("@purchasecost", bike.PurchaseCost);
                    if (bike.Description != null) command.Parameters.AddWithValue("@description", bike.Description);
                    else command.Parameters.AddWithValue("@description", DBNull.Value);
                    command.Parameters.AddWithValue("@customerid", bike.Customer.Id);
                    command.Parameters.AddWithValue("@id", bike.Id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new CustomerRepositoryException("UpdateBike", ex); }
        }
        public void UpdateCustomer(Customer customer)
        {
            try
            {
                string sql = "UPDATE customer SET name=@name,email=@email,address=@address WHERE id=@id and status=1";
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@name", customer.Name);
                    command.Parameters.AddWithValue("@email", customer.Email);
                    command.Parameters.AddWithValue("@address", customer.Address);
                    command.Parameters.AddWithValue("@id", customer.Id);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { throw new CustomerRepositoryException("UpdateCustomer", ex); }
        }
    }
}
