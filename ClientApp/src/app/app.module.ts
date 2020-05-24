import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { EmployeeComponent } from './employee/employee.component';
import { VocationComponent } from './vocation/vocation.component';
import { VocationCreate } from './vocation/create/vocation-create.component';
import { BadRequestVocation } from './vocation/badrequest/badrequest.component';
import { BadRequestEmployee } from './employee/badrequest/badrequest'
import { EmployeeCreate } from './employee/create/employee-create.component'

@NgModule({ 
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    EmployeeComponent,
    VocationComponent,
    VocationCreate,
    EmployeeCreate,
    BadRequestVocation,
    BadRequestEmployee    
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'employee', component: EmployeeComponent, pathMatch: 'full' },
      { path: 'vocation', component: VocationComponent, pathMatch: 'full' },
      { path: 'vocation/create/:id', component: VocationCreate, pathMatch: 'full' },
      { path: 'employee/create', component: EmployeeCreate, pathMatch: 'full' },
      { path: 'vocation/error', component: BadRequestVocation, pathMatch: 'full' },
      { path: 'employee/error', component: BadRequestEmployee, pathMatch: 'full' }      
    ], { useHash: true })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
