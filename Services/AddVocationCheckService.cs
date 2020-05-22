using System;
using System.Collections.Generic;
using System.Linq;
using TestWebApplication.Models;
using TestWebApplication.Data;


namespace TestWebApplication.Services
{
    public class AddVocationCheckService
    {
        private Vocation vocation;
        private EmployeeContext employeeDb;
        private VocationContext vocationDb;
        const int MAX_DAYS_PER_YEAR = 28;
        private bool CheckValidDate() //проверка данных отпуска, startData < endDate, startDat >= today
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
        private int CountVocations(string position) //нахождение количесства пересечений по датам для позиции
        {
            int count = 0;
            if (vocationDb.Vocations.Any())
            {
                IEnumerable<Employee> employees = employeeDb.Employees
                    .Where(emp => emp.position.Equals(position)); // фильтрация сотрудников по позиции в базе сотрудников
                List<Vocation> vocations = new List<Vocation>();
                foreach (var emp in employees)
                {
                    vocations.AddRange(vocationDb.Vocations         //фильтрация отпусков по позиции в базе отпусков
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

        private bool CheckDaysPerYear() //проверка количества отпускных дней для сотрудника, <=28
        {
            IEnumerable<Vocation> vocations = vocationDb.Vocations
                .Where(voc => voc.employeeId == vocation.employeeId); //вывод ранее оформленных отпусков для конкретного сотрудника
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
                return (diffInDays <= MAX_DAYS_PER_YEAR) ? true : false;
            }
            else
            {
                TimeSpan difference = vocation.endDate - vocation.startDate;
                return (difference.Days <= MAX_DAYS_PER_YEAR) ? true : false;
            }

        }
        
        private string FindEmployeePosition() //нахождение должности сотрудника
        {
            return employeeDb.Employees
                .FirstOrDefault(x => x.id == vocation.employeeId).position;
        }

        public bool Check(Vocation voc, EmployeeContext employeeContext, VocationContext vocationContext) //проверка возможности оформления отпуска
        {
            vocation = voc;
            employeeDb = employeeContext;
            vocationDb = vocationContext;
            if ((CheckValidDate())
                && (CheckDaysPerYear())) 
            {
                int devCount = CountVocations("Dev"); //подсчет количества пересекающихся по датам отпусков для позиции "Dev"
                int qaCount = CountVocations("QA");  //подсчет количества пересекающихся по датам отпусков для позиции "QA"
                int tlCount = CountVocations("TeamLead");   //подсчет количества пересекающихся по датам отпусков для позиции "TeamLead"
                switch (FindEmployeePosition())
                {
                    case "Dev":
                        return ((tlCount < 1) && (devCount < 2) && (qaCount < 2)) ? true : false;

                    case "TeamLead":
                        return ((devCount < 1) && (tlCount < 1)) ? true : false;

                    case "QA":
                        return ((qaCount < 3) && ((devCount < 1) || (qaCount < 1))) ? true : false;

                    default:
                        return false;
                }
            }
            return false;
        }
    }
}
