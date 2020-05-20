import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Employee } from './employee';
import { Observable,  } from 'rxjs';
import { retry } from 'rxjs/operators';

@Injectable()
export class EmployeeDataService {

  private url = "/employee";

  constructor(private http: HttpClient) {  }

  getEmployees(): Observable<Employee[]> {
    return this.http.get<Employee[]>(this.url).pipe(retry(1));
  }

  getEmployee(id: number) : Observable<Employee> {
    return this.http.get(this.url + '/' + id).pipe(retry(1));
  }

  createEmployee(employee: Employee) {
    return this.http.post(this.url, employee);
  }
  updateEmployee(employee: Employee) {
    return this.http.put(this.url, employee);
  }
  deleteEmployee(id: number) {
    return this.http.delete(this.url + '/' + id);
  }
}
