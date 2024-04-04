import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  username: string = '';
  password: string = '';
  email: string = '';
  errorOutput: string = '';
  constructor(private authService: AuthService) {}

  submit() {
    this.authService
      .register(this.username, this.password, this.email)
      .then(() => {
        window.location.href = 'table-view';
      })
      .catch((errorMessage: string) => (this.errorOutput = errorMessage));
  }
}
