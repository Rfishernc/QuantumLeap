using QuantumLeap.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumLeap.Models
{
    public class Leap
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int LeaperId { get; set; }
        public double Cost { get; set; }

        public void TakeALeap(string name, double cost)
        {
            var connection = new LeapQueries();

            var budget = connection.GetBudget();
            var eventToLeap = connection.GetAnUncompletedEvent();
            var leaper = connection.GetALeaper(name);

            var potentialLeap = new Leap() { EventId = eventToLeap.Id, LeaperId = leaper.Id, Cost = cost };

            if (budget.LeapIsAuthorized(potentialLeap))
            {
                var newLeap = connection.AddALeap(potentialLeap);
                connection.UpdateBudget(cost);
                connection.CompleteEvent(eventToLeap.Id);
                if (connection.GetALeapeeName(eventToLeap.LeapeeId) == leaper.Name)
                {
                    connection.LeaperGoesHome(leaper.Id);
                }
            }
        }
    }
}
