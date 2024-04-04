import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ShortenedUrlService } from '../../services/shortened-url/shortened-url.service';
import { ShortenedUrlInfoModel } from '../../models/shortened-url.models';

@Component({
  selector: 'app-info-view',
  templateUrl: './info-view.component.html',
  styleUrl: './info-view.component.css',
})
export class InfoViewComponent {
  public model: ShortenedUrlInfoModel | null = null;

  private id: number = 0;

  constructor(
    private route: ActivatedRoute,
    private shortenedUrlService: ShortenedUrlService
  ) {
    let paramId: string | null = this.route.snapshot.paramMap.get('id');

    if (paramId != null) this.id = +paramId;
  }

  //#region Methods

  private getInfo() {
    this.shortenedUrlService
      .get(this.id)
      .then((result: ShortenedUrlInfoModel) => (this.model = result))
      .catch((error) => console.log(error));
  }

  //#endregion

  //#region Triggers

  public ngOnInit() {
    this.getInfo();
  }

  //#endregion
}
