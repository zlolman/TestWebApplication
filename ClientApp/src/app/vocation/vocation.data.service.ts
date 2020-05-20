import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Vocation } from './vocation';
import { Observable, } from 'rxjs';
import { retry } from 'rxjs/operators';

@Injectable()
export class VocationDataService {

  private url = "/vocation";

  constructor(private http: HttpClient) { }

  getVocations() {
    return this.http.get(this.url);
  }

  getVocation(id: number) {
    return this.http.get(this.url + '/' + id);
  }

  createVocation(vocation: Vocation): Observable<Vocation> {
    return this.http.post<Vocation>(this.url, vocation).pipe(retry(1));
  }

  updateVocation(vocation: Vocation) {
    return this.http.put(this.url, vocation);
  }

  deleteVocation(id: number) {
    return this.http.delete(this.url + '/' + id);
  }
}
