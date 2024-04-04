import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import {
  ShortenedUrlInfoModel,
  ShortenedUrlTableModel,
} from '../../models/shortened-url.models';

@Injectable({
  providedIn: 'root',
})
export class ShortenedUrlService {
  private baseUrl: string = 'https://localhost:7190/api/';

  constructor(private http: HttpClient, private cookieService: CookieService) {}

  getAll(): Promise<ShortenedUrlTableModel[]> {
    return new Promise<ShortenedUrlTableModel[]>((resolve, reject) =>
      this.http
        .get<ShortenedUrlTableModel[]>(`${this.baseUrl}get-all`)
        .toPromise()
        .then((result: ShortenedUrlTableModel[] | undefined) => {
          if (result) {
            resolve(result);
          }
        })
        .catch(() => reject('Server error. Try again later'))
    );
  }

  get(id: number): Promise<ShortenedUrlInfoModel> {
    return new Promise<ShortenedUrlInfoModel>((resolve, reject) =>
      this.http
        .get<ShortenedUrlInfoModel>(`${this.baseUrl}get-info/${id}`)
        .toPromise()
        .then((result: ShortenedUrlInfoModel | undefined) => {
          if (result) {
            resolve(result);
          }
        })
        .catch(() => reject('Server error. Try again later'))
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
