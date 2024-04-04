import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ShortenedUrlService } from './services/shortened-url.service';
import { AuthService } from './services/auth.service';

interface ShortenedUrlTableElement {
  id: number;
  fullUrl: string;
  shortUrl: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {

  constructor() {}

  ngOnInit() {
    
  }

  title = 'urlshortener.client';
}
