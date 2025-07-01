import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  imports: [CommonModule],
  standalone: true,
})
export class ProfileComponent implements OnInit {
  user: any; // user data
  constructor(private authService: AuthService) {}
  ngOnInit(): void {
    this.user = this.authService.getCurrentUser();
  }
}