import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {
  ShortenedUrlService,
  ShortenedUrlInfoElement,
} from '../../services/shortened-url.service';

@Component({
  selector: 'app-info-view',
  templateUrl: './info-view.component.html',
  styleUrl: './info-view.component.css',
})
export class InfoViewComponent {
  public shortenedUrl: ShortenedUrlInfoElement | null = null;

  private id: number = 0;

  constructor(
    private route: ActivatedRoute,
    private shortenedUrlService: ShortenedUrlService
  ) {
    let paramId: string | null = this.route.snapshot.paramMap.get('id');

    if (paramId != null) this.id = +paramId;
  }

  ngOnInit() {
    this.getInfo();
  }

  public getInfo() {
    this.shortenedUrlService
      .get(this.id)
      .then((result: ShortenedUrlInfoElement) => {
        this.shortenedUrl = result;
      })
      .catch((error) => {
        console.log(error);
      });
  }
}
