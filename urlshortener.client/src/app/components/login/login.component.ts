import { Component } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  public username: string = '';
  public password: string = '';
  public errorOutput: string = '';

  constructor(private authService: AuthService) {}

  //#region Triggers

  public submitButtonClicked() {
    this.authService
      .loginByUserIdentities(this.username, this.password)
      .then(() => {
        window.location.href = '/';
      })
      .catch((errorMessage: string) => (this.errorOutput = errorMessage));
  }

  //#endregion
}
