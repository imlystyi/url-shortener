import { Component } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  public username: string = '';
  public password: string = '';
  public email: string = '';
  public errorOutput: string = '';

  constructor(private authService: AuthService) {}

  //#region Triggers

  public submitButtonClicked() {
    this.authService
      .register(this.username, this.password, this.email)
      .then(() => (window.location.href = '/'))
      .catch((errorMessage: string) => {
        this.errorOutput = errorMessage;
        console.log(errorMessage);
      });
  }

  //#endregion
}
