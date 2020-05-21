using System;
using System.Collections.Generic;
using System.Linq;
using TestWebApplication.Models;

namespace TestWebApplication.Controllers
{
    public static class AddVocationCheck
    {
        static int CountVocations(string position, Vocation vocation, EmployeeContext employeeDb, VocationContext vocationDb)
        {
            int count = 0;
            if (vocationDb.Vocations.Any())
            {
                IEnumerable<Employee> employees = employeeDb.Employees
                    .Where(emp => emp.position.Equals(position));
                List<Vocation> vocations = new List<Vocation>();
                foreach (var emp in employees)
                {
                    vocations.AddRange(vocationDb.Vocations
                        .Where(voc => voc.employeeId == emp.id));
                }
                foreach (var voc in vocations)
                {
                    if ((DateTimeOffset.Compare(vocation.endDate, voc.startDate) >= 0)
                        && (DateTimeOffset.Compare(vocation.startDate, voc.endDate) <= 0))
                    {
                        if (vocation.employeeId == voc.employeeId)
                        {
                            count = 10;
                        }
                        else
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
            else
            {
                return 0;
            }
        }

        static bool CheckDaysPerYear(Vocation vocation, VocationContext vocationDb)
        {
            IEnumerable<Vocation> vocations = vocationDb.Vocations
                .Where(voc => voc.employeeId == vocation.employeeId);
            if (vocations.Any())
            {
                int diffInDays = 0;
                vocations = vocations
                    .Where(voc => voc.startDate.Year == vocation.startDate.Year);
                foreach (var voc in vocations)
                {
                    TimeSpan diff = voc.endDate - voc.startDate;
                    diffInDays += diff.Days;
                }
                TimeSpan difference = vocation.endDate - vocation.startDate;
                diffInDays += difference.Days;
                if (diffInDays <= 28)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                TimeSpan diff = vocation.endDate - vocation.startDate;
                if (diff.Days <= 28)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

        }
        static bool CheckValidDate(Vocation vocation)
        {
            DateTime today = DateTime.Today;
            if ((DateTimeOffset.Compare(vocation.startDate, today) >= 0)
                && (DateTimeOffset.Compare(vocation.endDate, vocation.startDate) > 0)
                && (vocation.startDate.Year == vocation.endDate.Year))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static string FindEMployeePosition(Vocation vocation, EmployeeContext employeeDb)
        {
            return employeeDb.Employees
                .FirstOrDefault(x => x.id == vocation.employeeId).position;
        }
        static public bool Check(Vocation vocation, EmployeeContext employeeDb, VocationContext vocationDb)
        {
            if ((CheckValidDate(vocation))
                && (CheckDaysPerYear(vocation, vocationDb)))
            {
                int devCount = CountVocations("Dev", vocation, employeeDb, vocationDb);
                int qaCount = CountVocations("QA", vocation, employeeDb, vocationDb);
                int tlCount = CountVocations("TeamLead", vocation, employeeDb, vocationDb);
                switch (FindEMployeePosition(vocation, employeeDb))
                {
                    case "Dev":
                        if ((tlCount < 1) && (devCount < 2) && (qaCount < 2))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case "TeamLead":
                        if ((devCount < 1) && (tlCount < 1))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    case "QA":
                        if ((qaCount < 3) && ((devCount < 1) || (qaCount < 1)))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    default:
                        return false;
                }
            }
            return false;
        }
    }
}
