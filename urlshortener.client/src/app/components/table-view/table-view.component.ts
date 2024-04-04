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
  public models: ShortenedUrlTableModel[] = [];
  public fullUrl: string = '';

  private hasUserRole: boolean = false;
  private hasAdminRole: boolean = false;

  constructor(
    private shortenedUrlService: ShortenedUrlService,
    private authService: AuthService,
    private router: Router
  ) {}

  //#region Methods

  public isDeleteAndInfoButtonDisabled() {
    return !this.hasUserRole;
  }

  public isDeleteAllButtonDisabled() {
    return !this.hasAdminRole;
  }

  private getAll() {
    this.shortenedUrlService
      .getAll()
      .then((result: ShortenedUrlTableModel[]) => {
        this.models = result;
      })
      .catch((error) => {
        console.log(error);
      });
  }

  //#endregion

  //#region Triggers

  public infoButtonClicked(model: ShortenedUrlTableModel) {
    if (this.hasUserRole) {
      this.router.navigate(['/info', model.id]);
    }
  }

  public deleteButtonClicked(model: ShortenedUrlTableModel) {
    if (this.hasUserRole) {
      this.shortenedUrlService
        .delete(model.id)
        .then(() => this.getAll())
        .catch((error) => console.log(error));

      this.getAll();
    }
  }

  public deleteAllButtonClicked() {
    if (this.hasUserRole) {
      this.shortenedUrlService
        .deleteAll()
        .then(() => this.getAll())
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
        this.hasUserRole = result === 1 || result === 2;
        this.hasAdminRole = result === 2;
      })
      .catch((error) => {
        this.hasUserRole = false;
        console.log(error);
      });
  }

  //#endregion
}
