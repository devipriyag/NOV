using System;
using System.Collections.Generic;
using System.Text;

namespace DmvAppointmentScheduler
{
    public class Teller
    {
        public string id { get; set; }
        public string specialtyType { get; set; }
        public string multiplier { get; set; }
    }

    public class TellerList
    {
        public List<Teller> Teller { get; set; }
        public List<Teller> TellerByType(string type)
        {
            List<Teller> tellerByType = new List<Teller>();
            foreach (Teller teller in this.Teller)
            {
                if(teller.specialtyType == type)
                {
                    tellerByType.Add(teller);
                }
            }
            if(tellerByType.Count == 0)
            {
                tellerByType = this.Teller;
            }
            return tellerByType;
        }
    }
}
