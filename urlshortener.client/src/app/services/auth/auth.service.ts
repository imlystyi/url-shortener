import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { Session } from '../../models/auth.models';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private baseUrl: string = 'https://localhost:7190/api/';

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  public checkPermission(): Promise<number> {
    let id: number = +this.cookieService.get('ius-userId');

    return new Promise<number>((resolve, reject) => {
      this.http
        .get<number>(`${this.baseUrl}user/check-access/${id}`)
        .toPromise()
        .then((result: number | undefined) => {
          if (result) resolve(+result);
          else reject(-1);
        })
        .catch(() => reject(-1));
    });
  }

  public loginByUserIdentities(
    username: string,
    password: string
  ): Promise<string> {
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
          if (error.status === 401) reject('Invalid username and/or password');
          else reject('Server error. Try again later');
        });
    });
  }

  public loginBySessionIdentities(): Promise<boolean> {
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
          if (result) resolve(true);
        })
        .catch(() => reject(false));
    });
  }

  public register(
    inputUsername: string,
    inputEmail: string,
    inputPassword: string
  ): Promise<string> {
    return new Promise<string>((resolve, reject) => {
      this.http
        .post<Session>(`${this.baseUrl}user/register`, {
          role: 1,
          username: inputUsername,
          password: inputEmail,
          email: inputPassword,
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
          if (error.status === 409)
            reject(
              'User with the same email address or username already exists'
            );
          else if (error.status === 400) reject('Invalid data');
          else reject('Server error. Try again later');
        });
    });
  }
}
