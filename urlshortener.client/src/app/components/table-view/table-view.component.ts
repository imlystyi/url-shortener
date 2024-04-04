import { Component, OnInit } from '@angular/core';
import { ShortenedUrlService } from '../../services/shortened-url/shortened-url.service';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import { ShortenedUrlTableModel } from '../../models/shortened-url.models';

@Component({
  selector: 'app-table-view',
  templateUrl: './table-view.component.html',
  styleUrl: './table-view.component.css',
})
export class TableViewComponent implements OnInit {
  public shortenedUrls: ShortenedUrlTableModel[] = [];
  public fullUrl: string = '';

  private isUserRole: boolean = false;
  private isAdminRole: boolean = false;

  constructor(
    private shortenedUrlService: ShortenedUrlService,
    private authService: AuthService,
    private router: Router
  ) {}

  //#region Methods

  public isDeleteAndInfoButtonDisabled() {
    return !this.isUserRole;
  }

  public isDeleteAllButtonDisabled() {
    return !this.isAdminRole;
  }

  private getAll() {
    this.shortenedUrlService
      .getAll()
      .then((result: ShortenedUrlTableModel[]) => {
        this.shortenedUrls = result;
      })
      .catch((error) => {
        console.log(error);
      });
  }

  //#endregion

  //#region Triggers

  public infoButtonClicked(shortenedUrl: ShortenedUrlTableModel) {
    if (this.isUserRole) {
      this.router.navigate(['/info', shortenedUrl.id]);
    }
  }

  public deleteButtonClicked(shortenedUrl: ShortenedUrlTableModel) {
    if (this.isUserRole) {
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
  }

  public deleteAllButtonClicked() {
    if (this.isUserRole) {
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
  }

  public ngOnInit() {
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

  //#endregion
}
