import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class JobApplicationService {
  private backendUrl = 'http://localhost:5230/api';

  constructor(private http: HttpClient) {}

  getJobApplications(page: number, pageSize: number, filters: any): Observable<any> {
    return this.http.post<any>(`${this.backendUrl}/JobApplication/GetFiltered`, {
      page,
      pageSize,
      filters
    });
  }

  getJobApplicationById(id: number): Observable<any> {
    return this.http.get(`${this.backendUrl}/JobApplication/GetById/${id}`);
  }

  applyToJob(jobAdId: number, userId: number): Observable<any> {
    const payload = {
      jobAdId,
      userId,
      status: 0 // default initial status
    };

    return this.http.post(`${this.backendUrl}/JobApplication/Add`, payload);
  }
}
