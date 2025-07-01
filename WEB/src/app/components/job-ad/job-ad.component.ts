import { Component, OnInit } from '@angular/core';
import { CommonModule, NgClass } from '@angular/common';
import { Router } from '@angular/router';
import { JobAdsService } from '../../services/job-ads.service';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { JobApplicationService } from '../../services/job-application.service';

@Component({
  selector: 'app-job-ad',
  standalone: true,
  imports: [CommonModule, NgClass, FormsModule],
  templateUrl: './job-ad.component.html',
  styleUrls: ['./job-ad.component.scss']
})
export class JobAdComponent implements OnInit {
  // User info
  userRole: string | null = null;

  // Pagination controls
  totalItems = 0;
  pageSize = 10;
  currentPage = 1;
  totalPages = 1;
  pages: number[] = [];

  // Job ads list and filters
  jobAds: any[] = [];
  filters = {
    isOpen: null,
    title: '',
    companyName: '',
    publishedOn: null as string | null
  };

  // Applied jobs tracking
  appliedJobAdIds: number[] = [];
  applyingJobId: number | null = null;

  // Alert system for user feedback
  alertMessage: string | null = null;
  alertType: 'success' | 'danger' | 'warning' | null = null;

  // Delete modal and process state
  isDeleting = false;
  deletingJobAdId: number | null = null;
  showDeleteModal = false;

  constructor(
    private jobAdsService: JobAdsService,
    private jobApplicationService: JobApplicationService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    // Initialize user role and load first page of job ads
    this.currentPage = 1;
    this.pageSize = 10;
    this.userRole = this.authService.getUserRole();
    this.loadJobAds(this.currentPage);
  }

  // Show alert messages with auto-dismiss
  showAlert(message: string, type: 'success' | 'danger' | 'warning' = 'success') {
    this.alertMessage = message;
    this.alertType = type;

    setTimeout(() => {
      this.alertMessage = null;
      this.alertType = null;
    }, 4000);
  }

  // Check if user has already applied to a job ad
  isApplied(jobAdId: number): boolean {
    return this.appliedJobAdIds.includes(jobAdId);
  }

  onApply(job: any) {
    
  }

  // Navigation methods
  onAddJobAd() {
    this.router.navigate(['/create-job-ad']);
  }

  onEditJobAd(ad: any): void {
    this.router.navigate(['/edit-job-ad', ad.id]);
  }

  // Delete job ad with confirmation
  onDeleteJobAd(adId: number): void {
    const confirmed = confirm('Сигурни ли сте, че искате да изтриете тази обява?');
    if (confirmed) {
      this.jobAdsService.deleteJobAd(adId).subscribe({
        next: () => {
          this.loadJobAds(this.currentPage);
        },
        error: (err) => {
          console.error('Грешка при изтриването:', err);
          this.showAlert('Грешка при изтриването на обявата.', 'danger');
        }
      });
    }
  }

  // Apply filters and reload job ads list
  applyFilters(): void {
    this.userRole = this.authService.getUserRole();
    this.currentPage = 1;
    this.loadJobAds(this.currentPage);
  }

  loadJobAds(page: number) {
    if (page < 1 || (this.totalPages > 0 && page > this.totalPages)) {
      return;
    }

    this.currentPage = page;

    this.jobAdsService.getJobAds(this.currentPage, this.pageSize, this.filters).subscribe({
      next: (res) => {
        this.jobAds = res.items;
        this.totalItems = res.totalCount;
        this.totalPages = Math.ceil(this.totalItems / this.pageSize);
        this.pages = Array.from({ length: this.totalPages }, (_, i) => i + 1);
      },
      error: (err) => {
        this.showAlert('Грешка при зареждането на обявите.', 'danger');
        console.error(err);
      }
    });
  }

  // Confirm and perform delete action
  delete(adId: number) {
    this.jobAdsService.deleteJobAd(adId).subscribe({
      next: () => {
        this.loadJobAds(this.currentPage);
        this.isDeleting = false;
      },
      error: (err) => {
        console.error('Грешка при изтриването:', err);
        this.showAlert('Грешка при изтриването на обявата.', 'danger');
        this.isDeleting = false;
      }
    });
  }
}
