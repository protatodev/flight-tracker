using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace FlightTracker.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public City(string Name, int Id = 0)
        {
            this.Id = Id;
            this.Name = Name;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public void AddFlight(Flight newFlight)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_flights (flight_id, city_id) VALUES (@FlightId, @CityId);";

            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@FlightId";
            flight_id.Value = newFlight.Id;
            cmd.Parameters.Add(flight_id);

            MySqlParameter city_id = new MySqlParameter();
            city_id.ParameterName = "@CityId";
            city_id.Value = Id;
            cmd.Parameters.Add(city_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Flight> GetFlights()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT flights.* FROM cities
                                JOIN cities_flights ON (cities.id = cities_flights.city_id)
                                JOIN flights ON (cities_flights.flight_id = flights.id)
                                WHERE cities.id = @cityId;";

            MySqlParameter cityIdParameter = new MySqlParameter();
            cityIdParameter.ParameterName = "@cityId";
            cityIdParameter.Value = Id;
            cmd.Parameters.Add(cityIdParameter);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            List<Flight> flights = new List<Flight> { };

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                int flightNum = rdr.GetInt32(1);
                TimeSpan time = rdr.GetTimeSpan(2);
                string arrival_departure = rdr.GetString(3);
                string status = rdr.GetString(4);
                Flight newFlight = new Flight(flightNum, time, arrival_departure, status, id);
                flights.Add(newFlight);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return flights;
        }

        public void Edit(string newCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE cities SET name = @newCity WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = Id;
            cmd.Parameters.Add(searchId);

            MySqlParameter cityNum = new MySqlParameter();
            cityNum.ParameterName = "@newCity";
            cityNum.Value = newCity;
            cmd.Parameters.Add(cityNum);

            cmd.ExecuteNonQuery();
            this.Name = newCity;

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
            cmd.CommandText = @"DELETE FROM cities WHERE id = @CityId; DELETE FROM cities_flights WHERE city_id = @CityId;";

            MySqlParameter cityIdParameter = new MySqlParameter();
            cityIdParameter.ParameterName = "@CityId";
            cityIdParameter.Value = this.Id;
            cmd.Parameters.Add(cityIdParameter);

            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static List<City> GetAll()
        {
            List<City> allCities = new List<City> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();

            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities;";

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while (rdr.Read())
            {
                string name = rdr.GetString(1);
                int id = rdr.GetInt32(0);

                City newCity = new City(name, id);
                allCities.Add(newCity);
            }

            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }

            return allCities;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM cities;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherCity)
        {
            if (!(otherCity is City))
            {
                return false;
            }
            else
            {
                City newCity = (City)otherCity;
                bool idEquality = (this.Id == newCity.Id);
                bool cityNameEquality = (this.Name == newCity.Name);

                return (idEquality && cityNameEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities (name) VALUES (@cityName);";

            MySqlParameter cityName = new MySqlParameter();
            cityName.ParameterName = "@cityName";
            cityName.Value = this.Name;
            cmd.Parameters.Add(cityName);

            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static City Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM `cities` WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int cityId = 0;
            string cityName = "";

            while (rdr.Read())
            {
                cityId = rdr.GetInt32(0);
                cityName = rdr.GetString(1);
            }

            City foundCity = new City(cityName, cityId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundCity;
        }
    }
}
