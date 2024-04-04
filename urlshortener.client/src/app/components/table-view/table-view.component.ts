import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ShortenedUrlService, ShortenedUrlTableElement } from '../../services/shortened-url/shortened-url.service';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-table-view',
  templateUrl: './table-view.component.html',
  styleUrl: './table-view.component.css',
})
export class TableViewComponent implements OnInit {
  public shortenedUrls: ShortenedUrlTableElement[] = [];
  private isUserRole: boolean = false;
  private isAdminRole: boolean = false;

  constructor(
    private shortenedUrlService: ShortenedUrlService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.getAll();

    this.authService
      .checkPermission()
      .then((result: number) => {
        this.isUserRole = result === 1 || result === 2;
        this.isAdminRole = result === 2;
      })
      .catch(() => {
        this.isUserRole = false;
      });
  }

  isDeleteAndInfoButtonDisabled() {
    return !this.isUserRole;
  }

  isDeleteAllButtonDisabled() {
    return !this.isAdminRole;
  }

  infoButtonClicked(shortenedUrl: ShortenedUrlTableElement) {
    this.router.navigate(['/info', shortenedUrl.id]);
  }

  deleteButtonClicked(shortenedUrl: ShortenedUrlTableElement) {
    this.shortenedUrlService
      .delete(shortenedUrl.id)
      .then(() => {
        this.getAll();
      })
      .catch((error) => {
        console.log(error);
      });

      this.getAll();
  }

  deleteAllButtonClicked() {
    this.shortenedUrlService
      .deleteAll()
      .then(() => {
        this.getAll();
      })
      .catch((error) => {
        console.log(error);
      });

      this.getAll();
  }

  getAll() {
    this.shortenedUrlService
      .getAll()
      .then((result: ShortenedUrlTableElement[]) => {
        this.shortenedUrls = result;
      })
      .catch((error) => {
        console.log(error);
      });
  }

  title = 'urlshortener.client';
}
