import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

interface Session {
  token: string;
  userId: string;
}

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl: string = 'https://localhost:7190/api/';

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  loginByUserIdentities(username: string, password: string): Promise<string> {
    return new Promise<string>((resolve, reject) => {
      this.http
        .post<Session>(`${this.baseUrl}user/login`, {
          username: username,
          password: password,
        })
        .toPromise()
        .then((result: Session | undefined) => {
          if (result) {
            this.cookieService.set('ius-token', result.token);
            this.cookieService.set('ius-userId', result.userId);
            resolve('');
          }
        })
        .catch((error: HttpErrorResponse) => {
          if (error.status === 401) {
            reject('Invalid username and/or password');
          } else {
            reject('Server error. Try again later');
          }
        });
    });
  }

  loginBySessionIdentities(): Promise<boolean> {
    let token: string = this.cookieService.get('ius-token'),
      userId = this.cookieService.get('ius-userId');

    return new Promise<boolean>((resolve, reject) => {
      this.http
        .post<Session>(`${this.baseUrl}user/login/session`, {
          token: token,
          userId: userId,
        })
        .toPromise()
        .then((result: Session | undefined) => {
          if (result) {
            resolve(true);
          }
        })
        .catch((error: HttpErrorResponse) => {
          if (error.status === 401) {
            reject(false);
          } else {
            reject(false);
          }
        });
    });
  }
}