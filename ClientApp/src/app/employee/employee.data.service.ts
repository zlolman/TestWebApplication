import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Employee } from './employee';
import { Observable,  } from 'rxjs';
import { retry } from 'rxjs/operators';

@Injectable()
export class EmployeeDataService {

  private url = "/employee";

  constructor(private http: HttpClient) {  }

  getEmployees(): Observable<Employee[]>
  {
    return this.http.get<Employee[]>(this.url)
  }

  getEmployee(id: number): Observable<Employee>
  {
    return this.http.get(this.url + '/' + id)
  }

  createEmployee(employee: Employee): Observable<Employee>
  {
    return this.http.post(this.url, employee)
  }

  updateEmployee(employee: Employee): Observable<Employee>
  {
    return this.http.put(this.url, employee)
  }

  deleteEmployee(id: number): Observable<Employee>
  {
    return this.http.delete(this.url + '/' + id)
  }
}
