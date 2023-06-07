using System.Runtime.InteropServices.JavaScript;

namespace iBOS.Model;

public class EmployeeAttendence
{
    public int EmployeeId { get; set; }
    public DateTime AttendanceDate { get; set; }
    public int Ispresent { get; set; }
    public int  IsAbsent { get; set; }
    public int IsOffday { get; set; }
}