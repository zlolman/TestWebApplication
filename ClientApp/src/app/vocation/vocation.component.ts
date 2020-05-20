import { Component, OnInit, DoCheck } from '@angular/core';
import { VocationDataService } from './vocation.data.service';
import { Vocation } from './vocation';
import { Employee } from '../employee/employee';
import { EmployeeDataService } from '../employee/employee.data.service';


@Component({
  selector: 'app-vocation-component',
  templateUrl: './vocation.component.html',
  providers: [VocationDataService, EmployeeDataService]
})
export class VocationComponent implements OnInit {
  vocation: Vocation = new Vocation();
  vocations: Array<Vocation>;
  employee: Employee;
  employees: Array<Employee>;
  list: Array<Box> = [];
  vocloaded: boolean = false;
  emploaded: boolean = false;

  constructor(private dataService: VocationDataService, private employeeService: EmployeeDataService) { }

  ngOnInit() {
    this.loadEmployees();
    this.loadVocations();
  }
  ngDoCheck() {
    this.list = [];
    this.makeList();
  }

  loadVocations() {
    this.dataService.getVocations()
      .subscribe((data: Vocation[]) => {
        this.vocations = data;
        if (this.vocations != null) this.vocloaded = true;
      })    
  }

  loadEmployees() {
    this.employeeService.getEmployees()
      .subscribe((data: Employee[]) => {
        this.employees = data;
        if (this.employees != null) this.emploaded = true;
      });
  }

  makeList() {
    if (this.vocloaded && this.emploaded) {
      for (let i = 0, len = this.vocations.length; i < len; i++) {
        let line = new Box();
        line.voc = this.vocations[i];
        line.emp = this.employees.filter(x => x.id == this.vocations[i].employeeId)[0];
        if (line != null) this.list.push(line);
      }
    }
  }

  delete(p: Vocation) {    
    this.dataService.deleteVocation(p.id)
      .subscribe(data => {
        this.loadVocations();
        this.loadEmployees();            
      });    
  }
}

class Box {
  constructor(public voc?: Vocation, public emp?: Employee) { }
}
