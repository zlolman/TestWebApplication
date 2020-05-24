import { Component, OnInit } from '@angular/core';
import { EmployeeDataService } from './employee.data.service';
import { Employee } from './employee';
import { Router } from '@angular/router';
import { error } from '@angular/compiler/src/util';

@Component({
  selector: 'app-employee-component',
  templateUrl: './employee.component.html',
  providers: [EmployeeDataService]
})
export class EmployeeComponent implements OnInit {
  
  employee: Employee = new Employee();  
  employees: Employee[];                
  tableMode: boolean = true;   
  positions: string[] = ["TeamLead", "Dev", "QA"];

  constructor(private employeeService: EmployeeDataService, private router: Router) {};

  ngOnInit() {
    this.loadEmployees(); 
  }

  loadEmployees() {
    this.employeeService.getEmployees()
      .subscribe((data: Employee[]) => this.employees = data);
  }

  save() {
    if (this.employee.id == null) {
      this.employeeService.createEmployee(this.employee)
        .subscribe(data => {
          this.employee = data;
          if (this.employee != null) {
            this.cancel()
          }
        },
          error => {
            if (error != null) {
              this.router.navigateByUrl('/employee/error')
            }
          });
    } else {
      this.employeeService.updateEmployee(this.employee)
        .subscribe(data => {
          this.employee = data;
          if (this.employee != null) {
            this.cancel()
          }          
        },
          error => {
            if (error != null) {
              this.router.navigateByUrl('/employee/error')
            }
          });
    }
    this.cancel();
  }

  editEmployee(p: Employee)
  {
    this.employee = p;
  }

  cancel()
  {
    this.loadEmployees();
    this.employee = new Employee();
    this.tableMode = true;
  }

  delete(p: Employee)
  {
    this.employeeService.deleteEmployee(p.id)
      .subscribe(data => this.loadEmployees());
  }

  add()
  {
    this.cancel();
    this.tableMode = false;
  } 
  
}
