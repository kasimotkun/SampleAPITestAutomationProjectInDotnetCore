using System;
using NUnit.Framework;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GfK_TestAutomationTask2_NUnit
{
    public class APITests
    {
       public static int newlyCreatedEmployeeID;

        [SetUp]
        public void Setup()
        {
        }

        [Test, Order(1)]
        public void TestGetEmployees()
        {
            
              var client = new RestClient("http://dummy.restapiexample.com/api/v1/employees");
            var request = new RestRequest(Method.GET);
              request.AddCookie("PHPSESSID","8728c2cc7be5ef660cf12d93b0a7662a");
            IRestResponse response = client.Execute(request);

            StringAssert.Contains("\"status\":\"success\"",response.Content);
         

            List<Employee> Employees = JsonConvert.DeserializeObject<List<Employee>>(response.Content.Replace("{\"status\":\"success\",\"data\":","").Replace("]}","]"));
            StringAssert.AreEqualIgnoringCase(Employees[0].id,"1");
        }

         [Test, Order(2)]
        public void TestPostNewEmployee()
        {
            
            NewEmployee newEmployee = new NewEmployee();
            newEmployee.name="test_gk_unique";
            newEmployee.age="23";
            newEmployee.salary="123";
            
            var client = new RestClient("http://dummy.restapiexample.com/api/v1/create");
            var request = new RestRequest(Method.POST);

            request.RequestFormat = DataFormat.Json;


            request.AddJsonBody( JsonConvert.SerializeObject(newEmployee));
            IRestResponse response = client.Execute(request);

            StringAssert.Contains("\"status\":\"success\"",response.Content);



            NewlyCreatedEmployee createdEmployee = JsonConvert.DeserializeObject<NewlyCreatedEmployee>(response.Content.Replace("{\"status\":\"success\",\"data\":","").Replace("}}","}"));

            newlyCreatedEmployeeID = createdEmployee.id;

            Console.WriteLine(response.Content);

            Console.WriteLine("newlyCreatedEmployeeID ="+newlyCreatedEmployeeID);

            StringAssert.AreEqualIgnoringCase(newEmployee.name,createdEmployee.name);
        }

         [Test, Order(3)]
        public void TestGetEmployeeById()
        {
            
            
            var client = new RestClient("http://dummy.restapiexample.com/api/v1/employee/"+newlyCreatedEmployeeID);
            var request = new RestRequest(Method.GET);
         
             request.AddCookie("PHPSESSID","testcookie");
            IRestResponse response = client.Execute(request);
             response = client.Execute(request);

         
            StringAssert.Contains("\"status\":\"success\"",response.Content);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content.Replace("{\"status\":\"success\",\"data\":","").Replace("}}","}"));
            Console.WriteLine(response.Content);
            
            StringAssert.AreEqualIgnoringCase(employee.id, newlyCreatedEmployeeID.ToString());
        }

        

         [Test, Order(4)]
        public void TestUpdateEmployeeById()
        {
            NewEmployee newEmployee = new NewEmployee();
            newEmployee.name="test_gk_unique"+new Random().Next(100,1000);
            newEmployee.age="23";
            newEmployee.salary="123";
            
            var client = new RestClient("http://dummy.restapiexample.com/api/v1/update/"+newlyCreatedEmployeeID);
            var request = new RestRequest(Method.PUT);
                
            Console.WriteLine(client.BaseUrl);
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody( JsonConvert.SerializeObject(newEmployee));
            IRestResponse response = client.Execute(request);

            StringAssert.Contains("\"status\":\"success\"",response.Content);


            Console.WriteLine(response.Content);

            NewlyCreatedEmployee updatedEmployee = JsonConvert.DeserializeObject<NewlyCreatedEmployee>(response.Content.Replace("{\"status\":\"success\",\"data\":","").Replace("}}","}"));



            StringAssert.AreEqualIgnoringCase( newEmployee.name,updatedEmployee.name);
            
          
        }
        
        [Test, Order(5)]
         public void TestDeleteEmployeeById()
        {
            
            
            var client = new RestClient("http://dummy.restapiexample.com/api/v1/delete/"+newlyCreatedEmployeeID);
            var request = new RestRequest(Method.DELETE);
             
            Console.WriteLine(client.BaseUrl);
            IRestResponse response = client.Execute(request);

         
              StringAssert.Contains("\"status\":\"success\"",response.Content);
              StringAssert.Contains("\"message\":\"successfully! deleted Records\"",response.Content);
           
            
        }

    }
}