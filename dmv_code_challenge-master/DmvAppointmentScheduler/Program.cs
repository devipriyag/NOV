using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DmvAppointmentScheduler
{
    class Program
    {
        public static Random random = new Random();
        public static List<Appointment> appointmentList = new List<Appointment>();
        static void Main(string[] args)
        {
            CustomerList customers = ReadCustomerData();
            TellerList tellers = ReadTellerData();
            Calculation(customers, tellers);
            OutputTotalLengthToConsole();

        }
        private static CustomerList ReadCustomerData()
        {
            string fileName = "CustomerData.json";            
            string path = Path.Combine(Path.GetDirectoryName(@"InputData\"), fileName);
            string jsonString = File.ReadAllText(path);
            CustomerList customerData = JsonConvert.DeserializeObject<CustomerList>(jsonString);
            return customerData;

        }
        private static TellerList ReadTellerData()
        {
            string fileName = "TellerData.json";            
            string path = Path.Combine(Path.GetDirectoryName(@"InputData\"), fileName);
            string jsonString = File.ReadAllText(path);
            TellerList tellerData = JsonConvert.DeserializeObject<TellerList>(jsonString);
            return tellerData;

        }
        static void Calculation(CustomerList customers, TellerList tellers)
        {
            var type1index = 0;
            var type2index = 0;
            var type3index = 0;
            var type4index = 0;

            //----Start customer order by, We can sort the customers by duration and use if there is no first come first serve 
            //and replace foreach with  customersSorted instead of Customers.customer 
            List<Customer> customersSorted = new List<Customer>();
            customersSorted = customers.Customer.OrderByDescending(c => c.duration).ToList();
            //----End customer order by------------------

            foreach (Customer customer in customers.Customer)
            {
                // Get tellers by type
                var tellersByTypeOnly = tellers.TellerByType(customer.type);
                // ---- Start Prioritize tellers by multiplier, minimum time taken this way by tellers to finish serving customers.
                var tellersByTypePrioritizedMultiplier = tellersByTypeOnly.OrderByDescending(a => a.multiplier).ToList();
                // ---- End Prioritize tellers by multiplier

                //Console.WriteLine("serving customerId: " + customer.Id + " and Type: " + customer.type);

                // Take turns with customer appointments
                Teller servingTeller = new Teller();
                if (customer.type == "1")
                {
                    servingTeller = tellersByTypePrioritizedMultiplier[type1index];
                    type1index++;
                    if(type1index > tellersByTypePrioritizedMultiplier.Count - 1)
                    {
                        type1index = 0;
                    }
                }
                else if(customer.type == "2")
                {
                    servingTeller = tellersByTypePrioritizedMultiplier[type2index];
                    type2index++;
                    if (type2index > tellersByTypePrioritizedMultiplier.Count - 1)
                    {
                        type2index = 0;
                    }
                }
                else if (customer.type == "3")
                {
                    servingTeller = tellersByTypePrioritizedMultiplier[type3index];
                    type3index++;
                    if (type3index > tellersByTypePrioritizedMultiplier.Count - 1)
                    {
                        type3index = 0;
                    }
                }
                else if (customer.type == "4")
                {
                    servingTeller = tellersByTypePrioritizedMultiplier[type4index];
                    type4index++;
                    if (type4index > tellersByTypePrioritizedMultiplier.Count - 1)
                    {
                        type4index = 0;
                    }
                }

                // TODO: prioritize tellers by multiplier


                Console.WriteLine("teller type:" + servingTeller.specialtyType + " tellerId: " + servingTeller.id);
                var appointment = new Appointment(customer, servingTeller);
                appointmentList.Add(appointment);
            }
        }
        static void OutputTotalLengthToConsole()
        {
            var tellerAppointments =
                from appointment in appointmentList
                group appointment by appointment.teller into tellerGroup
                select new
                {
                    teller = tellerGroup.Key,
                    totalDuration = tellerGroup.Sum(x => x.duration),
                };
            var max = tellerAppointments.OrderBy(i => i.totalDuration).LastOrDefault();
            Console.WriteLine("Teller " + max.teller.id + " will work for " + max.totalDuration + " minutes!");
        }

    }
}
