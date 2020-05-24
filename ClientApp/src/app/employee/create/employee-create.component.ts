import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Employee } from '../employee';
import { EmployeeDataService } from '../employee.data.service';
import { error } from '@angular/compiler/src/util';

@Component({
  templateUrl: './employee-create.component.html',
  providers: [EmployeeDataService]
})

export class EmployeeCreate
{
  employee: Employee = new Employee();
  positions: string[] = ["TeamLead", "Dev", "QA"];

  constructor(private router: Router, private employeeService: EmployeeDataService, private activeRoute: ActivatedRoute) { }

  save()
  {
    this.employeeService.createEmployee(this.employee).subscribe(data => {
      this.employee = data;
      if (data != null) {
        this.router.navigateByUrl('/employee')
      }
    }, error => {
        if (error != null) {
          this.router.navigateByUrl('/employee/error');
        }
    })
  }
}
