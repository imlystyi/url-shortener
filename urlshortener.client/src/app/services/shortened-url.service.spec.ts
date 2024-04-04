import { TestBed } from '@angular/core/testing';

import { ShortenedUrlService } from './shortened-url.service';

describe('ShortenedUrlService', () => {
  let service: ShortenedUrlService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShortenedUrlService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
