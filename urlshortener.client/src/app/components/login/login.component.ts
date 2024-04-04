import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorOutput: string = '';
  constructor(private http: HttpClient, private authService: AuthService) {}

  submit() {
    this.authService
      .loginByUserIdentities(this.username, this.password)
      .then(() => {
        window.location.href = '/table-view';
      })
      .catch((errorMessage: string) => (this.errorOutput = errorMessage));
  }

  title = 'urlshortener.client';
}
