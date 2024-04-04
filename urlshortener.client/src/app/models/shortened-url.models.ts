export interface ShortenedUrlTableModel {
  id: number;
  fullUrl: string;
  shortUrl: string;
}

export interface ShortenedUrlInfoModel {
  id: string;
  authorNickname: string;
  fullUrl: string;
  shortUrl: string;
  clicks: number;
  createdAt: string;
}
