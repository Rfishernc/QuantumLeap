using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using QuantumLeap.Models;

namespace QuantumLeap.Data
{
    public class LeapQueries
    {
        const string ConnectionString = "Server = localhost; Database = QuantumLeap; Trusted_Connection = True;";

        public Leapee AddLeapee(string name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = @"Insert into Leapee(Name)
                                    Output inserted.*
                                    Values(@Name)";
                var newLeapee = connection.QueryFirstOrDefault<Leapee>(queryString, new { name });
                if (newLeapee != null)
                {
                    return newLeapee;
                }
            }
            throw new Exception("Leapee could not be added");
        }

        public Event GetAnUncompletedEvent()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = @"Select *
                                    From Event
                                    Where Id >= FLOOR(
	                                    RAND() * (
		                                    Select Count(*)
		                                    From Event
		                                    Where Event.IsCompleted = 0
		                                    )
	                                    )";
                var events = connection.QueryFirstOrDefault<Event>(queryString);
                return events;
            }
        }

        public Leaper GetALeaper(string name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = @"Select *
                                    From Leaper
                                    Where Leaper.Name = @Name";
                var leaper = connection.QueryFirstOrDefault<Leaper>(queryString, new { name });
                if (leaper != null)
                {
                    return leaper;
                }
            }
            throw new Exception("Leaper not found.");
        }

        public Budget GetBudget()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = @"Select * From Budget";
                var budget = connection.QueryFirst<Budget>(queryString);
                return budget;
            }
        }

        public Budget UpdateBudget(double cost)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = @"Update Budget
                                    Output updated.*
                                    Set Capital = Capital - @Cost";
                var budget = connection.QueryFirst<Budget>(queryString, new { cost });
                return budget;
            }
        }

        public Leap AddALeap(Leap leap)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = @"Insert into Leap(EventId, LeaperId, Cost)
                                    Output inserted.*
                                    Values(@EventId, @LeaperId, @Cost)";
                var newLeap = connection.QueryFirst<Leap>(queryString, leap);
                return newLeap;
            }
        }

        public string GetALeapeeName(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = @"Select Name
                                    From Leapee
                                    Where Id = @Id";
                var leapeeName = connection.QueryFirstOrDefault<string>(queryString, new { id });
                return leapeeName;
            }
        }

        public Event CompleteEvent(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = @"Update Event
                                    Output updated.*
                                    Set IsCompleted = 1
                                    Where Id = @Id";
                var completedEvent = connection.QueryFirstOrDefault<Event>(queryString, new { id } );
                return completedEvent;
            }
        }

        public Leaper LeaperGoesHome(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                var queryString = @"Delete
                                    From Leaper
                                    Where Id = @Id";
                var deletedLeaper = connection.QueryFirstOrDefault<Leaper>(queryString, new { id });
                return deletedLeaper;
            }
        }
    }
}
