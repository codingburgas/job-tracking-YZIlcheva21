import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JobApplicationService } from '../../services/job-application.service';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-job-application',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './job-application.component.html',
  styleUrls: ['./job-application.component.scss']
})
export class JobApplicationComponent implements OnInit {
  userRole: string = 'admin';

  totalItems = 0;
  pageSize = 10;
  currentPage = 1;
  totalPages = 1;
  pages: number[] = [];

  jobApplications: Array<{
    id: number;
    userId: number;
    jobAdId: number;
    status: number;
    user: {
      id: number;
      firstName: string | null;
      middleName: string | null;
      lastName: string | null;
      username: string;
      role: number;
    };
    jobAd: {
      id: number;
      title: string;
      companyName: string;
      description: string | null;
      publishedOn: string;
      isOpen: boolean;
    };
  }> = [];

  filters = {
    status: null as number | null,
    userId: null as number | null,
    jobAdId: null as number | null
  };

  constructor(
    private jobApplicationService: JobApplicationService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    const user = this.authService.getCurrentUser();
    this.userRole = user.role;
    this.currentPage = 1;
    this.pageSize = 10;
    this.loadJobApplications(this.currentPage);
  }

  goToReview(applicationId: number) {
    this.router.navigate(['/job-application-review', applicationId]);
  }

  loadJobApplications(page: number) {
    const user = this.authService.getCurrentUser();
    const isAdmin = user.role === 1;

    // If user is not admin, restrict filtering by their user ID
    const filters = {
      ...this.filters, 
      ...(isAdmin ? {} : { userId: user.id })
    };

    this.jobApplicationService
      .getJobApplications(this.currentPage, this.pageSize, filters)
      .subscribe(response => {
        console.log('Raw response items:', response.items);
        this.jobApplications = response.items;
        console.log('Loaded applications:', this.jobApplications); 

        this.totalItems = response.totalItems;
        this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
      });
  }
}