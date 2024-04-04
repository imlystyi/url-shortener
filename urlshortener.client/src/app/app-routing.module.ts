import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InfoViewComponent } from './components/info-view/info-view.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { TableViewComponent } from './components/table-view/table-view.component';

const routes: Routes = [
  { path: '', component: TableViewComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'info/:id', component: InfoViewComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
