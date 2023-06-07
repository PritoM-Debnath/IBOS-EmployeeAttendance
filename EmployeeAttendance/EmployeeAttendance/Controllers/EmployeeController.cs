using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using iBOS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EmployeeAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet("getAllDescSal")]
        public JsonResult Get()
        {
            string query = @"select employeeName,employeeCode, employeeSalary from tblEmployee order by employeeSalary desc";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException();
            MySqlDataReader myReader;
            using (MySqlConnection myCon = new MySqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }
        
        
        [HttpPut("{id}")]
        public JsonResult Put(int id, Employee employee)
        {
            string query = @"UPDATE tblEmployee
                     SET 
                     employeeCode = @EmployeeCode
                     WHERE employeeId = @EmployeeId";

            string sqlDataSource = _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
            MySqlDataReader myReader;
            using (MySqlConnection myCon = new MySqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeCode", employee.EmployeeCode);
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);

                    myReader = myCommand.ExecuteReader();

                    myReader.Close();
                    myCon.Close();
                }

                return new JsonResult("Update successfully");
            }
        }
        
    }
}
