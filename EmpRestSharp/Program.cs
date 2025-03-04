using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using EmpRestSharp;
using Newtonsoft.Json;
using RestSharp;

public class Program
{
    private static RestClient client;
    private static async Task Main(string[] args)
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
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        //Initialize the client 
        client = new RestClient("http://localhost:3000");



        //Call diffrent method to interact with API
        Console.WriteLine("Adding a new employee...");
        await AddNewEmployee("7", "ankita", "500000");

        Console.WriteLine("employee list :");
        await GetEmployeeList();


        Console.WriteLine(" multiple employee added :");
        await AddMultipleEmployee();

        Console.WriteLine(" update employee :");
        await UpdateEmployeeSalary("2", "45000");

        Console.WriteLine(" deleting employee :");
        await DeleteEmployeeById(5);

    }


    private static async Task AddNewEmployee(string Id, string name, string salary)
    {
        var request = new RestRequest("Employees", Method.Post);
        var jsonObj = new
        {
            Id = Id,
            name = name,
            salary = salary
        };

        request.AddJsonBody(jsonObj);

        var response = await client.ExecuteAsync(request);

        if (response.StatusCode == HttpStatusCode.Created)
        {
            var employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Console.WriteLine($"Added Employee: {employee.Name}, Salary: {employee.Salary}");
        }
        else
        {
            Console.WriteLine($"Failed to add employee. Status: {response.StatusCode}");
        }
    }


    // get employee list

    private static async Task GetEmployeeList()
    {
        var request = new RestRequest("Employees", Method.Get);
        var response = await client.ExecuteAsync(request);

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            List<Employee> employeeList = JsonConvert.DeserializeObject<List<Employee>>(response.Content);

            foreach (Employee employee in employeeList)
            {

                Console.WriteLine($"Id = {employee.Id}, EmpName = {employee.Name}, EmpSalary = {employee.Salary}");


            }
        }
        else {
            Console.WriteLine($"failed to  get employee , status :{response.StatusCode}");
        }
    }

    private static async  Task AddMultipleEmployee()
    {
        var addMultipleEmp = new List<Employee>{
                new Employee { Name = "pranay", Salary = "85536" },
                new Employee { Name = "pravin", Salary = "120123" },
                new Employee { Name = "Chetan", Salary = "123456" }
                };

        foreach (Employee emp in addMultipleEmp) {
            var request = new RestRequest("Employees", Method.Post);
            var jsonObj = new
            {
                
                name = emp.Name,
                Salary = emp.Salary
            };
            request.AddJsonBody(jsonObj);

            var response = await client.ExecuteAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var addEmp = JsonConvert.DeserializeObject<Employee>(response.Content);
                Console.WriteLine($"Added Emplyee : Id = {addEmp.Id}, Name = {addEmp.Name}, Salary = {addEmp.Salary}");
            }
            else {
                Console.WriteLine($"Error : failed to add employee {emp.Name}, Status : {response.StatusCode}");
            }
        }
        
            
    }

    private static async Task UpdateEmployeeSalary(string id, string newSalary)
    {
        var request = new RestRequest($"Employees/{id}", Method.Put);
        var jsonObj = new {
            
           Salary = newSalary

        } ;

        request.AddJsonBody(jsonObj);

        var response = await client.ExecuteAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var updateEmp = JsonConvert.DeserializeObject<Employee>(response.Content);
            Console.WriteLine($"Updated Employee: Employee = {updateEmp.Id}, New Salary = {updateEmp.Salary}");
        }
        else
        {
            Console.WriteLine($"failed to update employee. Status : {response.StatusCode}");
        }
    }

    private static async Task DeleteEmployeeById(int id)
    {
        var request = new RestRequest($"Employees/{id}" , Method.Delete);
        var response = await client.ExecuteAsync(request);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            Console.WriteLine($"Employee deleted successfully with  Id ={id}.");
        }
        else {
            Console.WriteLine($" failed to dekte employee not found . status :{response.StatusCode}");
        }
    }
}

