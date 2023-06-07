using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace EmployeeAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAttendenceController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeAttendenceController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet("getAtleastOneDayAbsent")]
        public JsonResult Get()
        {
            string query = @"select employeeId, attendanceDate from tblEmployeeAttendance where isAbsent = 1";

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
        
        [HttpGet("getReport")]
        public JsonResult GetReport()
        {
            string query = @"SELECT
    e.employeeName AS 'Employee Name',
    DATE_FORMAT(a.attendanceDate, '%M') AS 'Month Name',
    SUM(a.isPresent) AS 'Total Present',
    SUM(a.isAbsent) AS 'Total Absent',
    SUM(a.isOffday) AS 'Total Offday'
FROM
    tblemployee e
        JOIN tblEmployeeAttendance a ON e.employeeId = a.employeeId
GROUP BY
    e.employeeName,
    DATE_FORMAT(a.attendanceDate, '%M')
ORDER BY
    e.employeeName,
    DATE_FORMAT(a.attendanceDate, '%M')";

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
    }
}
