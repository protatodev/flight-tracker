using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FlightTracker.Models
{
    public class Flight
    {
        public int FlightNum { get; set; }
        public int Id { get; set; } 
        public TimeSpan Time { get; set; }
        public string Arrival_Departure { get; set; }
        public string Status { get; set; }

        public Flight(int FlightNum, TimeSpan Time, string Arrival_Departure, string Status, int Id = 0)
        {
            this.FlightNum = FlightNum;
            this.Id = Id;
            this.Time = Time;
            this.Arrival_Departure = Arrival_Departure;
            this.Status = Status;
        }

        public override int GetHashCode()
        {
            return this.FlightNum.GetHashCode();
        }

        public void AddCity(City newCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);";

            MySqlParameter city_id = new MySqlParameter();
            city_id.ParameterName = "@CityId";
            city_id.Value = newCity.Id;
            cmd.Parameters.Add(city_id);

            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@FlightId";
            flight_id.Value = Id;
            cmd.Parameters.Add(flight_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<City> GetCities()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT cities.* FROM flights
                                JOIN cities_flights ON (flights.id = cities_flights.flight_id)
                                JOIN cities ON (cities_flights.city_id = cities.id)
                                WHERE flights.id = @flightId;";

            MySqlParameter flightIdParameter = new MySqlParameter();
            flightIdParameter.ParameterName = "@flightId";
            flightIdParameter.Value = Id;
            cmd.Parameters.Add(flightIdParameter);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<City> cities = new List<City> { };

            while (rdr.Read())
            {
                int cityId = rdr.GetInt32(0);
                string cityName = rdr.GetString(1);
                City newCity = new City(cityName, cityId);
                cities.Add(newCity);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return cities;
        }

        public void Edit(int newFlightNum, TimeSpan newTime, string newArrival_Departure, string newStatus, int newCityId)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE flights SET flight_num = @newFlight, time = @newTime, arrival_departure = @newArrival_Departure, status = @newStatus  WHERE id = @searchId; UPDATE cities_flights SET city_id = @newCityId WHERE flight_id = @searchId";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = Id;
            cmd.Parameters.Add(searchId);

            MySqlParameter flightNum = new MySqlParameter();
            flightNum.ParameterName = "@newFlight";
            flightNum.Value = newFlightNum;
            cmd.Parameters.Add(flightNum);

            MySqlParameter time = new MySqlParameter();
            time.ParameterName = "@newTime";
            time.Value = newTime;
            cmd.Parameters.Add(time);

            MySqlParameter arrival_departure = new MySqlParameter();
            arrival_departure.ParameterName = "@newArrival_Departure";
            arrival_departure.Value = newArrival_Departure;
            cmd.Parameters.Add(arrival_departure);

            MySqlParameter status = new MySqlParameter();
            status.ParameterName = "@newStatus";
            status.Value = newStatus;
            cmd.Parameters.Add(status);

            MySqlParameter cityId = new MySqlParameter();
            cityId.ParameterName = "@newCityId";
            cityId.Value = newCityId;
            cmd.Parameters.Add(cityId);

            cmd.ExecuteNonQuery();
            this.FlightNum = newFlightNum;
            this.Time = newTime;
            this.Arrival_Departure = newArrival_Departure;
            this.Status = newStatus;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights WHERE id = @FlightId; DELETE FROM cities_flights WHERE flight_id = @FlightId;";

            MySqlParameter flightIdParameter = new MySqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = this.Id;
            cmd.Parameters.Add(flightIdParameter);

            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<City> GetAllCities()
        {
            return City.GetAll();
        }

        public static List<Flight> GetAll()
        {
            List<Flight> allFlights = new List<Flight> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights;";

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                int flightNum = rdr.GetInt32(1);
                int id = rdr.GetInt32(0);
                TimeSpan time = rdr.GetTimeSpan(2);
                string arrival_Departure = rdr.GetString(3);
                string status = rdr.GetString(4);

                Flight newFlight = new Flight(flightNum, time, arrival_Departure, status, id);
                allFlights.Add(newFlight);
            }

            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }

            return allFlights;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherFlight)
        {
            if (!(otherFlight is Flight))
            {
                return false;
            }
            else
            {
                Flight newFlight = (Flight)otherFlight;
                bool idEquality = (this.Id == newFlight.Id);
                bool flightNumEquality = (this.FlightNum == newFlight.FlightNum);

                return (idEquality && flightNumEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (flight_num, time, arrival_departure, status ) VALUES (@flightNum, @time, @arrival_departure, @status);";

            MySqlParameter flightNum = new MySqlParameter();
            flightNum.ParameterName = "@flightNum";
            flightNum.Value = this.FlightNum;
            cmd.Parameters.Add(flightNum);

            MySqlParameter time = new MySqlParameter();
            time.ParameterName = "@time";
            time.Value = this.Time;
            cmd.Parameters.Add(time);

            MySqlParameter arrival_departure = new MySqlParameter();
            arrival_departure.ParameterName = "@arrival_departure";
            arrival_departure.Value = this.Arrival_Departure;
            cmd.Parameters.Add(arrival_departure);

            MySqlParameter status = new MySqlParameter();
            status.ParameterName = "@status";
            status.Value = this.Status;
            cmd.Parameters.Add(status);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Flight Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `flights` WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int flightId = 0;
            int flightNum = 0;
            TimeSpan time;
            string arrival_departure = "";
            string status = "";

            while (rdr.Read())
            {
                flightId = rdr.GetInt32(0);
                flightNum = rdr.GetInt32(1);
                time = rdr.GetTimeSpan(2);
                arrival_departure = rdr.GetString(3);
                status = rdr.GetString(4);
            }

            Flight foundFlight = new Flight(flightNum, time, arrival_departure, status, flightId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundFlight;
        }
    }
}