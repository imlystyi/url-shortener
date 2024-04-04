import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';

export interface ShortenedUrlTableElement {
  id: number;
  fullUrl: string;
  shortUrl: string;
}

export interface ShortenedUrlInfoElement {
  id: string;
  authorNickname: string;
  fullUrl: string;
  shortUrl: string;
  clicks: number;
  createdAt: string;
}

@Injectable({
  providedIn: 'root',
})
export class ShortenedUrlService {
  private baseUrl: string = 'https://localhost:7190/api/';

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  getAll(): Promise<ShortenedUrlTableElement[]> {
    return new Promise<ShortenedUrlTableElement[]>((resolve, reject) =>
      this.http
        .get<ShortenedUrlTableElement[]>(`${this.baseUrl}get-all`)
        .toPromise()
        .then((result: ShortenedUrlTableElement[] | undefined) => {
          if (result) {
            resolve(result);
          }
        })
        .catch(() => {
          reject('Server error. Try again later');
        })
    );
  }

  get(id: number): Promise<ShortenedUrlInfoElement> {
    return new Promise<ShortenedUrlInfoElement>((resolve, reject) =>
      this.http
        .get<ShortenedUrlInfoElement>(`${this.baseUrl}get-info/${id}`)
        .toPromise()
        .then((result: ShortenedUrlInfoElement | undefined) => {
          if (result) {
            resolve(result);
          }
        })
        .catch(() => {
          reject('Server error. Try again later');
        })
    );
  }

  delete(id: number): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http
        .delete(`${this.baseUrl}delete/${id}`)
        .toPromise()
        .then(() => resolve(true))
        .catch(() => reject('Server error. Try again later'));
    });
  }

  deleteAll(): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this.http
        .delete(`${this.baseUrl}delete-all`)
        .toPromise()
        .then(() => resolve(true))
        .catch(() => reject('Server error. Try again later'));
    });
  }
}
