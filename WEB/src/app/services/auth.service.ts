import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private backendUrl = 'http://localhost:5230/api';

  constructor(private http: HttpClient) {}

  isAuthenticated(): boolean {
    return !!localStorage.getItem('jwt');
  }

  register(user: any): Observable<any> {
    return this.http.post(`${this.backendUrl}/User/Add`, user);
  }

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.backendUrl}/User/Login`, { username, password }).pipe(
      tap(user => {
        if (!user || !user.username) {
          throw new Error('Invalid login response');
        }
        localStorage.setItem('user', JSON.stringify(user));
      })
    );
  }

  logout() {
    localStorage.removeItem('jwt');
    localStorage.removeItem('user');
  }

  getUserRole(): string | null {
    const user = this.getCurrentUser();
    return user ? user.role : null;
  }

  getCurrentUser(): any {
    const userJson = localStorage.getItem('user');
    return userJson ? JSON.parse(userJson) : null;
  }

  getCurrentUserId(): string | null {
    const user = this.getCurrentUser();
    return user ? user.id : null;
  }
}
