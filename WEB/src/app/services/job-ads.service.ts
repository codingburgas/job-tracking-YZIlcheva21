import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class JobAdsService {
  private backendUrl = 'http://localhost:5230/api';

  constructor(private http: HttpClient) {}

  getJobAds(page: number, pageSize: number, filters: any): Observable<any> {
    return this.http.post<any>(`${this.backendUrl}/Job/GetFiltered`, {
      page,
      pageSize,
      filters
    });
  }

  createJobAd(jobAd: any): Observable<any> {
    return this.http.post(`${this.backendUrl}/Job/Add`, jobAd);
  }

  getJobAdById(id: number): Observable<any> {
    return this.http.get(`${this.backendUrl}/Job/GetById/${id}`);
  }

  updateJobAd(id: number, jobAd: any): Observable<any> {
    return this.http.put(`${this.backendUrl}/Job/Update/${id}`, jobAd);
  }

  deleteJobAd(adId: number): Observable<void> {
    return this.http.delete<void>(`${this.backendUrl}/Job/Delete/${adId}`);
  }
}