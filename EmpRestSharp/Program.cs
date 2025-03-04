using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using EmpRestSharp;
using Newtonsoft.Json;
using RestSharp;

public class Program
{
    private static RestClient client;
    private static void Main(string[] args)
    {
        var client = new RestClient("http://localhost:3000");

        //create GET request
        var request = new RestRequest("Employees", Method.Get);

        //EXCUTE request
        var response = client.Execute(request);

        //check Responce status 

        if (response.StatusCode == HttpStatusCode.OK)
        {
            Console.WriteLine("Data retrived successfully");

            //deserilize the responce content

            var employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);

            //print the employee details
            if (employeeList != null)
            {
                foreach (var employee in employeeList)
                {
                    Console.WriteLine($"Id = {employee.Id}, EmpName = {employee.Name}, EmpSalary = {employee.Salary}");
                }
            }
            else
            {
                Console.WriteLine("No employees found or failed to deserialize.");
            }
        }
        else
        {
            Console.WriteLine($"Error : {response.StatusCode}, message : {response.Content}");
        }
    }
}
        ////Initialize the client 
        //client = new RestClient("http://localhost:3000");

        ////Call diffrent method to interact with API
        //Console.WriteLine("Adding a new employee...");
        //AddNewEmployee("5", "Lucas", "50000");

        //}

        //public static async Task AddNewEmployee(string id, string name, string salary)
        //{
        //    var request = new RestRequest("Employee", Method.Post);

        //    var jsonObj = new
        //    {
        //        id = id,
        //        name = name,
        //        salary = salary
        //    };
        //    request.AddJsonBody(jsonObj);

        //    var response = await client.ExecuteAsync(request);

        //    if (response.StatusCode == HttpStatusCode.Created)
        //    {
        //        var employee = JsonConvert.DeserializeObject<Employee>(response.Content);
        //        Console.WriteLine($"Added Employee: {employee.Name}, Salary: {employee.Salary}");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Failed to add employee. Status: {response.StatusCode}");
        //    }

        //}
    //}
