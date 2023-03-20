using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Linq;

namespace BusinessLayer
{
    public static class DomainFactory
    {       
        public static Customer NewCustomer(CustomerInfo customerInfo)
        {
            try
            {
                return new Customer(customerInfo.Name, customerInfo.Email, customerInfo.Address);
            }
            catch (Exception ex) { throw new DomainException("NewCustomer", ex); }
        }
        public static Customer ExistingCustomer(int id, string name, string email, string address, List<Bike> bikes)
        {
            try
            {
                return new Customer(id, name, email, address, bikes);
            }
            catch (Exception ex) { throw new DomainException("ExistingCustomer", ex); }
        }       
        public static Bike NewBike(BikeInfo bikeInfo)
        {
            try
            {
                return new Bike(bikeInfo.Id, bikeInfo.BikeType, bikeInfo.PurchaseCost, bikeInfo.Description);
            }
            catch (Exception ex) { throw new DomainException("NewBike", ex); }
        }
        public static Bike ExistingBike(int? id, BikeType bikeType, double purchaseCost, string? description)
        {
            try
            {
                return new Bike(id,bikeType,purchaseCost,description);
            }
            catch (Exception ex) { throw new DomainException("NewBike", ex); }
        }      
    }
}
