import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Vocation } from './vocation';
import { VocationDataService } from './vocation.data.service';

@Component({
  templateUrl: './vocation-create.component.html',
  providers: [VocationDataService]
})
export class VocationCreate {
  vocation: Vocation = new Vocation();
  loaded: boolean;
  //response: HttpResponse<Vocation>;

  constructor(private router: Router, private vocationService: VocationDataService, private activeRoute: ActivatedRoute) {
    this.vocation.employeeId = Number.parseInt(activeRoute.snapshot.params["id"]);
  };


  save() {
    this.vocationService.createVocation(this.vocation).subscribe(data => {
      this.vocation = data;
      if (data != null) {
        this.router.navigateByUrl('/vocation')
      }
    }, error => {
      if (error != null) {
        this.router.navigateByUrl('/vocation/error');
      }
    });
  }
}
